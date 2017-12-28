using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Connector.Common;
using Kengic.Was.CrossCutting.ConfigurationSection.Connectors;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.CrossCutting.Sockets;

namespace Kengic.Was.Connector.Vitronic
{
    public class VitronicClient : IConnector
    {
        private readonly VitronicMessage _vitronicMessage = new VitronicMessage();
        private byte[] _beforeBytes = {};
        private string _logName;

        private SocketsClientThread _socketsClientThread;
        private DateTime _thelastReConnectMessageTime;

        public VitronicClient(ConnectorElement connectorElement)
        {
            ConnectorElement = connectorElement;
        }

        public bool RecSendMsgStatus
        {
            get { return (_socketsClientThread != null) && _socketsClientThread.RecSendMsgStatus; }
            set { }
        }

        public ConnectorElement ConnectorElement { get; set; }
        public string Id { get; set; }
        public ConcurrentDictionary<string, object> ReceiveDictionary { get; set; }
        public bool ConnectStatus { get; set; }

        public bool AlarmActiveStatus { get; set; }
        public bool InitializeStatus { get; set; }

        public bool Initialize()
        {
            ReceiveDictionary = new ConcurrentDictionary<string, object>();

            _logName = ConnectorElement.LogName;

            LogRepository.WriteInfomationLog(_logName, StaticParameterForMessage.InitializeConnectSuccess, Id);
            InitializeStatus = true;
            return true;
        }

        public bool Connect()
        {
            try
            {
                if (!InitializeStatus)
                {
                    return false;
                }

                //Start client thread
                _socketsClientThread = new SocketsClientThread
                {
                    RemoteIp = ConnectorElement.Connection.Remote.Ip,
                    RemotePort = ConnectorElement.Connection.Remote.Port,
                    IntervalTime = 6,
                    ProcessIsBegin = true,
                    HeartBeatSyncTime = 30000,
                    ThelastReceiveMessageTime = DateTime.Now,
                    ConnectStatus = false,
                    RecSendMsgStatus = false,
                    ConnectTimeOut = ConnectorElement.Connection.ConnectTimeOut,
                    LogName = _logName
                };

                _socketsClientThread.OnHearBeat += Client_OnHeartBeat;
                _socketsClientThread.OnReEstablishConnect += Client_ReEstablishConnect;
                _socketsClientThread.OnReceiceMessage += Client_OnReceiveMessage;
                _socketsClientThread.Start();

                return true;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(_logName, StaticParameterForMessage.ConnectException, ex.ToString());
                return false;
            }
        }

        public bool DisConnect()
        {
            //Stop client thread
            _socketsClientThread.ProcessIsBegin = false;
            return true;
        }

        public bool SendMessage(List<string> messageList)
        {
            //If connect is not established,don't send any message
            if (!RecSendMsgStatus)
            {
                return false;
            }

            return (messageList.Count == 1) && SendMessage(messageList[0]);
        }

        public bool SendMessage(string message)
        {
            try
            {
                var messageArray = message.Split('|');
                if (messageArray.Count() >= 3)
                {
                    var sequenceNumber = messageArray[0];
                    //add message to send queue

                    var startByte = new byte[] {2};
                    var endByte = new byte[] {3};
                    var crByte = new byte[] {13};


                    var messageBytes =
                        startByte.Concat(SocketsHelper.ConvertStringToBytes(message))
                            .Concat(endByte)
                            .Concat(crByte)
                            .ToArray();

                    //var messageBytes = SocketsHelper.ConvertStringToBytes("STX" + message + "ETX");

                    _socketsClientThread.SendMessagesDict.TryAdd(sequenceNumber, messageBytes);

                    LogRepository.WriteInfomationLog(_logName, StaticParameterForMessage.SendMessage, message);

                    return true;
                }

                LogRepository.WriteInfomationLog(_logName, StaticParameterForMessage.SendMessageFailure,
                    message);
                return false;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(_logName, StaticParameterForMessage.SendMessageException, ex.ToString());
                return false;
            }
        }

        private void Client_OnReceiveMessage(byte[] infoBytes)
        {
            try
            {
                var messageDict = new ConcurrentDictionary<string, string>();

                //var message = Encoding.ASCII.GetString(infoBytes);

                //merge last message
                byte[] vitornicMessageBytes = {};
                if (_beforeBytes.Length > 0)
                {
                    vitornicMessageBytes = vitornicMessageBytes.Concat(_beforeBytes).ToArray();
                    vitornicMessageBytes = vitornicMessageBytes.Concat(infoBytes).ToArray();
                }
                else
                {
                    vitornicMessageBytes = infoBytes;
                }

                _vitronicMessage.GetReceiveMessageForClient(vitornicMessageBytes, ref messageDict, ref _beforeBytes);
                if ((messageDict == null) || (messageDict.Count <= 0))
                {
                    return;
                }
                foreach (var theKeys in messageDict.Keys)
                {
                    _socketsClientThread.ThelastReceiveMessageTime = DateTime.Now;
                    ReceiveDictionary.TryAdd(theKeys, messageDict[theKeys]);

                    LogRepository.WriteInfomationLog(_logName, StaticParameterForMessage.ReceiveMessage,
                        messageDict[theKeys]);

                    var messageArray = messageDict[theKeys].Split('|');

                    if (messageArray.Length < 3)
                    {
                        LogRepository.WriteExceptionLog(ConnectorElement.LogName, StaticParameterForMessage.ErrorMessage,
                            messageDict[theKeys]);
                        continue;
                    }

                    //01 | VCS | STOP
                    //01 | VCS | START
                    if (messageArray[2] == "STOP")
                    {
                        _socketsClientThread.ConnectStatus = true;
                        _socketsClientThread.RecSendMsgStatus = false;
                    }

                    if (messageArray[2] == "START")
                    {
                        _socketsClientThread.ConnectStatus = true;
                        _socketsClientThread.RecSendMsgStatus = true;
                    }
                    //else if(messageArray[2] != "ACK")
                    //{
                    //    var akMessage = messageArray[0] + "|" + messageArray[1] + "|ACK";
                    //    SendMessage(akMessage);
                    //}
                }
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(ConnectorElement.LogName,
                    StaticParameterForMessage.ReceiveMessageException,
                    ex.ToString());
            }
        }

        private void Client_OnHeartBeat()
        {
            var message = GetHeartMessage();
            SendMessage(message);
        }

        private void Client_ReEstablishConnect()
        {
            if (_thelastReConnectMessageTime.Year.ToString() == "1")
            {
                var message = GetHeartMessage();
                SendMessage(message);
                _thelastReConnectMessageTime = DateTime.Now;
                return;
            }
            //if the last ce message is overtime,send again
            var timeSpan = DateTime.Now - _thelastReConnectMessageTime;
            if (timeSpan.TotalMilliseconds < ConnectorElement.Connection.ConnectTimeOut)
            {
                return;
            }
            if (!_socketsClientThread.RecSendMsgStatus)
            {
                var message = GetHeartMessage();
                SendMessage(message);
                _thelastReConnectMessageTime = DateTime.Now;
            }
        }

        public string GetHeartMessage()
        {
            return "40|01|00000000";
        }
    }
}