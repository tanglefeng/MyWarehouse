using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kengic.Was.Connector.Common;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection.Connectors;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.CrossCutting.Sockets;
using Newtonsoft.Json.Linq;

namespace Kengic.Was.Connector.Scs
{
    public class ScsClient : IConnector
    {
        private const string Hk5 = "5_HK";
        //RequestDestinations 请求地址标志
        private const string Pd5 = "5_PD";
        private const string Pd51 = "5_PD1";
        private const string Pd52 = "5_PD2";
        private readonly ScsHelper _scsHelper = new ScsHelper();
        private readonly SequenceNumberCreator _sendSequenceNoCreator = new SequenceNumberCreator();
        private string _logName;
        private SocketsClientThread _scsClientThread;
        private ScsSection _scsSection;
        public ConnectorElement ConnectorElement { get; set; }
        public string Id { get; set; }

        public ConcurrentDictionary<string, object> ReceiveDictionary { get; set; } =
            new ConcurrentDictionary<string, object>();

        public bool RecSendMsgStatus { get; set; }
        public bool ConnectStatus { get; set; }
        public bool AlarmActiveStatus { get; set; }
        public bool InitializeStatus { get; set; }

        public bool Initialize()
        {
            _scsSection = ConfigurationOperation<ScsSection>.GetCustomSection(ConnectorElement.Connection.FilePath,
                ConnectorElement.Connection.SectionName);
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


                _logName = ConnectorElement.LogName;


                //Start message send thread
                _scsClientThread = new SocketsClientThread
                {
                    RemoteIp = ConnectorElement.Connection.Remote.Ip,
                    RemotePort = ConnectorElement.Connection.Remote.Port,
                    IntervalTime = 6,
                    ProcessIsBegin = true,
                    ThelastReceiveMessageTime = DateTime.Now,
                    ConnectStatus = false,
                    ConnectTimeOut = ConnectorElement.Connection.ConnectTimeOut,
                    LogName = _logName
                };

                _scsClientThread.OnReceiceMessage += Client_OnReceiveMessage;

                _scsClientThread.Start();

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
            _scsClientThread.ProcessIsBegin = false;
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

            var scsMessage = CreateMesssage(operationId, message);
            return SendMessage(scsMessage);
        }

        public ScsClientMessage Get5_HK()
        {
            var scsHeader = _scsHelper.CreateClientHeader(ConnectorElement.Connection.Local.Node, Hk5);

            var scsBodyElement = _scsHelper.GetBodyElement(_scsSection, scsHeader.OperationId);
            var scsBody = _scsHelper.CreateBody(scsBodyElement);

            scsBody.ScsBodyFieldList[0].Value = scsHeader.StartChar;
            scsBody.ScsBodyFieldList[1].Value = scsHeader.SourceId;

            var scsClientMessage = new ScsClientMessage(SocketsHelper.GetUfoId())
            {
                ScsHeader = scsHeader,
                MessageTime = DateTime.Now
            };
            scsClientMessage.ScsBodyList.Add(scsBody);

            return scsClientMessage;
        }

        public bool SendMessage(ScsClientMessage scsMessage)
        {
            try
            {
                //Set telegram sequence number
                scsMessage.ScsHeader.Index = _sendSequenceNoCreator.GetSequenceNo().ToString("0000");

                scsMessage = _scsHelper.BulidClientMessage(scsMessage);

                var scsMessageBytes = scsMessage.ScsHeader.OperationId == Hk5
                    ? _scsHelper.ConvertBodyToBytes(scsMessage.ScsBodyList[0])
                    : _scsHelper.ConvertScsMessageToBytes(scsMessage);

                scsMessageBytes = scsMessageBytes.Concat(SocketsHelper.ConvertStringToBytes(")")).ToArray();

                _scsClientThread.SendMessagesDict.TryAdd(
                    scsMessage.Id,
                    scsMessageBytes);

                LogRepository.WriteInfomationLog(_logName, StaticParameterForMessage.SendMessage,
                    _scsHelper.ConvertClientMessageToString(scsMessage));

                return true;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(_logName, StaticParameterForMessage.SendMessageException, ex.ToString());
                return false;
            }
        }

        internal void Client_OnReceiveMessage(byte[] infoBytes)
        {
            var scsMessageDict = new ConcurrentDictionary<string, ScsServerMessage>();

            var message = Encoding.Default.GetString(infoBytes);

            _scsHelper.GetReceiveMessageForClient(message, _scsSection, ref scsMessageDict);
            if ((scsMessageDict == null) || (scsMessageDict.Count <= 0))
            {
                return;
            }
            foreach (var scsMessage in scsMessageDict.Values)
            {
                _scsClientThread.ThelastReceiveMessageTime = DateTime.Now;
                ReceiveDictionary.TryAdd(scsMessage.Id, _scsHelper.ConvertServerMessageToString(scsMessage));

                LogRepository.WriteInfomationLog(_logName, StaticParameterForMessage.ReceiveMessage,
                    _scsHelper.ConvertServerMessageToString(scsMessage));
            }
        }

        public ScsClientMessage CreateMesssage(string operationId, string message)
        {
            var jObject = message.JsonToValue<JObject>();
            var scsHeader = _scsHelper.CreateClientHeader(ConnectorElement.Connection.Local.Node,
                operationId);

            if (operationId == Pd5)
            {
                switch (jObject.Count)
                {
                    case 6:
                        operationId = Pd51;
                        break;
                    case 7:
                        operationId = Pd52;
                        break;
                }
            }

            var scsBodyElement = _scsHelper.GetBodyElement(_scsSection, operationId);


            var scsBody = _scsHelper.CreateBody(scsBodyElement);

            foreach (var scsBodyField in scsBody.ScsBodyFieldList)
            {
                scsBodyField.Value = JSonHelper.GetValue<string>(jObject, scsBodyField.Id);
            }

            var scsMessage = new ScsClientMessage(SocketsHelper.GetUfoId())
            {
                ScsHeader = scsHeader,
                MessageTime = DateTime.Now
            };
            scsMessage.ScsBodyList.Add(scsBody);

            return scsMessage;
        }
    }
}