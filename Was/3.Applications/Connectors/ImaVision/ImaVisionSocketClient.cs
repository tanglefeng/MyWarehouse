using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Connector.Common;
using Kengic.Was.CrossCutting.ConfigurationSection.Connectors;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.CrossCutting.Sockets;

namespace Kengic.Was.Connector.ImaVision
{
    public class ImaVisionSocketClient : IConnector
    {
        private readonly MessageHelper _JdMessage = new MessageHelper();
        private byte[] _beforeBytes = {};
        private string _logName;
        private SocketsClientThread _socketsClientThread;

        public ImaVisionSocketClient(ConnectorElement connectorElement)
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
                    LocalIp = ConnectorElement.Connection.Local.Ip,
                    LocalPort = ConnectorElement.Connection.Local.Port,
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

                //_socketsClientThread.OnHearBeat += Client_OnHeartBeat;
                //_socketsClientThread.OnReEstablishConnect += Client_ReEstablishConnect;
                _socketsClientThread.OnReceiceMessage += Client_OnReceiveMessage;
                _socketsClientThread.Start();

                ConnectStatus = true;
                _socketsClientThread.RecSendMsgStatus = true;

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


                    var sendMessageBytes =
                        startByte.Concat(SocketsHelper.ConvertStringToBytes(message)).Concat(endByte).ToArray();

                    _socketsClientThread.SendMessagesDict.TryAdd(sequenceNumber, sendMessageBytes);

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

                //merge last message
                byte[] messageBytes = {};
                if (_beforeBytes.Length > 0)
                {
                    messageBytes = messageBytes.Concat(_beforeBytes).ToArray();
                    messageBytes = messageBytes.Concat(infoBytes).ToArray();
                }
                else
                {
                    messageBytes = infoBytes;
                }

                _JdMessage.GetReceiveMessageForClient(messageBytes, ref messageDict, ref _beforeBytes);
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

                    if (messageArray.Length < 2)
                    {
                        LogRepository.WriteExceptionLog(ConnectorElement.LogName, StaticParameterForMessage.ErrorMessage,
                            messageDict[theKeys]);
                        continue;
                    }
                 
                    if (messageArray[2] == "PING")
                    {
                        var akMessage = "|"+messageArray[1] + "|ACK|";
                        SendMessage(akMessage);
                        ConnectStatus = true;
                        _socketsClientThread.RecSendMsgStatus = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(ConnectorElement.LogName,
                    StaticParameterForMessage.ReceiveMessageException,
                    ex.ToString());
            }
        }
    }
}