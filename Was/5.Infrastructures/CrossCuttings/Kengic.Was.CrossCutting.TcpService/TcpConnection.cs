using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace Kengic.Was.CrossCutting.TcpService
{
    public class TcpConnection
    {
        private readonly byte[] _buffer = new byte[4096];

        private readonly string _increasingCode = string.Empty;
        private readonly NetworkStream _stream;

        public TcpConnection(TcpClient client)
        {
            Client = client;
            _stream = Client.GetStream();
        }

        public TcpClient Client { get; }

        public void Start() => DoRead();

        private void DoRead()
        {
            if (!_stream.CanWrite)
            {
                OnClientMiss(new TcpConnectEventArgs(Client.Client.RemoteEndPoint, "Disconnect."));
                return;
            }
            _stream.BeginRead(_buffer, 0, _buffer.Length, GotRead, _stream);
        }

        private void GotRead(IAsyncResult ar)
        {
            try
            {
                var stream = (NetworkStream)ar.AsyncState;
                if (!stream.CanRead)
                {
                    return;
                }
                var count = stream.EndRead(ar);
                var data = _buffer.Take(count).ToArray();
                OnGotData(new NetworkDataEventArgs(_increasingCode, Client.Client.RemoteEndPoint, data));
                DoRead();
            }
            catch (IOException)
            {
                OnClientMiss(new TcpConnectEventArgs(null, "Disconnect."));
            }
        }

        public void SendData(byte[] data, string dataId)
            => _stream.BeginWrite(data, 0, data.Length, GotWrite, Tuple.Create(_stream, dataId, data));

        private void GotWrite(IAsyncResult ar)
        {
            try
            {
                var state = (Tuple<NetworkStream, string, byte[]>)ar.AsyncState;
                if (!state.Item1.CanWrite)
                {
                    return;
                }
                state.Item1.EndWrite(ar);
                OnDataSent(new NetworkDataEventArgs(state.Item2, Client.Client.RemoteEndPoint, state.Item3));
            }
            catch (IOException)
            {
                OnClientMiss(new TcpConnectEventArgs(null, "Disconnect."));
            }
        }

        public event EventHandler<NetworkDataEventArgs> DataGot;
        public event EventHandler<NetworkDataEventArgs> DataSent;
        public event EventHandler<TcpConnectEventArgs> ClientMiss;

        protected virtual void OnGotData(NetworkDataEventArgs e) => DataGot?.Invoke(this, e);

        public void Stop()
        {
            _stream.Close();
            Client.Close();
        }

        protected virtual void OnDataSent(NetworkDataEventArgs e) => DataSent?.Invoke(this, e);

        protected virtual void OnClientMiss(TcpConnectEventArgs e) => ClientMiss?.Invoke(this, e);
    }
}