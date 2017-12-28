using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Kengic.Was.CrossCutting.ConfigurationSection.Connectors;
using Kengic.Was.CrossCutting.Sockets;

namespace Kengic.Was.Connector.Scs
{
    internal class ScsHelper
    {
        private const string Hk5 = "5_HK";
        private const string Pd5 = "5_PD";
        private const string Pd51 = "5_PD1";
        private const string Pd52 = "5_PD2";
        //private const string Ak5 = "5_AK";
        //private const string Ak = "AK";

        internal ScsClientHeader CreateClientHeader(string sourceId)
        {
            var scsHeader = new ScsClientHeader
            {
                StartChar = "((",
                SourceId = sourceId
            };
            return scsHeader;
        }

        internal ScsClientHeader CreateClientHeader(string sourceId, string operationId)
        {
            var scsHeader = CreateClientHeader(sourceId);
            scsHeader.OperationId = operationId;
            scsHeader.Index = SocketsHelper.GetSequenceNo().ToString();
            return scsHeader;
        }

        internal ScsServerHeader CreateServerHeader(string operationId)
        {
            var scsHeader = new ScsServerHeader {OperationId = operationId};
            return scsHeader;
        }

        internal ScsBodyElement GetBodyElement(ScsSection scsBodyConfiguration, string operationId)
        {
            var scsBodyConfigurationsList = scsBodyConfiguration.ScsBodys;

            return
                scsBodyConfigurationsList.Cast<ScsBodyElement>()
                    .FirstOrDefault(timBodyConfigurationElement => timBodyConfigurationElement.Id == operationId);
        }

        internal ScsBody CreateBody(ScsBodyElement timBodyElement)
        {
            var scsBody = new ScsBody {OperationId = timBodyElement.Id, Length = timBodyElement.Length};

            foreach (ScsBodyFieldElement scsBodyField in timBodyElement.ScsBodyFields)
            {
                var scsBodyProproty = new ScsBodyProproty
                {
                    Id = scsBodyField.Id,
                    Type = scsBodyField.Type,
                    SequenceNo = scsBodyField.SequenceNo,
                    MapName = scsBodyField.MapName,
                    Length = scsBodyField.Length
                };
                scsBody.ScsBodyFieldList.Add(scsBodyProproty);
            }

            return scsBody;
        }

        internal ScsClientMessage BulidClientMessage(ScsClientMessage scsMessage)
        {
            var blockLength = 0;
            if ((scsMessage.ScsBodyList != null) && (scsMessage.ScsBodyList.Count > 0))
            {
                foreach (var scsBody in scsMessage.ScsBodyList)
                {
                    foreach (var scsBodyField in scsBody.ScsBodyFieldList)
                    {
                        blockLength = scsBodyField.Length > 0
                            ? blockLength + scsBodyField.Length
                            : blockLength + scsBodyField.Value.Length;
                    }
                }
            }
            scsMessage.ScsHeader.NumOfDataBytes = blockLength.ToString("0000");

            return scsMessage;
        }

        internal ScsServerMessage BulidServerMessage(ScsServerMessage scsMessage)
        {
            var blockLength = 0;
            if ((scsMessage.ScsBodyList != null) && (scsMessage.ScsBodyList.Count > 0))
            {
                foreach (var scsBody in scsMessage.ScsBodyList)
                {
                    foreach (var scsBodyField in scsBody.ScsBodyFieldList)
                    {
                        blockLength = string.IsNullOrEmpty(scsBodyField.Value)
                            ? blockLength + scsBodyField.Length
                            : blockLength + scsBodyField.Value.Length;
                    }
                }
            }
            scsMessage.ScsHeader.NumOfDataBytes = blockLength.ToString("0000");

            return scsMessage;
        }

        internal byte[] ConvertScsMessageToBytes(ScsClientMessage scsMessage)
        {
            var timMessageByte = ConvertClientHeaderToBytes(scsMessage.ScsHeader);

            return scsMessage.ScsBodyList
                .Aggregate(timMessageByte,
                    (current, timBody) => current.Concat(ConvertBodyToBytes(timBody)).ToArray());
        }

        internal byte[] ConvertServerMessageToBytes(ScsServerMessage scsMessage)
        {
            var timMessageByte = ConvertServerHeaderToBytes(scsMessage.ScsHeader);

            return scsMessage.ScsBodyList
                .Aggregate(timMessageByte,
                    (current, timBody) => current.Concat(ConvertBodyToBytes(timBody)).ToArray());
        }

        internal string ConvertClientMessageToString(ScsClientMessage scsMessage)
        {
            var rtnMessage = ConvertClientHeaderToString(scsMessage.ScsHeader);

            return scsMessage.ScsBodyList
                .Aggregate(rtnMessage, (current, timBody) => current + ConvertBodyToString(timBody));
        }

        internal string ConvertClientHeaderToString(ScsClientHeader scsHeader)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(scsHeader.StartChar);
            stringBuilder.Append(scsHeader.SourceId);
            stringBuilder.Append(scsHeader.Index);
            stringBuilder.Append(scsHeader.OperationId);
            stringBuilder.Append(scsHeader.NumOfDataBytes);

            return stringBuilder.ToString();
        }

        internal string ConvertServerMessageToString(ScsServerMessage scsMessage)
        {
            var rtnMessage = ConvertServerHeaderToString(scsMessage.ScsHeader);

            return scsMessage.ScsBodyList
                .Aggregate(rtnMessage, (current, timBody) => current + ConvertBodyToString(timBody));
        }

        internal string ConvertServerHeaderToString(ScsServerHeader scsHeader)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(scsHeader.OperationId);
            stringBuilder.Append(scsHeader.NumOfDataBytes);

            return stringBuilder.ToString();
        }

        internal string ConvertBodyToString(ScsBody scsBody) => scsBody.ScsBodyFieldList.Aggregate(string.Empty,
            (current, timBodyProproty) => current + timBodyProproty.Value);

        internal byte[] ConvertClientHeaderToBytes(ScsClientHeader scsHeader)
        {
            var scsHeaderBytes = SocketsHelper.ConvertStringToBytes(scsHeader.StartChar).ToArray();
            scsHeaderBytes = scsHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(scsHeader.SourceId)).ToArray();
            scsHeaderBytes = scsHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(scsHeader.Index)).ToArray();
            scsHeaderBytes = scsHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(scsHeader.OperationId)).ToArray();
            scsHeaderBytes =
                scsHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(scsHeader.NumOfDataBytes)).ToArray();

            return scsHeaderBytes;
        }

        internal byte[] ConvertServerHeaderToBytes(ScsServerHeader scsHeader)
        {
            var scsHeaderBytes = SocketsHelper.ConvertStringToBytes(scsHeader.OperationId).ToArray();
            scsHeaderBytes =
                scsHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(scsHeader.NumOfDataBytes)).ToArray();

            return scsHeaderBytes;
        }

        internal byte[] ConvertBodyToBytes(ScsBody scsBody)
        {
            byte[] rtnBytes = {};

            //return scsBody.ScsBodyFieldList.Select(scsBodyProproty => scsBodyProproty.Value)
            //    .Aggregate(rtnBytes,
            //        (current, charValue) => current.Concat(SocketsHelper.ConvertStringToBytes(charValue)).ToArray());
            foreach (var scsBodyFileds in scsBody.ScsBodyFieldList)
            {
                if (string.IsNullOrEmpty(scsBodyFileds.Value))
                {
                    if (scsBodyFileds.Length > 0)
                    {
                        rtnBytes = rtnBytes.Concat(SocketsHelper.FillWithSpaceChar(scsBodyFileds.Length)).ToArray();
                    }
                    continue;
                }


                if (scsBodyFileds.Length > 0)
                {
                    if (scsBodyFileds.Length > scsBodyFileds.Value.Length)
                    {
                        rtnBytes =
                            rtnBytes.Concat(
                                SocketsHelper.FillWithSpaceChar(scsBodyFileds.Length - scsBodyFileds.Value.Length))
                                .ToArray();
                    }
                }

                rtnBytes = rtnBytes.Concat(SocketsHelper.ConvertStringToBytes(scsBodyFileds.Value)).ToArray();
            }
            return rtnBytes;
        }

        internal void GetReceiveMessageForServer(string receiveMessage,
            ScsSection scsBodyConfiguration,
            ref ConcurrentDictionary<string, ScsClientMessage> scsMessagesDict, ref string remainString)
        {
            //If the byte of receiving is less than 7,return;
            if (receiveMessage.Length < 7)
            {
                remainString = receiveMessage;
                return;
            }

            if (!receiveMessage.Contains("("))
            {
                remainString = receiveMessage;
                return;
            }
            //Get ((---) message
            receiveMessage = receiveMessage.Substring(receiveMessage.IndexOf('('));
            var headerMessage = receiveMessage.Substring(0, receiveMessage.IndexOf(')'));

            //Get tim header
            var scsHeader = headerMessage.Length == 6
                ? CreateClientHeader("XXXX", Hk5)
                : CreateClientHeaderByMessage(headerMessage);

            var scsClientMessage = new ScsClientMessage(SocketsHelper.GetUfoId())
            {
                ScsHeader = scsHeader,
                MessageTime = DateTime.Now
            };

            //Get the length of the scs body
            var bodyMessageLength = Convert.ToInt32(scsHeader.NumOfDataBytes);
            if ((bodyMessageLength > 0) && (receiveMessage.Length >= 18 + bodyMessageLength))
            {
                var bodyMessage = receiveMessage.Substring(18, bodyMessageLength);

                //Get operationId
                if (scsHeader.OperationId == Pd5)
                {
                    var noPabs = bodyMessage.Substring(10, 2);
                    switch (noPabs)
                    {
                        case "04":
                            scsHeader.OperationId = Pd51;
                            break;
                        case "02":
                            scsHeader.OperationId = Pd5;
                            break;
                        default:
                            scsHeader.OperationId = Pd52;
                            break;
                    }
                }

                var scsBodyElement = GetBodyElement(scsBodyConfiguration, scsHeader.OperationId);

                var scsBody = CreateBody(scsBodyElement, bodyMessage);

                scsClientMessage.ScsBodyList.Add(scsBody);
            }

            scsMessagesDict.TryAdd(scsClientMessage.Id, scsClientMessage);

            if (receiveMessage.Length > 19 + bodyMessageLength)
            {
                var nextMessageLength = receiveMessage.Length - 19 - bodyMessageLength;
                var nextMessage = receiveMessage.Substring(19 + bodyMessageLength, nextMessageLength);

                GetReceiveMessageForServer(nextMessage,
                    scsBodyConfiguration,
                    ref scsMessagesDict, ref remainString);
            }
        }

        internal void GetReceiveMessageForClient(string receiveMessage,
            ScsSection scsBodyConfiguration,
            ref ConcurrentDictionary<string, ScsServerMessage> scsMessagesDict)
        {
            //If the byte of receiving is less than 52,return;
            if (receiveMessage.Length < 52)
            {
                return;
            }

            //Get tim header
            var scsHeader = new ScsServerHeader
            {
                OperationId = receiveMessage.Substring(0, 4),
                NumOfDataBytes = receiveMessage.Substring(4, 4)
            };

            var scsServerMessage = new ScsServerMessage(SocketsHelper.GetUfoId())
            {
                ScsHeader = scsHeader,
                MessageTime = DateTime.Now
            };

            //Get the length of the scs body
            var bodyMessageLength = Convert.ToInt32(scsHeader.NumOfDataBytes);
            if ((bodyMessageLength > 0) && (receiveMessage.Length >= bodyMessageLength + 8))
            {
                var bodyMessage = receiveMessage.Substring(8, bodyMessageLength);

                //Get operationId
                var operationId = scsHeader.OperationId;

                var scsBodyElement = GetBodyElement(scsBodyConfiguration, operationId);

                var scsBody = CreateBody(scsBodyElement, bodyMessage);

                scsServerMessage.ScsBodyList.Add(scsBody);
            }

            scsMessagesDict.TryAdd(scsServerMessage.Id, scsServerMessage);

            if (receiveMessage.Length > 9 + bodyMessageLength)
            {
                var nextMessageLength = receiveMessage.Length - 8 - bodyMessageLength;
                var nextMessage = receiveMessage.Substring(8 + bodyMessageLength, nextMessageLength);

                GetReceiveMessageForClient(nextMessage,
                    scsBodyConfiguration,
                    ref scsMessagesDict);
            }
        }

        internal ScsBody CreateBody(ScsBodyElement scsBodyElement, string bodyMessageString)
        {
            var scsBody = new ScsBody {OperationId = scsBodyElement.Id, Length = scsBodyElement.Length};

            foreach (ScsBodyFieldElement scsBodyField in scsBodyElement.ScsBodyFields)
            {
                var fieldValue = scsBodyField.Length == 0
                    ? bodyMessageString.Substring(scsBodyField.StartAddress,
                        bodyMessageString.Length - scsBodyField.StartAddress)
                    : bodyMessageString.Substring(scsBodyField.StartAddress,
                        scsBodyField.Length);

                var scsBodyProproty = new ScsBodyProproty
                {
                    Id = scsBodyField.Id,
                    Type = scsBodyField.Type,
                    Value = fieldValue,
                    SequenceNo = scsBodyField.SequenceNo
                };
                scsBody.ScsBodyFieldList.Add(scsBodyProproty);
            }

            return scsBody;
        }

        internal ScsClientHeader CreateClientHeaderByMessage(string scsHeaderString)
        {
            var scsHeader = new ScsClientHeader
            {
                StartChar = scsHeaderString.Substring(0, 2),
                SourceId = scsHeaderString.Substring(2, 4),
                Index = scsHeaderString.Substring(6, 4),
                OperationId = scsHeaderString.Substring(10, 4),
                NumOfDataBytes = scsHeaderString.Substring(14, 4)
            };

            return scsHeader;
        }

        internal ScsServerHeader CreateServerHeaderByMessage(string scsHeaderString)
        {
            var scsHeader = new ScsServerHeader
            {
                OperationId = scsHeaderString.Substring(0, 4),
                NumOfDataBytes = scsHeaderString.Substring(4, 4)
            };

            return scsHeader;
        }
    }
}