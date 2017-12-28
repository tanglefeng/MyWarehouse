using System;
using System.Net;

namespace Kengic.Was.CrossCutting.TcpService
{
    public class NetworkDataEventArgs : EventArgs
    {
        public NetworkDataEventArgs(string dataId, EndPoint remoteEndPoint, byte[] data)
        {
            DataId = dataId;
            RemoteEndPoint = remoteEndPoint;
            Data = data;
        }

        public string DataId { get; set; }
        public EndPoint RemoteEndPoint { get; set; }
        public byte[] Data { get; set; }
    }
}