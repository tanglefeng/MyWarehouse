using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Kengic.Was.Connector.Common;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection.Connectors;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.CrossCutting.Prodave;

namespace Kengic.Was.Connector.Prodave
{
    public class ProdaveClient : IConnector
    {
        private const string AccessPoint = "S7ONLINE"; // Default access point——S7ONLINE        
        private ushort _connectionNumber;
        private string _logName;
        private OpcSection _opcSection;
        public string Id { get; set; }
        public ConcurrentDictionary<string, object> ReceiveDictionary { get; set; }
        public bool RecSendMsgStatus { get; set; }
        public ConnectorElement ConnectorElement { get; set; }
        public bool ConnectStatus { get; set; }
        public bool AlarmActiveStatus { get; set; }
        public bool InitializeStatus { get; set; }

        public bool Initialize()
        {
            _logName = ConnectorElement.LogName;
            _connectionNumber = Convert.ToUInt16(ConnectorElement.Connection.Remote.Node);
            _opcSection = ConfigurationOperation<OpcSection>.GetCustomSection(ConnectorElement.Connection.FilePath,
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

                Prodave6.ConnectionProperty connectionProperty; // Connection table
                var connectionPropertyLength = Marshal.SizeOf(typeof (Prodave6.ConnectionProperty));
                // Length of the connection table
                connectionProperty.Address = ProdaveHelper.ConvertIpToAddress(ConnectorElement.Connection.Remote.Ip);
                connectionProperty.AddressType = 2; // Type of address: MPI/PB (1), IP (2), MAC (3)
                connectionProperty.SlotNum = 2; // 插槽号
                connectionProperty.RackNum = 0; // 机架号  
                var result = Prodave6.LoadConnection_ex6(_connectionNumber, AccessPoint, connectionPropertyLength,
                    ref connectionProperty);

                if ((result != 0) && (result != 39)) // 39已经初始化
                {
                    return false;
                }

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
            var result = Prodave6.UnloadConnection_ex6(_connectionNumber);

            if (result != 0)
            {
                LogRepository.WriteExceptionLog(_logName, StaticParameterForMessage.ConnectException,
                    ProdaveHelper.GetErrorCode(result));
            }
            ConnectStatus = false;
            InitializeStatus = false;
            return true;
        }

        public bool SendMessage(List<string> messageList) => false;

        /// <summary>
        ///     PLC数据读取方法-最长200个字节
        /// </summary>
        /// <param name="datablock">块号</param>
        /// <param name="startAddress">起始字</param>
        /// <param name="dataLength">长度,存放读取的数据</param>
        /// <param name="buffer"></param>
        /// <returns>
        ///     读取成功返回true,读取失败返回false
        /// </returns>
        public bool ReadByte(ushort datablock, int startAddress, int dataLength, out byte[] buffer)
        {
            buffer = new byte[dataLength];

            if (!ConnectStatus)
            {
                return false;
            }

            try
            {
                Prodave6.SetActiveConnection_ex6(_connectionNumber);
                var readbuffer = new byte[dataLength];
                var pAmount = Convert.ToUInt32(dataLength);
                var pDataLen = Convert.ToUInt32(dataLength);

                var iResult = Prodave6.field_read_ex6(Prodave6.FieldType.D, datablock, (ushort) startAddress, pAmount,
                    1024,
                    readbuffer, ref pDataLen);

                if (iResult != 0)
                {
                    return false;
                }

                for (var i = 0; i < dataLength; i++)
                {
                    buffer[i] = readbuffer[i];
                }
                return true;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(_logName, StaticParameterForMessage.ReceiveMessageException,
                    ex.ToString());
                return false;
            }
        }

        public bool ReadByteForMulti(ushort datablock, int startAddress, int dataLength, out byte[] buffer)
        {
            buffer = new byte[dataLength];

            for (var i = 0; i < dataLength; i++)
            {
                buffer[i] = 0;
            }
            const int maxOneLength = 200; //单次允许读取的最大长度,限制为200个字节
            var count = dataLength/maxOneLength; //要读取的次数
            var mod = dataLength%maxOneLength; //剩余的长度

            byte[] readbuffer;
            for (var i = 0; i < count; i++)
            {
                if (!ReadByte(datablock, startAddress + i*maxOneLength, maxOneLength, out readbuffer))
                {
                    return false;
                }

                for (var k = i*maxOneLength; k < (i + 1)*maxOneLength; k++)
                {
                    buffer[k] = readbuffer[k - i*maxOneLength];
                }
            }

            if (mod > 0)
            {
                if (!ReadByte(datablock, startAddress + count*maxOneLength, mod, out readbuffer))
                {
                    return false;
                }
                for (var k = count*maxOneLength; k < count*maxOneLength + mod; k++)
                {
                    buffer[k] = readbuffer[k - count*maxOneLength];
                }
            }
            return true;
        }

        public ushort ReadInt16(ushort datablock, int startAddress)
        {
            byte[] readBuffer;

            if (!ReadByte(datablock, startAddress, 2, out readBuffer))
            {
                return 0;
            }
            var value = Prodave6.bytes_2_word(readBuffer[1], readBuffer[0]);
            return (ushort) (((value >> 8) & 0xFF) | ((value << 8) & 0xFF00));
        }

        public List<int> ReadInt16ByBatch(ushort datablock, int startAddress, int number)
        {
            var rtnList = new List<int>();
            byte[] readBuffer;

            if (!ReadByte(datablock, startAddress, 2*number, out readBuffer))
            {
                return null;
            }
            for (var i = 0; i < number; i++)
            {
                var value = Prodave6.bytes_2_word(readBuffer[i*2 + 1], readBuffer[i*2]);
                rtnList.Add(((value >> 8) & 0xFF) + ((value << 8) & 0xFF00));
            }

            return rtnList;
        }

        public string Readstring(ushort datablock, int startAddress, int length)
        {
            byte[] readBuffer;

            return ReadByte(datablock, startAddress, length, out readBuffer) ? Encoding.UTF8.GetString(readBuffer) : "";
        }

        /// <summary>
        ///     单次写入最多200个字节至PLC
        /// </summary>
        /// <param name="datablock">要写入的块号</param>
        /// <param name="startAddress">要写入的起始字</param>
        /// <param name="buffer">要写入的数据</param>
        /// <returns>
        ///     写入成功返回true,失败返回false
        /// </returns>
        public bool WriteByte(ushort datablock, int startAddress, byte[] buffer)
        {
            try
            {
                var pAmount = Convert.ToUInt32(buffer.Length);
                var pDataLen = Convert.ToUInt32(buffer.Length);

                var iResult = Prodave6.field_write_ex6(Prodave6.FieldType.D, datablock, (ushort) startAddress, pAmount,
                    pDataLen, buffer);
                if (iResult == 0)
                {
                    return true;
                }
                LogRepository.WriteErrorLog(_logName, StaticParameterForMessage.SendMessageFailure,
                    ProdaveHelper.GetErrorCode(iResult));
                return false;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(_logName, StaticParameterForMessage.SendMessageException, ex.ToString());
                return false;
            }
        }

        public bool WriteInt16(ushort datablock, int startAddress, ushort value)
        {
            var buffer = Prodave6.word_2_bytes(value);

            var writeBuffer = new byte[2];
            writeBuffer[0] = buffer[1];
            writeBuffer[1] = buffer[0];

            return WriteByte(datablock, startAddress, writeBuffer);
        }

        public bool WriteString(ushort datablock, int startAddress, string value)
        {
            var writeBuffer = Encoding.ASCII.GetBytes(value);
            return WriteByte(datablock, startAddress, writeBuffer);
        }

        /// <summary>
        ///     PLC数据写入方法
        /// </summary>
        /// <param name="datablock">要写入的块号</param>
        /// <param name="startAddress">起始字</param>
        /// <param name="buffer">要写入PLC的数据</param>
        /// <returns>
        ///     写入成功返回true,失败返回false
        /// </returns>
        public bool WriteByteForMulti(ushort datablock, int startAddress, byte[] buffer)
        {
            var dataLength = buffer.Length;
            const int maxOneLength = 200; //单次允许读取的最大长度,限制为200个字节
            var count = dataLength/maxOneLength; //要读取的次数
            var mod = dataLength%maxOneLength; //剩余的长度

            for (var i = 0; i < count; i++)
            {
                var writeBuffer = new byte[maxOneLength];

                for (var k = i*maxOneLength; k < (i + 1)*maxOneLength; k++)
                {
                    writeBuffer[k - i*maxOneLength] = buffer[k];
                }
                if (!WriteByte(datablock, startAddress + i*maxOneLength, writeBuffer))
                {
                    return false;
                }
            }
            if (mod > 0)
            {
                var writeBuffer = new byte[mod];

                for (var k = count*maxOneLength; k < count*maxOneLength + mod; k++)
                {
                    writeBuffer[k - count*maxOneLength] = buffer[k];
                }
                if (!WriteByte(datablock, startAddress + count*maxOneLength, writeBuffer))
                {
                    return false;
                }
            }

            return true;
        }

        public string ReadGroup(string connectionName, string group)
        {
            var rtnValue = "";
            foreach (OpcGroupElement groupElement  in _opcSection.OpcGroups)
            {
                if ((groupElement.Id == @group) && (groupElement.ConnectionName == connectionName))
                {
                    var startAddress = Convert.ToUInt16(groupElement.StartAddress) +
                                       Convert.ToUInt16(groupElement.OpcItems[0].OppositeAddress);
                    switch (groupElement.OpcItems[0].DataType.ToUpper())
                    {
                        case "INT":
                            rtnValue = ReadInt16(Convert.ToUInt16(groupElement.StorageDb), startAddress).ToString();
                            break;
                        case "STRING":
                            var length = Convert.ToUInt16(groupElement.OpcItems[0].DataLength);
                            rtnValue = Readstring(Convert.ToUInt16(groupElement.StorageDb), startAddress, length);
                            break;
                    }

                    return rtnValue;
                }
            }

            return rtnValue;
        }

        public bool WriteGroup(string connectionName, string group, string value)
        {
            var rtnValue = false;
            foreach (OpcGroupElement groupElement in _opcSection.OpcGroups)
            {
                if ((groupElement.Id == @group) && (groupElement.ConnectionName == connectionName))
                {
                    var startAddress = Convert.ToUInt16(groupElement.StartAddress) +
                                       Convert.ToUInt16(groupElement.OpcItems[0].OppositeAddress);
                    switch (groupElement.OpcItems[0].DataType.ToUpper())
                    {
                        case "INT":
                            rtnValue = WriteInt16(Convert.ToUInt16(groupElement.StorageDb), startAddress,
                                Convert.ToUInt16(value));
                            break;
                        case "STRING":
                            rtnValue = WriteString(Convert.ToUInt16(groupElement.StorageDb), startAddress, value);
                            break;
                    }

                    return rtnValue;
                }
            }

            return false;
        }
    }
}