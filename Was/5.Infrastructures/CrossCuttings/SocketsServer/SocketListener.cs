using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Kengic.Was.CrossCutting.SocketsServer
{
    public enum ServerState
    {
        Initialing,
        Inited,
        Ready,
        Running,
        Stoped
    }

    public sealed class SocketListener : IDisposable
    {
        /// <summary>
        ///     客户端关闭连接时的委托
        /// </summary>
        /// <param name="uid"></param>
        public delegate void ClientClose(string uid);

        /// <summary>
        ///     回调委托
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public delegate string GetIdbyIpFun(string ip);

        /// <summary>
        ///     接收到信息时的事件委托
        /// </summary>
        /// <param name="info"></param>
        public delegate void ReceiveMsgHandler(string uid, string info, byte[] infoBytes);

        /// <summary>
        ///     发送信息完成后的委托
        /// </summary>
        public delegate void SendCompletedHandler(string uid, string exception);

        /// <summary>
        ///     开始监听数据的委托
        /// </summary>
        public delegate void StartListenHandler();

        /// <summary>
        ///     读取写入字节
        /// </summary>
        private const int OpsToPreAlloc = 1;

        /// <summary>
        ///     回调方法实例
        /// </summary>
        private readonly GetIdbyIpFun _getIdbyIp;

        /// <summary>
        ///     服务同步锁
        /// </summary>
        private readonly Mutex _mutex;

        /// <summary>
        ///     并发控制信号量
        /// </summary>
        private readonly Semaphore _semaphoreAcceptedClients;

        /// <summary>
        ///     缓冲区
        /// </summary>
        private BufferManager _bufferManager;

        /// <summary>
        ///     服务器端Socket
        /// </summary>
        private Socket _listenSocket;

        /// <summary>
        ///     当前连接数
        /// </summary>
        private int _numConnections;

        /// <summary>
        ///     Socket连接池
        /// </summary>
        private SocketAsyncEventArgsPool _readWritePool;

        /// <summary>
        ///     初始化服务器端
        /// </summary>
        /// <param name="numConcurrence">并发的连接数量(1000以上)</param>
        /// <param name="receiveBufferSize">每一个收发缓冲区的大小(32768)</param>
        /// <param name="getIdbyIp"></param>
        public SocketListener(int numConcurrence, int receiveBufferSize, GetIdbyIpFun getIdbyIp)
        {
            ListenFlag = false;
            try
            {
                _mutex = new Mutex();

                State = ServerState.Initialing;
                _numConnections = 0;
                MaxConcurrence = numConcurrence;
                _bufferManager = new BufferManager(receiveBufferSize*numConcurrence*OpsToPreAlloc, receiveBufferSize);
                _readWritePool = new SocketAsyncEventArgsPool(numConcurrence);
                _semaphoreAcceptedClients = new Semaphore(numConcurrence, numConcurrence);
                Handler = new RequestHandler();
                _getIdbyIp = getIdbyIp;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("socket listening exception" + ex);
            }
        }

        /// <summary>
        ///     通信协议
        /// </summary>
        public RequestHandler Handler { get; private set; }

        public bool ListenFlag { get; set; }

        /// <summary>
        ///     获取当前的并发数
        /// </summary>
        public int NumConnections => _numConnections;

        /// <summary>
        ///     最大并发数
        /// </summary>
        public int MaxConcurrence { get; }

        /// <summary>
        ///     返回服务器状态
        /// </summary>
        public ServerState State { get; private set; }

        /// <summary>
        ///     获取当前在线用户的UID
        /// </summary>
        public string[] OnlineUid => _readWritePool?.OnlineUid;

        public void Dispose()
        {
            try
            {
                if (_bufferManager != null)
                {
                    _bufferManager.Dispose();
                    _bufferManager = null;
                }
                if (_readWritePool != null)
                {
                    _readWritePool.Dispose();
                    _readWritePool = null;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("dispose exception" + ex);
            }
        }

        /// <summary>
        ///     接收到信息时的事件
        /// </summary>
        public event ReceiveMsgHandler OnMsgReceived;

        /// <summary>
        ///     客户端关闭连接时的事件
        /// </summary>
        public event ClientClose OnClientClose;

        /// <summary>
        ///     开始监听数据的事件
        /// </summary>
        public event StartListenHandler StartListenThread;

        /// <summary>
        ///     发送信息完成后的事件
        /// </summary>
        public event SendCompletedHandler OnSended;

        /// <summary>
        ///     服务端初始化
        /// </summary>
        public void Init()
        {
            try
            {
                _bufferManager.InitBuffer();
                for (var i = 0; i < MaxConcurrence; i++)
                {
                    var readWriteEventArgWithId = new SocketAsyncEventArgsWithId();

                    readWriteEventArgWithId.SendSaea.Completed += OnSendCompleted;
                    readWriteEventArgWithId.ReceiveSaea.Completed += OnReceiveCompleted;
                    //只给接收的SocketAsyncEventArgs设置缓冲区
                    _bufferManager.SetBuffer(readWriteEventArgWithId.ReceiveSaea);
                    _readWritePool.Push(readWriteEventArgWithId);
                }
                State = ServerState.Inited;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("initialize socket exception" + ex);
            }
        }

        /// <summary>
        ///     启动服务器
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="data">端口号</param>
        public void Start(string ip, object data)
        {
            try
            {
                var port = (int) data;
                //var addresslist = Dns.GetHostEntry(Environment.MachineName).AddressList;
                var ipAddress = IPAddress.Parse(ip); //addresslist[addresslist.Length - 1];
                //object listenServerIp = ConfigurationManager.AppSettings["ListenServerIP"];
                //if (listenServerIp != null)
                //{
                //    IPAddress.TryParse(listenServerIp.ToString(), out ipAddress);
                //}
                var localEndPoint = new IPEndPoint(ipAddress, port);
                _listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                if (localEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    _listenSocket.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName) 27, false);
                    _listenSocket.Bind(new IPEndPoint(IPAddress.IPv6Any, localEndPoint.Port));
                }
                else
                {
                    _listenSocket.Bind(localEndPoint);
                }
                _listenSocket.Listen(100);
                StartAccept(null);
                //开始监听已连接用户的发送数据
                StartListenThread?.Invoke();
                State = ServerState.Running;

                _mutex.WaitOne();

                _mutex.ReleaseMutex();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("start socket exception" + ex);
            }
        }

        /// <summary>
        ///     开始监听线程的入口函数
        /// </summary>
        public void Listen()
        {
            while (ListenFlag)
            {
                var currUser = string.Empty;
                try
                {
                    var keys = _readWritePool.OnlineUid;
                    foreach (var uid in keys)
                    {
                        currUser = uid;
                        lock (string.Empty)
                        {
                            if ((uid != null) && (_readWritePool != null) && _readWritePool.BusyPool.ContainsKey(uid) &&
                                (_readWritePool.BusyPool[uid] != null))
                            {
                                var sawd = _readWritePool.BusyPool[uid];
                                if (sawd != null)
                                {
                                    var socket = _readWritePool.BusyPool[uid].ReceiveSaea.UserToken as Socket;
                                    if ((socket != null) && (socket.Available > 0))
                                    {
                                        var willRaiseEvent =
                                            socket.ReceiveAsync(_readWritePool.BusyPool[uid].ReceiveSaea);
                                        if (!willRaiseEvent)
                                        {
                                            ProcessReceive(_readWritePool.BusyPool[uid].ReceiveSaea);
                                        }
                                    }
                                }
                                else
                                {
                                    CloseClientSocket(currUser);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    CloseClientSocket(currUser);
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        ///     发送信息
        /// </summary>
        /// <param name="uid">要发送的用户的uid</param>
        /// <param name="msg">消息体</param>
        public void Send(string uid, string msg)
        {
            var message = msg;
            var sendbuffer = Encoding.Default.GetBytes(message);
            Send(uid, sendbuffer);
        }

        public void Send(string uid, byte[] msgBytes)
        {
            try
            {
                if (string.IsNullOrEmpty(uid) || (msgBytes.Length <= 0))
                {
                    return;
                }
                if (_readWritePool == null)
                {
                    return;
                }
                var socketWithId = _readWritePool.FindByUid(uid);
                if (socketWithId == null)
                    //说明用户已经断开  
                    //100   发送成功
                    //200   发送失败
                    //300   用户不在线
                    //其它  表示异常的信息
                {
                    OnSended?.Invoke(uid, "300");
                }
                else
                {
                    var e = socketWithId.SendSaea;
                    if (e.SocketError == SocketError.Success)
                    {
                        try
                        {
                            lock (string.Empty)
                            {
                                //string message = @"[lenght=" + msg.Length + @"]" + msg;
                                //var message = msgBytes;
                                //e.SetBuffer(sendbuffer, 0, sendbuffer.Length);
                                var socket = (Socket) e.UserToken;
                                socket?.Send(msgBytes);
                            }
                        }
                        catch (Exception ex)
                        {
                            OnSended?.Invoke(uid, ex.ToString());
                            CloseClientSocket(e.Uid);
                        }
                    }
                    else
                    {
                        OnSended?.Invoke(uid, "200");
                        CloseClientSocket(e.Uid);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("send exception" + ex);
            }
        }

        /// <summary>
        ///     停止服务器
        /// </summary>
        public void Stop()
        {
            try
            {
                ListenFlag = false;
                _listenSocket?.Close();
                _listenSocket = null;
                Dispose();
                //try
                //{
                //    _mutex.ReleaseMutex();
                //}
                //catch (ApplicationException ae)
                //{
                //    throw new ApplicationException(ae.ToString());
                //}
                State = ServerState.Stoped;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("stop socket exception" + ex);
            }
        }

        private void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            try
            {
                if (acceptEventArg == null)
                {
                    acceptEventArg = new SocketAsyncEventArgs();
                    acceptEventArg.Completed += OnAcceptCompleted;
                }
                else
                {
                    acceptEventArg.AcceptSocket = null;
                }
                _semaphoreAcceptedClients.WaitOne();
                var willRaiseEvent = _listenSocket.AcceptAsync(acceptEventArg);
                if (!willRaiseEvent)
                {
                    ProcessAccept(acceptEventArg);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("start accept exception" + ex);
            }
        }

        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs e) => ProcessAccept(e);

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            try
            {
                if (e.LastOperation != SocketAsyncOperation.Accept) //检查上一次操作是否是Accept，不是就返回
                {
                    return;
                }
                if (State == ServerState.Stoped)
                {
                    return;
                }
                if (e.AcceptSocket?.RemoteEndPoint == null) //服务器断开连接
                {
                    return;
                }
                var ipEndPoint = e.AcceptSocket.RemoteEndPoint as IPEndPoint;
                if (ipEndPoint != null)
                {
                    var uid = _getIdbyIp(ipEndPoint.Address + ":" + ipEndPoint.Port); //根据IP获取用户的UID
                    if (string.IsNullOrEmpty(uid) || (uid == string.Empty))
                    {
                        return;
                    }
                    if (_readWritePool.BusyPoolContains(uid)) //判断现在的用户是否已经连接,避免同一用户开两个连接
                    {
                        CloseClientSocket(uid);
                        return;
                    }
                    var readEventArgsWithId = _readWritePool.Pop(uid);
                    if (readEventArgsWithId.ReceiveSaea.SocketError != SocketError.Success)
                    {
                        if (readEventArgsWithId.ReceiveSaea.SocketError != SocketError.ConnectionReset)
                        {
                            readEventArgsWithId.ReceiveSaea.UserToken = null;
                            readEventArgsWithId.ReceiveSaea.Dispose();
                            readEventArgsWithId.ReceiveSaea = new MySocketAsyncEventArgs("Receive");
                            readEventArgsWithId.ReceiveSaea.Completed += OnReceiveCompleted;
                        }
                    }
                    if (readEventArgsWithId.SendSaea.SocketError != SocketError.Success)
                    {
                        readEventArgsWithId.SendSaea.UserToken = null;
                        readEventArgsWithId.SendSaea.Dispose();
                        readEventArgsWithId.SendSaea = new MySocketAsyncEventArgs("Send");
                        readEventArgsWithId.SendSaea.Completed += OnSendCompleted;
                    }

                    readEventArgsWithId.ReceiveSaea.UserToken = e.AcceptSocket;
                    readEventArgsWithId.SendSaea.UserToken = e.AcceptSocket;
                    readEventArgsWithId.IsAllowReceive = true;
                    Interlocked.Increment(ref _numConnections);
                    //触发处理客户端连接完成事件
                    ProcessAcceptComplete?.Invoke(uid, e);
                }
                StartAccept(null);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("process accept exception" + ex);
            }
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs e) => ProcessReceive(e);
        private void OnSendCompleted(object sender, SocketAsyncEventArgs e) => ProcessSend(e);

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            try
            {
                lock (string.Empty)
                {
                    if (e.LastOperation != SocketAsyncOperation.Receive)
                    {
                        return;
                    }
                    // 检查如果远程主机关闭了连接
                    if (e.BytesTransferred > 0)
                    {
                        if (e.SocketError == SocketError.Success)
                        {
                            var byteTransferred = e.BytesTransferred;
                            var received = Encoding.Default.GetString(e.Buffer, e.Offset, byteTransferred);
                            var receiveBytes = new byte[byteTransferred];
                            Array.Copy(e.Buffer, e.Offset, receiveBytes, 0, byteTransferred);
                            OnMsgReceived?.Invoke(((MySocketAsyncEventArgs) e).Uid, received, receiveBytes);

                            try
                            {
                                //可以在这里设一个停顿来实现间隔时间段监听，这里的停顿是单个用户间的监听间隔
                                //发送一个异步接受请求，并获取请求是否为成功
                                var socket = (Socket) e.UserToken;
                                var willRaiseEvent = (socket != null) && socket.ReceiveAsync(e);
                                if (!willRaiseEvent)
                                {
                                    ProcessReceive(e);
                                }
                            }
                            catch (InvalidOperationException ex)
                            {
                                throw new ApplicationException("Invalid Operation Exception" + ex);
                            }
                        }
                    }
                    else
                    {
                        CloseClientSocket(((MySocketAsyncEventArgs) e).Uid);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("process receive exception" + ex);
            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            try
            {
                // 检查如果远程主机关闭了连接
                if (e.SocketError == SocketError.Success)
                {
                    if (e.LastOperation != SocketAsyncOperation.Send)
                    {
                        return;
                    }
                    if (e.BytesTransferred <= 0)
                    {
                        return;
                    }
                    OnSended?.Invoke(((MySocketAsyncEventArgs) e).Uid,
                        e.SocketError == SocketError.Success ? "100" : "200");
                }
                else
                {
                    CloseClientSocket(((MySocketAsyncEventArgs) e).Uid);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("process send exception" + ex);
            }
        }

        private void CloseClientSocket(string uid)
        {
            try
            {
                if (uid == string.Empty)
                {
                    return;
                }

                if (_readWritePool == null)
                {
                    return;
                }
                var saeaw = _readWritePool.FindByUid(uid);
                if (saeaw == null)
                {
                    return;
                }
                var s = saeaw.ReceiveSaea.UserToken as Socket;
                try
                {
                    s?.Shutdown(SocketShutdown.Both);
                    saeaw.ReceiveSaea.DisconnectReuseSocket = true;
                }
                catch
                {
                    //客户端已经关闭
                    OnClientClose?.Invoke(uid);
                    return;
                }
                _semaphoreAcceptedClients.Release();

                Interlocked.Decrement(ref _numConnections);
                IniSocketAsyncEventArgsWithId(saeaw);
                _readWritePool.Push(saeaw);
                //出发事件
                OnClientClose?.Invoke(uid);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("close socket exception" + ex);
            }
        }

        /// <summary>
        ///     处理接收客户端连接完成事件
        /// </summary>
        public event EventHandler<SocketAsyncEventArgs> ProcessAcceptComplete;

        private void IniSocketAsyncEventArgsWithId(SocketAsyncEventArgsWithId readEventArgsWithId)
        {
            var currentIndex = readEventArgsWithId.ReceiveSaea.Offset;
            var bufferSize = readEventArgsWithId.ReceiveSaea.Count;
            readEventArgsWithId.ReceiveSaea.UserToken = null;
            readEventArgsWithId.ReceiveSaea.Dispose();
            readEventArgsWithId.ReceiveSaea = new MySocketAsyncEventArgs("Receive");
            readEventArgsWithId.ReceiveSaea.Completed += OnReceiveCompleted;
            _bufferManager.SetBuffer(readEventArgsWithId.ReceiveSaea, currentIndex, bufferSize);
        }
    }
}