using System;
using System.Linq;
using System.Text;

namespace Kengic.Was.CrossCutting.Sockets
{
    public class SocketsHelper
    {
        private static int _squenceNo;
        public static string Windowffffff = "yyyy-MM-dd HH:mm:ss:fff";

        public static string GetUfoId()
        {
            var sequenceNo = GetSequenceNo();
            return DateTime.Now.ToString("yyyyMMddHHmmssffffff") + sequenceNo;
        }

        public static int GetSequenceNo()
        {
            if (_squenceNo >= 99999)
            {
                _squenceNo = 1;
            }
            else
            {
                _squenceNo = _squenceNo + 1;
            }

            return _squenceNo;
        }

        public static byte[] ConvertUint16ToBytes(ushort value) => BitConverter.GetBytes(value).Take(2).ToArray();
        public static byte[] ConvertStringToBytes(string value) => Encoding.UTF8.GetBytes(value);

        public static string GetStringInBytes(byte[] bytes, int offset, int length)
        {
            var valueBytes = new byte[length];
            Buffer.BlockCopy(bytes, offset, valueBytes, 0, length);
            return Encoding.Default.GetString(valueBytes);
        }

        public static ushort GetUnit16InBytes(byte[] bytes, int offset, int length)
        {
            var valueBytes = new byte[length];
            Buffer.BlockCopy(bytes, offset, valueBytes, 0, length);

            var highByte = valueBytes[1];
            var lowByte = valueBytes[0];

            var newBytes = new[] {highByte, lowByte};

            return BitConverter.ToUInt16(newBytes, 0);
        }

        public static byte[] GetBytesForChangeHighLowBytes(byte[] bytes)
        {
            var highByte = bytes[1];
            var lowByte = bytes[0];

            var newBytes = new[] {highByte, lowByte};

            return newBytes;
        }

        public static string FillWithEmptyChar(string value, int emptyCharNum)
        {
            for (var i = 0; i < emptyCharNum; i++)
            {
                value = value + " ";
            }

            return value;
        }

        public static byte[] FillWithSpaceChar(int fillLength)
        {
            if (fillLength == 0)
            {
                return null;
            }

            var destFillBytes = new byte[fillLength];
            for (var i = 0; i < fillLength; i++)
            {
                destFillBytes[i] = Convert.ToByte(' ');
            }
            return destFillBytes;
        }
    }
}