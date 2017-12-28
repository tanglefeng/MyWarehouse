using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Kengic.Was.CrossCutting.TcpService
{
    public class TcpService
    {
        private readonly List<TcpConnection> _connectionList = new List<TcpConnection>();
        private InsideTcpListener _listener;


        private void DoListen(TcpListener listener)
        {
            listener.Start();
            listener.BeginAcceptTcpClient(GotAccept, listener);
        }

        private void GotAccept(IAsyncResult ar)
        {
            var listener = (InsideTcpListener) ar.AsyncState;
            if (!listener.IsActive)
            {
                return;
            }
            var client = listener.EndAcceptTcpClient(ar);
            OnClientGot(new TcpConnectEventArgs(client.Client.RemoteEndPoint, "Connected."));
            var connection = new TcpConnection(client);
            _connectionList.Add(connection);
            connection.DataGot += Connection_DataGot;
            connection.DataSent += Connection_DataSent;
            connection.ClientMiss += Connection_ClientMiss;
            connection.Start();
            DoListen(listener);
        }

        private void Connection_ClientMiss(object sender, TcpConnectEventArgs e)
        {
            var connection = (TcpConnection) sender;
            connection.Stop();
            _connectionList.Remove(connection);
            OnClientMiss(e);
        }

        private void Connection_DataSent(object sender, NetworkDataEventArgs e) => OnDataSent(e);

        private void Connection_DataGot(object sender, NetworkDataEventArgs e) => OnDataGot(e);

        public void Start(IPAddress address, int port)
        {
            _listener = new InsideTcpListener(address, port);
            DoListen(_listener);
        }

        public void Stop()
        {
            _listener.Stop();
            foreach (var tcpConnection in _connectionList)
            {
                tcpConnection.Stop();
            }
            _connectionList.Clear();
        }

        public void Broadcast(string dataId, byte[] data)
        {
            foreach (var tcpConnection in _connectionList)
            {
                tcpConnection.SendData(data, dataId);
            }
        }

        public event EventHandler<NetworkDataEventArgs> DataGot;
        public event EventHandler<TcpConnectEventArgs> ClientGot;
        public event EventHandler<NetworkDataEventArgs> DataSent;
        public event EventHandler<TcpConnectEventArgs> ClientMiss;

        protected virtual void OnDataGot(NetworkDataEventArgs e) => DataGot?.Invoke(this, e);

        protected virtual void OnClientGot(TcpConnectEventArgs e) => ClientGot?.Invoke(this, e);

        protected virtual void OnDataSent(NetworkDataEventArgs e) => DataSent?.Invoke(this, e);

        protected virtual void OnClientMiss(TcpConnectEventArgs e) => ClientMiss?.Invoke(this, e);

        private sealed class InsideTcpListener : TcpListener
        {
            public InsideTcpListener(IPAddress localaddr, int port) : base(localaddr, port)
            {
            }

            public bool IsActive => Active;
        }
    }
}