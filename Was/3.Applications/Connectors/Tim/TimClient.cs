using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Connector.Common;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection.Connectors;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.CrossCutting.Sockets;
using Newtonsoft.Json;

namespace Kengic.Was.Connector.Tim
{
    public class TimClient : IConnector
    {
        private const string BcmCe = "BCM_CE";
        private const string RmmRq = "RMM_RQ";
        private const string RmsEn = "RMS_EN";
        private const string BcsRb = "BCS_RB";
        private const string BcsRe = "BCS_RE";
        private const string BssMe = "BSS_ME";
        private const string BcmTs = "BCM_TS";
        private const string BcmAl = "BCM_AL";
        private const string BsmRs = "BSM_RS";
        private const string Ak = "AK";
        private const string SrmTim = "SRM";

        private readonly ConcurrentDictionary<string, TimMessage> _sendMessagesDictionary =
            new ConcurrentDictionary<string, TimMessage>();

        private readonly SequenceNumberCreator _sendSequenceNoCreator = new SequenceNumberCreator();
        private readonly TimHelper _timHelper = new TimHelper();
        private byte[] _beforeBytes = {};
        private int _heartBeatSyncTime;
        private string _logName;
        private DateTime _thelastCeMessageTime;
        private SocketsClientThread _timClientThread;
        private TimSection _timSection;

        public bool RecSendMsgStatus
        {
            get { return (_timClientThread != null) && _timClientThread.RecSendMsgStatus; }
            set { }
        }

        public ConnectorElement ConnectorElement { get; set; }
        public string Id { get; set; }

        public ConcurrentDictionary<string, string> ReceiveDictionary { get; set; } =
            new ConcurrentDictionary<string, string>();

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

                //Start client thread
                _timClientThread = new SocketsClientThread
                {
                    Ip = ConnectorElement.Connection.Remote.Ip,
                    Port = ConnectorElement.Connection.Remote.Port,
                    IntervalTime = 6,
                    ProcessIsBegin = true,
                    HeartBeatSyncTime = _heartBeatSyncTime,
                    ThelastReceiveMessageTime = DateTime.Now,
                    ConnectStatus = false,
                    RecSendMsgStatus = false,
                    ConnectTimeOut = ConnectorElement.Connection.ConnectTimeOut,
                    LogName = _logName
                };

                _timClientThread.OnHearBeat += Client_OnHeartBeat;
                _timClientThread.OnReEstablishConnect += Client_ReEstablishConnect;
                _timClientThread.OnReceiceMessage += Client_OnReceiveMessage;
                _timClientThread.Start();

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
            _timClientThread.ProcessIsBegin = false;
            return true;
        }

        public bool SendMessage(List<string> messageList)
        {
            //If connect is not established,don't send any message
            if (!RecSendMsgStatus)
            {
                return false;
            }

            if (messageList.Count != 2)
            {
                return false;
            }
            //the first string is operation id
            //the sceond string is message
            var operationId = messageList[0];
            var message = messageList[1];

            var timMessage = CreateMesssage(operationId, message);
            return SendMessage(timMessage);
        }

        public bool SendMessage(TimMessage timMessage)
        {
            try
            {
                //If the message is not ak message,get a sequence of this mesage
                if (timMessage.TimHeader.FlowControl != Ak)
                {
                    if ((ConnectorElement.TimType == SrmTim) && (timMessage.TimHeader.OperationId == BcmCe))
                    {
                        _sendSequenceNoCreator.SetSequenceZero();
                    }

                    timMessage.TimHeader.SequenceNumber = _sendSequenceNoCreator.GetSequenceNo().ToString("0000");
                }

                timMessage = _timHelper.BulidMessage(timMessage);

                var timMessageBytes = _timHelper.ConvertMessageToBytes(timMessage);

                if (ConnectorElement.TimMessageLength > 0)
                {
                    if (timMessageBytes.Length < ConnectorElement.TimMessageLength)
                    {
                        timMessageBytes =
                            timMessageBytes.Concat(SocketsHelper.FillWithSpaceChar(ConnectorElement.TimMessageLength -
                                                                                   timMessageBytes.Length)).ToArray();
                    }
                }

                //add message to send queue
                _timClientThread.SendMessagesDict.TryAdd(
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

        private void Client_OnReceiveMessage(byte[] infoBytes)
        {
            try
            {
                var timMessageDict = new ConcurrentDictionary<string, TimMessage>();

                //merge last message
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
                    _timClientThread.ThelastReceiveMessageTime = DateTime.Now;

                    LogRepository.WriteInfomationLog(_logName, StaticParameterForMessage.ReceiveMessage,
                        _timHelper.ConvertMessageToString(timMessage));

                    if (timMessage.TimHeader.FlowControl == Ak)
                    {
                        //if receiving AK message,remove the mesage from send message queue
                        var sequenceNo = timMessage.TimHeader.SequenceNumber;
                        TimMessage removeTimMessage;
                        _sendMessagesDictionary.TryRemove(sequenceNo, out removeTimMessage);
                        ReceiveDictionary.TryAdd(timMessage.Id, JsonConvert.SerializeObject(removeTimMessage));
                    }
                    else
                    {
                        //if receive message from plc,and send AK mesage to PLC
                        ReceiveMessage(timMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(_logName, StaticParameterForMessage.ReceiveMessageException,
                    ex.ToString());
            }
        }

        private void ReceiveMessage(TimMessage timMessage)
        {
            //if receive message from plc,and send AK mesage to PLC
            ReceiveDictionary.TryAdd(timMessage.Id, JsonConvert.SerializeObject(timMessage));

            var timHeaderForAk = _timHelper.CreateHeader(ConnectorElement.Connection);
            var timMessageForAk = _timHelper.CreateAkMessage(timHeaderForAk, timMessage.TimHeader);
            SendMessage(timMessageForAk);

            switch (timMessage.TimHeader.OperationId)
            {
                case BcsRb:
                    _heartBeatSyncTime = Convert.ToInt32(timMessage.TimBodyList[0].TimBodyFieldList[2].Value);
                    _timClientThread.HeartBeatSyncTime = _heartBeatSyncTime;
                    var bcmTsTimMessage = GetBCM_TS();
                    SendMessage(bcmTsTimMessage);
                    break;
                case BcsRe:
                    if (ConnectorElement.TimType == "SRM")
                    {
                        var rmmRqTimMessage = GetRMM_RQ();
                        SendMessage(rmmRqTimMessage);
                    }
                    else
                    {
                        var bsmRsTimMessage = GetBSM_RS();
                        SendMessage(bsmRsTimMessage);
                    }

                    break;
                case BssMe:
                    _timClientThread.RecSendMsgStatus = true;
                    break;
                case RmsEn:
                    _timClientThread.RecSendMsgStatus = true;
                    break;
            }
        }

        private void Client_OnHeartBeat()
        {
            var timMessage = GetBCM_AL();
            SendMessage(timMessage);
        }

        private void Client_ReEstablishConnect()
        {
            if (_thelastCeMessageTime.Year.ToString() == "1")
            {
                var timMessage = GetBCM_CE();
                SendMessage(timMessage);
                _thelastCeMessageTime = DateTime.Now;
                return;
            }
            //if the last ce message is overtime,send again
            var timeSpan = DateTime.Now - _thelastCeMessageTime;
            if (timeSpan.TotalMilliseconds < ConnectorElement.Connection.ConnectTimeOut)
            {
                return;
            }
            if (!_timClientThread.RecSendMsgStatus)
            {
                var timMessage = GetBCM_CE();
                SendMessage(timMessage);
                _thelastCeMessageTime = DateTime.Now;
            }
        }

        public TimMessage GetBCM_CE()
        {
            var timHeader = _timHelper.CreateHeader(ConnectorElement.Connection);
            timHeader.OperationId = BcmCe;

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

        public TimMessage GetBCM_TS()
        {
            var timHeader = _timHelper.CreateHeader(ConnectorElement.Connection);
            timHeader.OperationId = BcmTs;

            var timBodyElement = _timHelper.GetBodyElement(_timSection, timHeader.OperationId);
            var timBody = _timHelper.CreateBody(timBodyElement);

            timBody.TimBodyFieldList[0].Value = Convert.ToUInt16(timHeader.SourceNode).ToString();
            timBody.TimBodyFieldList[1].Value = DateTime.Now.ToString(SocketsHelper.Windowffffff);

            var timMessage = new TimMessage(SocketsHelper.GetUfoId())
            {
                TimHeader = timHeader,
                MessageTime = DateTime.Now
            };
            timMessage.TimBodyList.Add(timBody);

            return timMessage;
        }

        public TimMessage GetBCM_AL()
        {
            var timHeader = _timHelper.CreateHeader(ConnectorElement.Connection);
            timHeader.OperationId = BcmAl;

            var timBodyElement = _timHelper.GetBodyElement(_timSection, timHeader.OperationId);
            var timBody = _timHelper.CreateBody(timBodyElement);

            timBody.TimBodyFieldList[0].Value = Convert.ToUInt16(timHeader.DestinationNode).ToString();

            var timMessage = new TimMessage(SocketsHelper.GetUfoId())
            {
                TimHeader = timHeader,
                MessageTime = DateTime.Now
            };
            timMessage.TimBodyList.Add(timBody);
            return timMessage;
        }

        public TimMessage GetBSM_RS()
        {
            var timHeader = _timHelper.CreateHeader(ConnectorElement.Connection);
            timHeader.OperationId = BsmRs;

            var timBodyElement = _timHelper.GetBodyElement(_timSection, timHeader.OperationId);
            var timBody = _timHelper.CreateBody(timBodyElement);

            timBody.TimBodyFieldList[0].Value = Convert.ToUInt16(timHeader.DestinationNode).ToString();

            var timMessage = new TimMessage(SocketsHelper.GetUfoId())
            {
                TimHeader = timHeader,
                MessageTime = DateTime.Now
            };
            timMessage.TimBodyList.Add(timBody);
            return timMessage;
        }

        public TimMessage GetRMM_RQ()
        {
            var timHeader = _timHelper.CreateHeader(ConnectorElement.Connection);
            timHeader.OperationId = RmmRq;

            var timBodyElement = _timHelper.GetBodyElement(_timSection, timHeader.OperationId);
            var timBody = _timHelper.CreateBody(timBodyElement);

            timBody.TimBodyFieldList[0].Value = timHeader.SourceNode;

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