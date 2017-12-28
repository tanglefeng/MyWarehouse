using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.CrossCutting.TcpService;

namespace Kengic.Was.CrossCutting.Sockets
{
    public class SocketsClientThread
    {
        public delegate void HearBeatHander();

        public delegate void ReceiceMessage(byte[] messageBytes);

        public delegate void ReEstablishConnectHander();

        private DateTime _theLastConnectTime;

        public ConcurrentDictionary<string, byte[]> SendMessagesDict =
            new ConcurrentDictionary<string, byte[]>();

        public DateTime ThelastMessageSendTime;
        public DateTime ThelastReceiveMessageTime;
        public bool ProcessIsBegin { get; set; }
        public bool ProcessIsClosed { get; set; }
        public int IntervalTime { get; set; }
        public int ConnectTimeOut { get; set; }
        public TcpConnection TcpConnection { get; set; }
        public int HeartBeatSyncTime { get; set; }
        public string LocalIp { get; set; }
        public int LocalPort { get; set; }
        public string RemoteIp { get; set; }
        public int RemotePort { get; set; }
        public string LogName { get; set; }
        public bool ConnectStatus { get; set; }
        public bool RecSendMsgStatus { get; set; }
        public event HearBeatHander OnHearBeat;
        public event ReEstablishConnectHander OnReEstablishConnect;
        public event ReceiceMessage OnReceiceMessage;

        public void Start()
        {
            if (IntervalTime == 0)
            {
                IntervalTime = 10;
            }

            var theThread = new Thread(RunThread) {IsBackground = true};
            theThread.Start();
        }

        public void RunThread()
        {
            while (ProcessIsBegin)
            {
                TcpClient tcpClient = null;
                try
                {
                    ProcessIsClosed = false;
                    if (!ConnectStatus)
                    {
                        if (!string.IsNullOrEmpty(LocalIp) && (LocalPort != 0))
                        {
                            //TcpClient tcpClient;
                            if (_theLastConnectTime == new DateTime())
                            {
                                _theLastConnectTime = DateTime.Now;
                                var ipaddress = IPAddress.Parse(LocalIp);
                                var ipendPoint = new IPEndPoint(ipaddress, LocalPort);
                                tcpClient = new TcpClient(ipendPoint);
                                tcpClient.Connect(RemoteIp, RemotePort);
                                TcpConnection = new TcpConnection(tcpClient);
                                
                            }
                            else
                            {
                                var timeSpan = DateTime.Now - _theLastConnectTime;
                                if (timeSpan.TotalMilliseconds >= ConnectTimeOut)
                                {
                                    _theLastConnectTime = DateTime.Now;
                                    var ipaddress = IPAddress.Parse(LocalIp);
                                    var ipendPoint = new IPEndPoint(ipaddress, LocalPort);
                                    tcpClient = new TcpClient(ipendPoint);
                                    tcpClient.Connect(RemoteIp, RemotePort);
                                    TcpConnection = new TcpConnection(tcpClient);
                                   
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            tcpClient = new TcpClient(RemoteIp, RemotePort);
                            TcpConnection = new TcpConnection(tcpClient);
                        }
                        //if socket is disconnect,continue to try agian
                        TcpConnection.Start();
                        ConnectStatus = true;
                        TcpConnection.DataGot += TcpConnectionOnDataGot;
                        TcpConnection.ClientMiss += TcpConnection_ClientMiss;
                        LogRepository.WriteInfomationLog(LogName, StaticParameterForMessage.ConnectIsEstablished,
                            RemoteIp + ":" + RemotePort);

                        //if socket is connect,try to establish tim connect
                        Thread.Sleep(20);
                        if (OnReEstablishConnect == null)
                        {
                            continue;
                        }
                        ReEstablishConnect();
                    }
                    else
                    {
                        SendMessage();

                        if (ConnectStatus && !RecSendMsgStatus)
                        {
                            //if socket is connect and tim message is not established,try angin
                            if (OnReEstablishConnect == null)
                            {
                                continue;
                            }
                            ReEstablishConnect();
                        }
                        else
                        {
                            //链接成功 持续链接
                            if (RecSendMsgStatus && (ThelastMessageSendTime != new DateTime()) &&
                                (HeartBeatSyncTime > 0))
                            {
                                HeartBeatMonitor();
                            }


                            if (RecSendMsgStatus && (ThelastMessageSendTime != new DateTime()) && (ConnectTimeOut > 0))
                            {
                                //接收的最后时间与开始时间的差 与最长链接时间对照
                                ReConnectMonitor();
                            }
                        }
                    }
                    Thread.Sleep(IntervalTime);
                }
                catch (Exception ex)
                {
                    LogRepository.WriteErrorLog(LogName, StaticParameterForMessage.ConnectException,
                        ex.ToString());
                    tcpClient?.Dispose();
                    TcpConnection?.Stop();
                    ConnectStatus = false;
                    RecSendMsgStatus = false;
                    ProcessIsClosed = true;
                }
            }

            ProcessIsClosed = true;
        }

        private void TcpConnection_ClientMiss(object sender, TcpConnectEventArgs e) => ReConnectMonitor();

        private void TcpConnectionOnDataGot(object sender, NetworkDataEventArgs networkDataEventArgs)
            => OnReceiceMessage?.Invoke(networkDataEventArgs.Data);

        internal void HeartBeatMonitor()
        {
            var timeSpan = DateTime.Now - ThelastMessageSendTime;
            if (!(timeSpan.TotalMilliseconds >= HeartBeatSyncTime))
            {
                return;
            }
            OnHearBeat?.Invoke();
        }

        internal void ReEstablishConnect()
        {
            var timeSpan = DateTime.Now - ThelastMessageSendTime;
            if (!(timeSpan.TotalMilliseconds >= ConnectTimeOut))
            {
                return;
            }
            OnReEstablishConnect?.Invoke();
        }

        internal void SendMessage()
        {
            var removeList = new List<string>();
            foreach (var messageKey in SendMessagesDict.Keys.OrderBy(e => e).ToList())
            {
                if (TcpConnection != null)
                {
                    TcpConnection.SendData(SendMessagesDict[messageKey], null);
                    ThelastMessageSendTime = DateTime.Now;
                }

                removeList.Add(messageKey);
            }

            if (removeList.Count <= 0)
            {
                return;
            }
            foreach (var messageKey in removeList)
            {
                byte[] newByte;
                SendMessagesDict.TryRemove(messageKey, out newByte);
            }
        }

        internal void ReConnectMonitor()
        {
            var timeSpan = DateTime.Now - ThelastReceiveMessageTime;
            if (!(timeSpan.TotalMilliseconds >= ConnectTimeOut))
            {
                return;
            }

            TcpConnection?.Stop();
            ConnectStatus = false;
            RecSendMsgStatus = false;

            LogRepository.WriteErrorLog(LogName, StaticParameterForMessage.Disconnect,
                RemoteIp + ":" + RemotePort);
        }
    }
}