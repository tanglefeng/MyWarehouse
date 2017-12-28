using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Kengic.Was.Connector.Common;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection.Connectors;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.CrossCutting.Sockets;
using Kengic.Was.CrossCutting.TcpService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kengic.Was.Connector.Scs
{
    public class ScsServer : IConnector
    {
        private const string Hk5 = "5_HK";
        private const string Ak5 = "5_AK";
        private const string Ak = "AK";
        private readonly ScsHelper _scsHelper = new ScsHelper();
        private string _beforeString;
        private ScsSection _scsSection;
        private TcpService _tcpService;
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
            LogRepository.WriteInfomationLog(ConnectorElement.LogName,
                StaticParameterForMessage.InitializeConnectSuccess, Id);
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

                var ipAddress = IPAddress.Parse(ConnectorElement.Connection.Local.Ip);
                var port = ConnectorElement.Connection.Local.Port;
                _tcpService = new TcpService();
                _tcpService.Start(ipAddress, port);
                _tcpService.DataGot += TcpServiceOnDataGot;

                ConnectStatus = true;
                RecSendMsgStatus = true;
                LogRepository.WriteInfomationLog(ConnectorElement.LogName, StaticParameterForMessage.ConnectSuccess,
                    ipAddress + ":" + port);


                return true;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(ConnectorElement.LogName, StaticParameterForMessage.ConnectException,
                    ex.ToString());

                return false;
            }
        }

        public bool DisConnect()
        {
            _tcpService.Stop();
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

        private void TcpServiceOnDataGot(object sender, NetworkDataEventArgs networkDataEventArgs)
        {
            var scsMessageDict = new ConcurrentDictionary<string, ScsClientMessage>();
            var message = Encoding.Default.GetString(networkDataEventArgs.Data);
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            LogRepository.WriteInfomationLog(ConnectorElement.LogName, StaticParameterForMessage.Message,
                message);

            try
            {
                if (!string.IsNullOrEmpty(_beforeString))
                {
                    message = _beforeString + message;
                }
                _scsHelper.GetReceiveMessageForServer(message, _scsSection, ref scsMessageDict, ref _beforeString);
                if ((scsMessageDict == null) || (scsMessageDict.Count <= 0))
                {
                    return;
                }
                foreach (var scsMessage in scsMessageDict.Values)
                {
                    ReceiveDictionary.TryAdd(scsMessage.Id, scsMessage);

                    LogRepository.WriteInfomationLog(ConnectorElement.LogName, StaticParameterForMessage.ReceiveMessage,
                        _scsHelper.ConvertClientMessageToString(scsMessage));

                    if (scsMessage.ScsHeader.OperationId == Hk5)
                    {
                        var timMessageForAk = Get5_AK();
                        SendMessage(timMessageForAk);
                    }
                }
            }
            catch (Exception)
            {
                LogRepository.WriteExceptionLog(ConnectorElement.LogName, StaticParameterForMessage.ErrorMessage,
                    message);
            }
        }

        public bool SendMessage(ScsServerMessage scsMessage)
        {
            try
            {
                scsMessage = _scsHelper.BulidServerMessage(scsMessage);

                var scsMessageBytes = _scsHelper.ConvertServerMessageToBytes(scsMessage);

                _tcpService.Broadcast(scsMessage.Id, scsMessageBytes);

                LogRepository.WriteInfomationLog(ConnectorElement.LogName, StaticParameterForMessage.SendMessage,
                    _scsHelper.ConvertServerMessageToString(scsMessage));
                return true;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(ConnectorElement.LogName, StaticParameterForMessage.SendMessageException,
                    ex.ToString());

                return false;
            }
        }

        public ScsServerMessage Get5_AK()
        {
            var scsHeader = _scsHelper.CreateServerHeader(Ak5);

            var scsBodyElement = _scsHelper.GetBodyElement(_scsSection, scsHeader.OperationId);
            var scsBody = _scsHelper.CreateBody(scsBodyElement);

            scsBody.ScsBodyFieldList[0].Value = Ak;

            var scsServerMessage = new ScsServerMessage(SocketsHelper.GetUfoId())
            {
                ScsHeader = scsHeader,
                MessageTime = DateTime.Now
            };
            scsServerMessage.ScsBodyList.Add(scsBody);

            return scsServerMessage;
        }

        public ScsServerMessage CreateMesssage(string operationId, string message)
        {
            var jObject = message.JsonToValue<JObject>();
            var scsHeader = _scsHelper.CreateServerHeader(operationId);


            var scsBodyElement = _scsHelper.GetBodyElement(_scsSection, scsHeader.OperationId);
            var scsBody = _scsHelper.CreateBody(scsBodyElement);

            foreach (var scsBodyField in scsBody.ScsBodyFieldList)
            {
                scsBodyField.Value = JSonHelper.GetValue<string>(jObject,
                    string.IsNullOrEmpty(scsBodyField.MapName) ? scsBodyField.Id : scsBodyField.MapName);
            }

            var scsMessage = new ScsServerMessage(SocketsHelper.GetUfoId())
            {
                ScsHeader = scsHeader,
                MessageTime = DateTime.Now
            };
            scsMessage.ScsBodyList.Add(scsBody);

            return scsMessage;
        }
    }
}