using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Kengic.Was.CrossCutting.SocketsClient
{
    public class SocketClient : IDisposable
    {
        /// <summary>
        ///     接受到数据时的委托
        /// </summary>
        /// <param name="message"></param>
        public delegate void ReceiveMsgHandler(byte[] message);

        /// <summary>
        ///     发送信息完成的委托
        /// </summary>
        /// <param name="successorfalse"></param>
        public delegate void SendCompleted(bool successorfalse);

        /// <summary>
        ///     开始监听数据的委托
        /// </summary>
        public delegate void StartListenHandler();

        /// <summary>
        ///     连接信号量
        /// </summary>
        private static readonly AutoResetEvent AutoConnectEvent = new AutoResetEvent(false);

        private readonly IPEndPoint _localEndPoint;

        private readonly int _messageBufferSize;

        /// <summary>
        ///     连接点
        /// </summary>
        private readonly IPEndPoint _remoteEndPoint;

        /// <summary>
        ///     客户端  连接Socket对象
        /// </summary>
        private Socket _clientSocket;

        /// <summary>
        ///     监听接收的SocketAsyncEventArgs
        /// </summary>
        private SocketAsyncEventArgs _listenerSocketAsyncEventArgs;

        /// <summary>
        ///     初始化客户端
        /// </summary>
        /// <param name="ip">服务端地址{IP地址}</param>
        /// <param name="port">端口号</param>
        public SocketClient(string remoteIp, int remotePort) : this(null, 0, remoteIp, remotePort)
        {
        }

        /// <summary>
        ///     初始化客户端
        /// </summary>
        /// <param name="ip">服务端地址{IP地址}</param>
        /// <param name="port">端口号</param>
        public SocketClient(string localIp, int localPort, string remoteIp, int remotePort)
        {
            _messageBufferSize = 1024;
            if (!string.IsNullOrEmpty(localIp) && (localPort != 0))
            {
                var localIpAddress = IPAddress.Parse(localIp);
                _localEndPoint = new IPEndPoint(localIpAddress, localPort);
            }
            var remoteIpAddress = IPAddress.Parse(remoteIp);
            _remoteEndPoint = new IPEndPoint(remoteIpAddress, remotePort);

            _clientSocket = new Socket(_remoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        ///     连接状态
        /// </summary>
        public bool Connected => _clientSocket.Connected;

        public void Dispose()
        {
            AutoConnectEvent.Close();
            if (_clientSocket.Connected)
            {
                _clientSocket.Close();
            }
        }

        /// <summary>
        ///     接收到数据时调用的事件
        /// </summary>
        public event ReceiveMsgHandler OnMsgReceived;

        /// <summary>
        ///     开始监听数据的事件
        /// </summary>
        public event StartListenHandler StartListenThread;

        /// <summary>
        ///     发送信息完成的事件
        /// </summary>
        public event SendCompleted OnSended;

        public event Action<SocketError> ServerEvent;
        public event Action ServerStop;
        //public event Action<byte[], int, int> DateReceivedEvent;
        public event Action<byte[], int, int> DataSendedEvent;

        /// <summary>
        ///     断开连接
        /// </summary>
        public void Disconnect() => _clientSocket.Close();

        protected virtual void OnOnMsgReceived(byte[] message)
        {
            var handler = OnMsgReceived;
            handler?.Invoke(message);
        }

        protected virtual void OnStartListenThread()
        {
            var handler = StartListenThread;
            handler?.Invoke();
        }

        /// <summary>
        ///     连接服务端
        /// </summary>
        public bool Connect()
        {
            _clientSocket?.Dispose();
            _clientSocket = new Socket(_remoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            if (_localEndPoint != null)
            {
                _clientSocket.Bind(_localEndPoint);
            }
            var connectArgs = new SocketAsyncEventArgs
            {
                UserToken = _clientSocket,
                RemoteEndPoint = _remoteEndPoint
            };

            connectArgs.Completed += OnConnect;
            _clientSocket.ConnectAsync(connectArgs);

            AutoConnectEvent.WaitOne(10000); //Waiting for the connect result.
            if ((connectArgs.SocketError == SocketError.Success) && _clientSocket.Connected)
            {
                _listenerSocketAsyncEventArgs = new SocketAsyncEventArgs();
                var receiveBuffer = new byte[_messageBufferSize];
                _listenerSocketAsyncEventArgs.UserToken = _clientSocket;
                _listenerSocketAsyncEventArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
                _listenerSocketAsyncEventArgs.Completed += OnReceive;
                Listen();
                return true;
            }
            return false;
        }

        /// <summary>
        ///     连接的完成方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnConnect(object sender, SocketAsyncEventArgs e)
        {
            //AutoConnectEvent.Set();
        }

        /// <summary>
        ///     开始监听线程的入口函数
        /// </summary>
        public void Listen()
        {
            var socket = _listenerSocketAsyncEventArgs.UserToken as Socket;
            if ((socket != null) && socket.Connected)
            {
                socket.ReceiveAsync(_listenerSocketAsyncEventArgs);
            }
        }

        /// <summary>
        ///     接收的完成方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReceive(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation != SocketAsyncOperation.Receive)
            {
                return;
            }
            //OnMsgReceived(string.Format("Server COMMAND: {0}", e.SocketError.ToString()));
            switch (e.SocketError)
            {
                case SocketError.Success:
                    //Listen();
                    if (OnMsgReceived != null)
                    {
                        var infoBytes = new byte[e.BytesTransferred];
                        Buffer.BlockCopy(e.Buffer, 0, infoBytes, 0, e.BytesTransferred);

                        //DateReceivedEvent(e.Buffer, e.Offset, e.BytesTransferred);
                        OnMsgReceived(infoBytes);
                    }

                    Listen();
                    break;
                case SocketError.ConnectionReset:
                    ServerStop?.Invoke();
                    break;
                case SocketError.SocketError:
                    break;
                case SocketError.Interrupted:
                    break;
                case SocketError.AccessDenied:
                    break;
                case SocketError.Fault:
                    break;
                case SocketError.InvalidArgument:
                    break;
                case SocketError.TooManyOpenSockets:
                    break;
                case SocketError.WouldBlock:
                    break;
                case SocketError.InProgress:
                    break;
                case SocketError.AlreadyInProgress:
                    break;
                case SocketError.NotSocket:
                    break;
                case SocketError.DestinationAddressRequired:
                    break;
                case SocketError.MessageSize:
                    break;
                case SocketError.ProtocolType:
                    break;
                case SocketError.ProtocolOption:
                    break;
                case SocketError.ProtocolNotSupported:
                    break;
                case SocketError.SocketNotSupported:
                    break;
                case SocketError.OperationNotSupported:
                    break;
                case SocketError.ProtocolFamilyNotSupported:
                    break;
                case SocketError.AddressFamilyNotSupported:
                    break;
                case SocketError.AddressAlreadyInUse:
                    break;
                case SocketError.AddressNotAvailable:
                    break;
                case SocketError.NetworkDown:
                    break;
                case SocketError.NetworkUnreachable:
                    break;
                case SocketError.NetworkReset:
                    break;
                case SocketError.ConnectionAborted:
                    break;
                case SocketError.NoBufferSpaceAvailable:
                    break;
                case SocketError.IsConnected:
                    break;
                case SocketError.NotConnected:
                    break;
                case SocketError.Shutdown:
                    break;
                case SocketError.TimedOut:
                    break;
                case SocketError.ConnectionRefused:
                    break;
                case SocketError.HostDown:
                    break;
                case SocketError.HostUnreachable:
                    break;
                case SocketError.ProcessLimit:
                    break;
                case SocketError.SystemNotReady:
                    break;
                case SocketError.VersionNotSupported:
                    break;
                case SocketError.NotInitialized:
                    break;
                case SocketError.Disconnecting:
                    break;
                case SocketError.TypeNotFound:
                    break;
                case SocketError.HostNotFound:
                    break;
                case SocketError.TryAgain:
                    break;
                case SocketError.NoRecovery:
                    break;
                case SocketError.NoData:
                    break;
                case SocketError.IOPending:
                    break;
                case SocketError.OperationAborted:
                    break;
                default:
                    ServerEvent?.Invoke(e.SocketError);
                    break;
            }
        }

        /// <summary>
        ///     发送信息
        /// </summary>
        /// <param name="sendBuffer"></param>
        public void Send(byte[] sendBuffer)
        {
            if (Connected)
            {
                var senderSocketAsyncEventArgs = new SocketAsyncEventArgs {UserToken = _clientSocket};
                senderSocketAsyncEventArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
                senderSocketAsyncEventArgs.RemoteEndPoint = _remoteEndPoint;
                senderSocketAsyncEventArgs.Completed += OnSend;
                _clientSocket.SendAsync(senderSocketAsyncEventArgs);
                DataSendedEvent?.Invoke(sendBuffer, 0, sendBuffer.Length);
            }
            else
            {
                OnSended?.Invoke(false);
            }
        }

        /// <summary>
        ///     发送的完成方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSend(object sender, SocketAsyncEventArgs e)
            => OnSended?.Invoke(e.SocketError == SocketError.Success);
    }
}