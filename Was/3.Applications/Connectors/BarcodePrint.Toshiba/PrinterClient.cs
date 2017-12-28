using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kengic.Was.Connector.Common;
using Kengic.Was.CrossCutting.ConfigurationSection.Connectors;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.CrossCutting.Sockets;

namespace Kengic.Was.Connector.BarcodePrinter.Toshiba
{
    public class PrinterClient : IConnector
    {
        public const byte Soh = 0x01;
        public const byte Stx = 0x02;
        private readonly SequenceNumberCreator _sequenceNoCreator = new SequenceNumberCreator();
        private SocketsClientThread _clientThread;
        private string _logName;

        public bool RecSendMsgStatus
        {
            get { return (_clientThread != null) && _clientThread.RecSendMsgStatus; }
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
                _clientThread = new SocketsClientThread
                {
                    Ip = ConnectorElement.Connection.Remote.Ip,
                    Port = ConnectorElement.Connection.Remote.Port,
                    IntervalTime = 6,
                    ProcessIsBegin = true,
                    HeartBeatSyncTime = ConnectorElement.Connection.ConnectTimeOut/2,
                    ThelastReceiveMessageTime = DateTime.Now,
                    ThelastMessageSendTime = DateTime.Now,
                    ConnectStatus = false,
                    RecSendMsgStatus = false,
                    ConnectTimeOut = ConnectorElement.Connection.ConnectTimeOut,
                    LogName = _logName
                };

                _clientThread.OnHearBeat += Client_OnHeartBeat;
                _clientThread.OnReEstablishConnect += Client_ReEstablishConnect;
                _clientThread.OnReceiceMessage += Client_OnReceiveMessage;
                _clientThread.Start();

                ConnectStatus = true;

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
            _clientThread.ProcessIsBegin = false;
            return true;
        }

        public bool SendMessage(List<string> messageList)
        {
            //If connect is not established,don't send any message
            if (!RecSendMsgStatus)
            {
                return false;
            }

            if ((messageList == null) || (messageList.Count < 1))
            {
                return false;
            }
            return SendMessage(messageList[0]);
        }

        public bool SendMessage(string message)
        {
            try
            {
                var messageBytes = SocketsHelper.ConvertStringToBytes(message).ToArray();

                //add message to send queue
                _clientThread.SendMessagesDict.TryAdd(
                    _sequenceNoCreator.GetSequenceNo().ToString(),
                    messageBytes);

                LogRepository.WriteInfomationLog(_logName, StaticParameterForMessage.SendMessage, message);

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
            const int messageLength = 13;

            var receiveMessage = Encoding.ASCII.GetString(infoBytes);
            LogRepository.WriteInfomationLog(_logName, StaticParameterForMessage.ReceiveMessage,
                receiveMessage);
            try
            {
                _clientThread.ThelastReceiveMessageTime = DateTime.Now;
                var messageDict = new ConcurrentDictionary<string, byte[]>();
                if (infoBytes.Length == messageLength)
                {
                    messageDict.TryAdd(_sequenceNoCreator.GetSequenceNo().ToString(), infoBytes);
                }
                else
                {
                    for (var i = 0; i < infoBytes.Length; i++)
                    {
                        if ((infoBytes[i] == Soh) && (infoBytes[i + 1] == Stx))
                        {
                            var receiveBytes = new byte[messageLength];
                            Buffer.BlockCopy(infoBytes, i, receiveBytes, 0, messageLength);
                            messageDict.TryAdd(_sequenceNoCreator.GetSequenceNo().ToString(), receiveBytes);
                            break;
                        }
                    }
                }

                foreach (var messageKey in messageDict.Keys)
                {
                    //var message = Encoding.ASCII.GetString(messageDict[messageKey]);                   
                    int printSatus;
                    int.TryParse(Encoding.ASCII.GetString(messageDict[messageKey], 2, 2), out printSatus);
                    //Todo:请把所有的状态弄成枚举
                    switch (printSatus)
                    {
                        case 0:
                        case 2:
                        case 4:
                        case 27:
                        case 29:
                        case 40:
                        case 41:
                            ConnectStatus = true;
                            RecSendMsgStatus = true;
                            _clientThread.RecSendMsgStatus = true;
                            break;
                        case 11:
                            ReceiveDictionary.TryAdd(messageKey, "PaperJam");
                            RecSendMsgStatus = false;
                            break;
                        case 12:
                            ReceiveDictionary.TryAdd(messageKey, "CutterError");
                            RecSendMsgStatus = false;
                            break;
                        case 13:
                            ReceiveDictionary.TryAdd(messageKey, "NoPaper");
                            RecSendMsgStatus = false;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(_logName, StaticParameterForMessage.ReceiveMessageException,
                    ex.ToString());
            }
        }

        private void Client_OnHeartBeat()
        {
            const string message = "{WS|}";
            SendMessage(message);
        }

        private void Client_ReEstablishConnect()
        {
            const string message = "{WS|}";
            SendMessage(message);
        }
    }
}