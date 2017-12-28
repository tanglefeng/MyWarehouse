using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.ConfigurationSection.Connectors;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.CrossCutting.Sockets;
using Newtonsoft.Json.Linq;

namespace Kengic.Was.Connector.Tim
{
    public class TimHelper
    {
        public const string Ak = "AK";
        public const string Ay = "AY";
        public const string Ti = "TI";
        public const string TypeChar = "CHAR";
        public const string TypeDatetime = "DATETIME";
        public const string TypeUint16 = "UINT16";

        public byte[] ConvertMessageToBytes(TimMessage timMessage)
        {
            var timMessageByte = ConvertHeaderToBytes(timMessage.TimHeader);

            return timMessage.TimBodyList
                .Aggregate(timMessageByte,
                    (current, timBody) => current.Concat(ConvertBodyToBytes(timBody)).ToArray());
        }

        public byte[] ConvertHeaderToBytes(TimHeader timHeader)
        {
            var timHeaderBytes = SocketsHelper.ConvertStringToBytes(timHeader.Version).ToArray();
            timHeaderBytes = timHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(timHeader.Protocol)).ToArray();
            timHeaderBytes =
                timHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(timHeader.DatagramCounter)).ToArray();
            timHeaderBytes = timHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(timHeader.ReturnValue)).ToArray();
            timHeaderBytes =
                timHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(timHeader.DatagramLength)).ToArray();
            timHeaderBytes = timHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(timHeader.SourceNode)).ToArray();
            timHeaderBytes =
                timHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(timHeader.DestinationNode)).ToArray();
            timHeaderBytes =
                timHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(timHeader.SequenceNumber)).ToArray();
            timHeaderBytes = timHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(timHeader.FlowControl)).ToArray();
            timHeaderBytes =
                timHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(timHeader.SourceService)).ToArray();
            timHeaderBytes =
                timHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(timHeader.DestinationService)).ToArray();
            timHeaderBytes = timHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(timHeader.OperationId)).ToArray();
            timHeaderBytes = timHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(timHeader.BlockCount)).ToArray();
            timHeaderBytes = timHeaderBytes.Concat(SocketsHelper.ConvertStringToBytes(timHeader.BlockLength)).ToArray();

            return timHeaderBytes;
        }

        public byte[] ConvertBodyToBytes(TimBody timBody)
        {
            byte[] rtnBytes = {};
            foreach (var timBodyProproty in timBody.TimBodyFieldList)
            {
                switch (timBodyProproty.Type.ToUpper())
                {
                    case TypeUint16:
                        var uint16Value = Convert.ToUInt16(timBodyProproty.Value);
                        var tmepBytes = SocketsHelper.ConvertUint16ToBytes(uint16Value);
                        rtnBytes = rtnBytes.Concat(SocketsHelper.GetBytesForChangeHighLowBytes(tmepBytes)).ToArray();
                        break;

                    case TypeChar:
                        var charValue = timBodyProproty.Value;
                        if (charValue.Length < timBodyProproty.Length)
                        {
                            charValue = SocketsHelper.FillWithEmptyChar(charValue,
                                timBodyProproty.Length - charValue.Length);
                        }

                        rtnBytes = rtnBytes.Concat(SocketsHelper.ConvertStringToBytes(charValue)).ToArray();
                        break;

                    case TypeDatetime:
                        var dateTime = DateTime.ParseExact(timBodyProproty.Value, SocketsHelper.Windowffffff, null);
                        rtnBytes = rtnBytes.Concat(ConvertDateTimeToBytes(dateTime)).ToArray();
                        break;
                }
            }

            return rtnBytes;
        }

        public string ConvertMessageToString(TimMessage timMessage)
        {
            if (timMessage != null)
            {
                var rtnMessage = ConvertHeaderToString(timMessage.TimHeader);

                return timMessage.TimBodyList
                    .Aggregate(rtnMessage, (current, timBody) => current + ConvertBodyToString(timBody));
            }

            return "";
        }

        public string ConvertHeaderToString(TimHeader timHeader)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(timHeader.Version);
            stringBuilder.Append(timHeader.Protocol);
            stringBuilder.Append(timHeader.DatagramCounter);
            stringBuilder.Append(timHeader.ReturnValue);
            stringBuilder.Append(timHeader.DatagramLength);
            stringBuilder.Append(timHeader.SourceNode);
            stringBuilder.Append(timHeader.DestinationNode);
            stringBuilder.Append(timHeader.SequenceNumber);
            stringBuilder.Append(timHeader.FlowControl);
            stringBuilder.Append(timHeader.SourceService);
            stringBuilder.Append(timHeader.DestinationService);
            stringBuilder.Append(timHeader.OperationId);
            stringBuilder.Append(timHeader.BlockCount);
            stringBuilder.Append(timHeader.BlockLength);

            return stringBuilder.ToString();
        }

        /// <summary>
        ///     <see cref="Convert" /> body to string If the value of body is
        ///     <see langword="int" /> or datetime,formate it by []
        /// </summary>
        /// <param name="timBody"></param>
        /// <returns>
        /// </returns>
        public string ConvertBodyToString(TimBody timBody)
        {
            var timBodyStr = "\r\n";
            foreach (var timBodyProproty in timBody.TimBodyFieldList)
            {
                switch (timBodyProproty.Type.ToUpper())
                {
                    case TypeUint16:
                    case TypeDatetime:
                        timBodyStr = timBodyStr + "[" + timBodyProproty.Value + "]";
                        break;

                    default:
                        timBodyStr = timBodyStr + timBodyProproty.Value;
                        break;
                }
            }

            return timBodyStr;
        }

        public int GetMessageLength(TimMessage timMessage) => timMessage.TimBodyList
            .Aggregate(64, (current, timBody) => current + GetBodyLength(timBody));

        public int GetBodyLength(TimBody timBody) => timBody.Length;

        public TimBodyElement GetBodyElement(TimSection timBodyConfiguration, string operationId)
        {
            var timBodyConfigurationsList = timBodyConfiguration.TimBodys;

            return
                timBodyConfigurationsList.Cast<TimBodyElement>()
                    .FirstOrDefault(timBodyConfigurationElement => timBodyConfigurationElement.Id == operationId);
        }

        public void GetReceiveMessage(byte[] receiveBytes,
            TimSection timBodyConfiguration,
            ref ConcurrentDictionary<string, TimMessage> timMessagesDict, ref byte[] remainBytes, string logName)
        {
            //If the byte of receiving is less than 64,return;
            if (receiveBytes.Length < 64)
            {
                remainBytes = new byte[receiveBytes.Length];
                Buffer.BlockCopy(receiveBytes, 0, remainBytes, 0, receiveBytes.Length);
                return;
            }

            //Get tim header
            var timHeaderBytes = new byte[64];
            Buffer.BlockCopy(receiveBytes, 0, timHeaderBytes, 0, 64);
            var strTimHeader = Encoding.Default.GetString(timHeaderBytes);


            if (!strTimHeader.Contains(Ti))
            {
                //if the tim header don't include 'TI',dispose the message(64)
                if (strTimHeader.Contains(" "))
                {
                    strTimHeader = strTimHeader.Replace(" ", "*");
                }
                if (strTimHeader.Contains("\0"))
                {
                    strTimHeader = strTimHeader.Replace("\0", "&");
                }
                LogRepository.WriteErrorLog(logName, StaticParameterForMessage.ErrorMessage, strTimHeader);

                //renew to cutting the byte after 64 
                var nextBytesLength = receiveBytes.Length - 64;
                var nextBytes = new byte[nextBytesLength];
                Buffer.BlockCopy(receiveBytes, 64, nextBytes, 0, nextBytesLength);
                GetReceiveMessage(nextBytes, timBodyConfiguration, ref timMessagesDict, ref remainBytes, logName);
                return;
            }

            var tiCharPosition = strTimHeader.IndexOf('T');
            if (tiCharPosition > 1)
            {
                //renew to cutting the 64 byte after "T" char 
                LogRepository.WriteErrorLog(logName, StaticParameterForMessage.ErrorMessage,
                    strTimHeader.Substring(0, tiCharPosition));
                Buffer.BlockCopy(receiveBytes, tiCharPosition, timHeaderBytes, 0, 64);
            }

            //Convert to tim header
            var timHeader = CreateHeader(timHeaderBytes);

            var timMessage = new TimMessage(SocketsHelper.GetUfoId())
            {
                TimHeader = timHeader,
                MessageTime = DateTime.Now
            };


            //Get the length of the timbody by block count and block length
            var blockCount = Convert.ToInt32(timHeader.BlockCount);
            var timBodyLength = Convert.ToInt32(timHeader.BlockLength);

            var timBodyBytes = new byte[timBodyLength];
            Buffer.BlockCopy(receiveBytes, 64 + tiCharPosition, timBodyBytes, 0, timBodyLength);

            //Get operationId
            var operationId = timHeader.OperationId;

            var timBodyElement = GetBodyElement(timBodyConfiguration, operationId);
            if (timBodyElement == null)
            {
                LogRepository.WriteErrorLog(logName, StaticParameterForMessage.NoOperationId,
                    operationId);
            }
            else
            {
                if ((blockCount > 0) && (timBodyLength > 0))
                {
                    for (var i = 0; i < blockCount; i++)
                    {
                        var theTimbodysBytes = new byte[timBodyElement.Length];
                        Buffer.BlockCopy(timBodyBytes, i*timBodyElement.Length, theTimbodysBytes, 0,
                            timBodyElement.Length);
                        var timBody = CreateBody(timBodyElement, theTimbodysBytes);
                        timMessage.TimBodyList.Add(timBody);
                    }
                }
            }


            timMessagesDict.TryAdd(timMessage.TimHeader.SequenceNumber, timMessage);

            if (receiveBytes.Length > 64 + timBodyLength + tiCharPosition)
            {
                var nextBytesLength = receiveBytes.Length - 64 - timBodyLength - tiCharPosition;
                var nextBytes = new byte[nextBytesLength];
                Buffer.BlockCopy(receiveBytes, 64 + timBodyLength + tiCharPosition, nextBytes, 0, nextBytesLength);
                GetReceiveMessage(nextBytes,
                    timBodyConfiguration,
                    ref timMessagesDict, ref remainBytes, logName);
            }
        }

        public TimMessage CreateAkMessage(TimHeader timHeaderForAk, TimHeader timHeader)
        {
            var timMessage = new TimMessage(SocketsHelper.GetUfoId()) {MessageTime = DateTime.Now};

            timHeaderForAk.SequenceNumber = timHeader.SequenceNumber;
            //timHeaderForAk.ReturnValue = timHeader.ReturnValue;
            timHeaderForAk.FlowControl = Ak;
            timHeaderForAk.OperationId = timHeader.OperationId;
            timMessage.TimHeader = timHeaderForAk;

            return timMessage;
        }

        public TimHeader CreateHeader(string version, string protocol, string sourceNode, string sourceService,
            string destinationNode, string destinationService)
        {
            var timHeader = new TimHeader
            {
                Version = version,
                Protocol = protocol,
                DatagramCounter = "0000",
                ReturnValue = "00",
                SourceNode = sourceNode,
                DestinationNode = destinationNode,
                FlowControl = Ay,
                SourceService = sourceService,
                DestinationService = destinationService
            };
            return timHeader;
        }

        public TimHeader CreateHeader(ConnectionElement connectConfiguration)
            => CreateHeader(connectConfiguration.Version,
                connectConfiguration.Protocol,
                connectConfiguration.Local.Node,
                connectConfiguration.Local.Name,
                connectConfiguration.Remote.Node,
                connectConfiguration.Remote.Name);

        public TimHeader CreateHeader(byte[] bytes)
        {
            var timHeader = new TimHeader
            {
                Version = SocketsHelper.GetStringInBytes(bytes, 0, 2),
                Protocol = SocketsHelper.GetStringInBytes(bytes, 2, 2),
                DatagramCounter = SocketsHelper.GetStringInBytes(bytes, 4, 4),
                ReturnValue = SocketsHelper.GetStringInBytes(bytes, 8, 2),
                DatagramLength = SocketsHelper.GetStringInBytes(bytes, 10, 6),
                SourceNode = SocketsHelper.GetStringInBytes(bytes, 16, 6),
                DestinationNode = SocketsHelper.GetStringInBytes(bytes, 22, 6),
                SequenceNumber = SocketsHelper.GetStringInBytes(bytes, 28, 4),
                FlowControl = SocketsHelper.GetStringInBytes(bytes, 32, 2),
                SourceService = SocketsHelper.GetStringInBytes(bytes, 34, 6),
                DestinationService = SocketsHelper.GetStringInBytes(bytes, 40, 6),
                OperationId = SocketsHelper.GetStringInBytes(bytes, 46, 6),
                BlockCount = SocketsHelper.GetStringInBytes(bytes, 52, 6),
                BlockLength = SocketsHelper.GetStringInBytes(bytes, 58, 6)
            };

            return timHeader;
        }

        /// <summary>
        ///     Create a tim body by configuration fill with by receiving
        ///     <paramref name="bytes" />
        /// </summary>
        /// <param name="timBodyElement"></param>
        /// <param name="bytes"></param>
        /// <returns>
        /// </returns>
        public TimBody CreateBody(TimBodyElement timBodyElement, byte[] bytes)
        {
            var timBody = new TimBody {OperationId = timBodyElement.Id, Length = timBodyElement.Length};

            foreach (TimBodyFieldElement timBodyField in timBodyElement.TimBodyFields)
            {
                var fieldValue = string.Empty;
                switch (timBodyField.Type.ToUpper())
                {
                    case TypeChar:
                        fieldValue = SocketsHelper.GetStringInBytes(bytes, timBodyField.StartAddress,
                            timBodyField.Length);
                        break;
                    case TypeUint16:
                        fieldValue =
                            SocketsHelper.GetUnit16InBytes(bytes, timBodyField.StartAddress, timBodyField.Length)
                                .ToString();
                        break;

                    case TypeDatetime:
                        var dateTimeBytes = new byte[timBodyField.Length];
                        Buffer.BlockCopy(bytes, timBodyField.StartAddress, dateTimeBytes, 0, timBodyField.Length);
                        fieldValue = ConvertBytesToDateTime(dateTimeBytes).ToString(SocketsHelper.Windowffffff);
                        break;
                }

                var timBodyProproty = new TimBodyProproty
                {
                    Id = timBodyField.Id,
                    Type = timBodyField.Type,
                    Value = fieldValue,
                    SequenceNo = timBodyField.SequenceNo
                };
                timBody.TimBodyFieldList.Add(timBodyProproty);
            }

            return timBody;
        }

        /// <summary>
        ///     Create a tim body by configuration
        /// </summary>
        /// <param name="timBodyElement"></param>
        /// <returns>
        /// </returns>
        public TimBody CreateBody(TimBodyElement timBodyElement)
        {
            var timBody = new TimBody {OperationId = timBodyElement.Id, Length = timBodyElement.Length};

            foreach (var timBodyProproty in from TimBodyFieldElement timBodyField in timBodyElement.TimBodyFields
                select new TimBodyProproty
                {
                    Id = timBodyField.Id,
                    Type = timBodyField.Type,
                    SequenceNo = timBodyField.SequenceNo,
                    MapName = timBodyField.MapName,
                    Length = timBodyField.Length
                })
            {
                timBody.TimBodyFieldList.Add(timBodyProproty);
            }

            return timBody;
        }

        /// <summary>
        ///     <see cref="Convert" /> date time to bytes
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>
        /// </returns>
        public byte[] ConvertDateTimeToBytes(DateTime dateTime)
        {
            var timeBytes = new byte[8];
            var year = dateTime.Year - 2000;
            timeBytes[0] = Convert.ToByte(year);

            var month = dateTime.Month;
            timeBytes[1] = Convert.ToByte(month);

            var day = dateTime.Day;
            timeBytes[2] = Convert.ToByte(day);

            var hour = dateTime.Hour;
            timeBytes[3] = Convert.ToByte(hour);

            var minute = dateTime.Minute;
            timeBytes[4] = Convert.ToByte(minute);

            var second = dateTime.Second;
            timeBytes[5] = Convert.ToByte(second);

            var millisecond = dateTime.Millisecond;
            timeBytes[6] = Convert.ToByte(Math.Floor((double) millisecond/10));

            var msSignificant = dateTime.Millisecond%10;
            timeBytes[7] = Convert.ToByte(msSignificant + 16 + Convert.ToInt16(dateTime.DayOfWeek));

            return timeBytes;
        }

        /// <summary>
        ///     <see cref="Convert" /> bytes to date time
        /// </summary>
        /// <param name="dateTimeBytes"></param>
        /// <returns>
        /// </returns>
        public DateTime ConvertBytesToDateTime(byte[] dateTimeBytes)
        {
            var year = Convert.ToInt32(dateTimeBytes[0]) + 2000;
            var month = Convert.ToInt32(dateTimeBytes[1]);
            var day = Convert.ToInt32(dateTimeBytes[2]);
            var hour = Convert.ToInt32(dateTimeBytes[3]);
            var minute = Convert.ToInt32(dateTimeBytes[4]);
            var second = Convert.ToInt32(dateTimeBytes[5]);
            var millisecond = Convert.ToInt32(dateTimeBytes[6])*10;

            var dateTime = new DateTime(year, month, day, hour, minute, second, millisecond);
            var msSignificant = Convert.ToInt32(dateTimeBytes[7]) - (int) dateTime.DayOfWeek - 16;
            dateTime = dateTime.AddMilliseconds(msSignificant);
            return dateTime;
        }

        /// <summary>
        ///     Set tim message block count,block length and the length of datagram
        /// </summary>
        /// <param name="timMessage"></param>
        /// <returns>
        /// </returns>
        public TimMessage BulidMessage(TimMessage timMessage)
        {
            timMessage.TimHeader.BlockCount = timMessage.TimBodyList.Count.ToString("000000");

            var blockLength = 0;
            if ((timMessage.TimBodyList != null) && (timMessage.TimBodyList.Count > 0))
            {
                blockLength = timMessage.TimBodyList[0].Length;
            }
            timMessage.TimHeader.BlockLength = blockLength.ToString("000000");

            timMessage.TimHeader.DatagramLength =
                GetMessageLength(timMessage).ToString("000000");

            return timMessage;
        }

        /// <summary>
        ///     Create tim <paramref name="message" /> from the configuration Fill
        ///     with tim <paramref name="message" /> by receiving
        ///     <paramref name="message" />
        /// </summary>
        /// <param name="operationId"></param>
        /// <param name="message"></param>
        /// <param name="connectConfiguration"></param>
        /// <param name="timBodyConfiguration"></param>
        /// <returns>
        /// </returns>
        public TimMessage CreateMesssage(string operationId, string message,
            ConnectionElement connectConfiguration,
            TimSection timBodyConfiguration)
        {
            var jObject = message.JsonToValue<JObject>();
            var timHeader = CreateHeader(connectConfiguration);
            timHeader.OperationId = operationId;

            var timBodyElement = GetBodyElement(timBodyConfiguration, timHeader.OperationId);
            var timBody = CreateBody(timBodyElement);

            foreach (var timBodyField in timBody.TimBodyFieldList)
            {
                timBodyField.Value = JSonHelper.GetValue<string>(jObject,
                    string.IsNullOrEmpty(timBodyField.MapName) ? timBodyField.Id : timBodyField.MapName);
            }

            var timMessage = new TimMessage(SocketsHelper.GetUfoId())
            {
                TimHeader = timHeader,
                MessageTime = DateTime.Now
            };
            timMessage.TimBodyList.Add(timBody);

            return timMessage;
        }
    }
}