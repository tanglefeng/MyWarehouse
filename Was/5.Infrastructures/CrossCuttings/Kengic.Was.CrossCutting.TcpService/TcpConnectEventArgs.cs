using System;
using System.Net;

namespace Kengic.Was.CrossCutting.TcpService
{
    public class TcpConnectEventArgs : EventArgs
    {
        public TcpConnectEventArgs(EndPoint remoteEndPoint, string message)
        {
            RemoteEndPoint = remoteEndPoint;
            Message = message;
        }

        public EndPoint RemoteEndPoint { get; set; }
        public string Message { get; set; }
    }
}