using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace Kengic.Was.Connector.Jd
{
    public class MessageHelper
    {
        internal void GetReceiveMessageForClient(byte[] infoBytes,
            ref ConcurrentDictionary<string, string> messagesDict, ref byte[] remainBytes)
        {
            const byte startByte = 2;
            const byte endByte = 3;       

            var startIndex = 0;
            var endIndex = 0;

            while (true)
            {
                if (!infoBytes.Contains(startByte) || !infoBytes.Contains(endByte))
                {
                    remainBytes = new byte[infoBytes.Length];
                    Buffer.BlockCopy(infoBytes, 0, remainBytes, 0, infoBytes.Length);
                    return;
                }

                for (var i = 0; i < infoBytes.Length; i++)
                {
                    if (infoBytes[i] == startByte)
                    {
                        startIndex = i;
                    }

                    if (infoBytes[i] != endByte) continue;

                    endIndex = i;
                    break;
                }

                var receiveBytes = new byte[endIndex - startIndex - 1];
                Buffer.BlockCopy(infoBytes, startIndex + 1, receiveBytes, 0, endIndex - startIndex - 1);

                var messageBody = Encoding.ASCII.GetString(receiveBytes);

                var messageArray = messageBody.Split('|');

                //var messageArray = messageBody.Split(':');
                if (messageArray.Length < 3)
                {
                    return;
                }

                messagesDict.TryAdd(messageArray[0], messageBody);

                if (infoBytes.Length <= endIndex + 1)
                {
                    return;
                }
                var remainByte = new byte[infoBytes.Length - endIndex - 1];
                Buffer.BlockCopy(infoBytes, endIndex + 1, remainByte, 0, infoBytes.Length - endIndex - 1);

                infoBytes = remainByte;
            }
        }
    }
}