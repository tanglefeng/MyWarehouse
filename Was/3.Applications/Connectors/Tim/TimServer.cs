using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Kengic.Was.Connector.Common;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection.Connectors;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.CrossCutting.Sockets;
using Kengic.Was.CrossCutting.SocketsServer;
using Newtonsoft.Json;

namespace Kengic.Was.Connector.Tim
{
    public class TimServer : IConnector
    {
        private const string Ak = "AK";
        private const string BcmCe = "BCM_CE";
        private const string BcmTs = "BCM_TS";
        private const string BcsRb = "BCS_RB";
        private const string BcsRe = "BCS_RE";
        private const string BsmRs = "BSM_RS";
        private const string BssMb = "BSS_MB";
        private const string BssMe = "BSS_ME";
        private const string Rcon = "RCON";

        private readonly ConcurrentDictionary<string, TimMessage> _sendMessagesDictionary =
            new ConcurrentDictionary<string, TimMessage>();

        private readonly SequenceNumberCreator _sendSequenceNoCreator = new SequenceNumberCreator();
        private readonly TimHelper _timHelper = new TimHelper();
        private byte[] _beforeBytes = {};
        private string _logName;
        private SocketServer _socketsServer;
        private TimSection _timSection;
        private SocketsServerThread _timServerThread;
        public ConnectorElement ConnectorElement { get; set; }
        public string Id { get; set; }

        public ConcurrentDictionary<string, string> ReceiveDictionary { get; set; } =
            new ConcurrentDictionary<string, string>();

        public bool RecSendMsgStatus { get; set; }
        public bool ConnectStatus { get; set; }
        public bool AlarmActiveStatus { get; set; }
        public bool InitializeStatus { get; set; }

        public bool Initialize()
        {
            _timSection = ConfigurationOperation<TimSection>.GetCustomSection(ConnectorElement.Connection.FilePath,
                ConnectorElement.Connection.SectionName);
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

                var port = ConnectorElement.Connection.Local.Port;
                var ipAddress = ConnectorElement.Connection.Local.Ip;
                _socketsServer = new SocketServer(100, ipAddress, port);
                if (!_socketsServer.StartService())
                {
                    return false;
                }


                _socketsServer.ProcessAcceptComplete += Server_ProcessAcceptComplete; //客户端连接完成事件
                _socketsServer.ReceiveMsgHandler += Server_ReceiveMsgHandler;

                _timServerThread = new SocketsServerThread
                {
                    IntervalTime = 6,
                    SocketsServer = _socketsServer,
                    ProcessIsBegin = true
                };
                _timServerThread.Start();

                ConnectStatus = true;
                RecSendMsgStatus = true;


                LogRepository.WriteInfomationLog(_logName, StaticParameterForMessage.ConnectSuccess,
                    ipAddress + ":" + port);

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
            _timServerThread.ProcessIsBegin = false;
            return true;
        }

        public bool SendMessage(List<string> messageList)
        {
            if (messageList.Count != 2)
            {
                return false;
            }

            var operationId = messageList[0];
            var message = messageList[1];

            var timMessage = CreateMesssage(operationId, message);
            return SendMessage(timMessage);
        }

        private static void Server_ProcessAcceptComplete(object sender, SocketAsyncEventArgs e)
        {
        }

        private void Server_ReceiveMsgHandler(string uid, string info, byte[] infoBytes)
        {
            try
            {
                var timMessageDict = new ConcurrentDictionary<string, TimMessage>();
                byte[] timMessageBytes = {};

                if (_beforeBytes.Length > 0)
                {
                    timMessageBytes = timMessageBytes.Concat(_beforeBytes).ToArray();
                    timMessageBytes = timMessageBytes.Concat(infoBytes).ToArray();
                }
                else
                {
                    timMessageBytes = infoBytes;
                }

                _timHelper.GetReceiveMessage(timMessageBytes, _timSection, ref timMessageDict,
                    ref _beforeBytes, _logName);
                if ((timMessageDict == null) || (timMessageDict.Count <= 0))
                {
                    return;
                }
                foreach (var timMessage in timMessageDict.Values)
                {
                    ReceiveDictionary.TryAdd(timMessage.Id, JsonConvert.SerializeObject(timMessage));

                    LogRepository.WriteInfomationLog(_logName, StaticParameterForMessage.ReceiveMessage,
                        _timHelper.ConvertMessageToString(timMessage));

                    if (timMessage.TimHeader.FlowControl == Ak)
                    {
                        //if receiving AK message,remove the mesage from send message queue
                        var sequenceNo = timMessage.TimHeader.SequenceNumber;
                        TimMessage removeTimMessage;
                        _sendMessagesDictionary.TryRemove(sequenceNo, out removeTimMessage);
                    }
                    else
                    {
                        //if receive message from plc,and send AK mesage to PLC
                        var timHeaderForAk = _timHelper.CreateHeader(ConnectorElement.Connection);
                        var timMessageForAk = _timHelper.CreateAkMessage(timHeaderForAk, timMessage.TimHeader);
                        SendMessage(timMessageForAk);

                        switch (timMessage.TimHeader.OperationId)
                        {
                            case BcmCe:
                                var bcsRbTimMessage = GetBCS_RB();
                                SendMessage(bcsRbTimMessage);
                                break;
                            case BcmTs:
                                var bcsReTimMessage = GetBCS_RE();
                                SendMessage(bcsReTimMessage);
                                ConnectStatus = true;
                                break;
                            case BsmRs:
                                var bssMbTimMessage = GetBSS_MB();
                                SendMessage(bssMbTimMessage);

                                Thread.Sleep(20);

                                var bssMeTimMessage = GetBSS_ME();
                                SendMessage(bssMeTimMessage);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(_logName, StaticParameterForMessage.ReceiveMessageException,
                    ex.ToString());
            }
        }

        public bool SendMessage(TimMessage timMessage)
        {
            try
            {
                if (timMessage.TimHeader.FlowControl != Ak)
                {
                    timMessage.TimHeader.SequenceNumber = _sendSequenceNoCreator.GetSequenceNo().ToString("0000");
                }

                timMessage = _timHelper.BulidMessage(timMessage);

                var timMessageBytes = _timHelper.ConvertMessageToBytes(timMessage);

                _timServerThread.SendMessagesDict.TryAdd(
                    timMessage.Id,
                    timMessageBytes);
                _sendMessagesDictionary.TryAdd(timMessage.TimHeader.SequenceNumber, timMessage);

                LogRepository.WriteInfomationLog(_logName, StaticParameterForMessage.SendMessage,
                    _timHelper.ConvertMessageToString(timMessage));


                return true;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(_logName, StaticParameterForMessage.SendMessageException, ex.ToString());

                return false;
            }
        }

        public TimMessage GetBCS_RB()
        {
            var timHeader = _timHelper.CreateHeader(ConnectorElement.Connection);
            timHeader.OperationId = BcsRb;

            var timBodyElement = _timHelper.GetBodyElement(_timSection, timHeader.OperationId);
            var timBody = _timHelper.CreateBody(timBodyElement);

            timBody.TimBodyFieldList[0].Value = Convert.ToUInt16(timHeader.SourceNode).ToString();
            timBody.TimBodyFieldList[1].Value = Rcon;
            timBody.TimBodyFieldList[2].Value = "30000";
            timBody.TimBodyFieldList[3].Value = "1";

            var timMessage = new TimMessage(SocketsHelper.GetUfoId())
            {
                TimHeader = timHeader,
                MessageTime = DateTime.Now
            };
            timMessage.TimBodyList.Add(timBody);

            return timMessage;
        }

        public TimMessage GetBCS_RE()
        {
            var timHeader = _timHelper.CreateHeader(ConnectorElement.Connection);
            timHeader.OperationId = BcsRe;

            var timBodyElement = _timHelper.GetBodyElement(_timSection, timHeader.OperationId);
            var timBody = _timHelper.CreateBody(timBodyElement);

            timBody.TimBodyFieldList[0].Value = Convert.ToUInt16(timHeader.SourceNode).ToString();
            timBody.TimBodyFieldList[1].Value = Rcon;

            var timMessage = new TimMessage(SocketsHelper.GetUfoId())
            {
                TimHeader = timHeader,
                MessageTime = DateTime.Now
            };
            timMessage.TimBodyList.Add(timBody);

            return timMessage;
        }

        private TimMessage GetBSS_ME()
        {
            var timHeader = _timHelper.CreateHeader(ConnectorElement.Connection);
            timHeader.OperationId = BssMe;

            var timBodyElement = _timHelper.GetBodyElement(_timSection, timHeader.OperationId);
            var timBody = _timHelper.CreateBody(timBodyElement);

            timBody.TimBodyFieldList[0].Value = Convert.ToUInt16(timHeader.SourceNode).ToString();

            var timMessage = new TimMessage(SocketsHelper.GetUfoId())
            {
                TimHeader = timHeader,
                MessageTime = DateTime.Now
            };
            timMessage.TimBodyList.Add(timBody);

            return timMessage;
        }

        private TimMessage GetBSS_MB()
        {
            var timHeader = _timHelper.CreateHeader(ConnectorElement.Connection);
            timHeader.OperationId = BssMb;

            var timBodyElement = _timHelper.GetBodyElement(_timSection, timHeader.OperationId);
            var timBody = _timHelper.CreateBody(timBodyElement);

            timBody.TimBodyFieldList[0].Value = Convert.ToUInt16(timHeader.SourceNode).ToString();

            var timMessage = new TimMessage(SocketsHelper.GetUfoId())
            {
                TimHeader = timHeader,
                MessageTime = DateTime.Now
            };
            timMessage.TimBodyList.Add(timBody);

            return timMessage;
        }

        public TimMessage CreateMesssage(string operationId, string message)
            => _timHelper.CreateMesssage(operationId, message, ConnectorElement.Connection, _timSection);
    }
}