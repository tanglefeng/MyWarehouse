using System;
using System.Net.Sockets;
using System.Threading;

namespace Kengic.Was.CrossCutting.SocketsServer
{
    public class SocketServer
    {
        public delegate void ClientDisconnectEventHadler(string uid, string exception);

        private const int BufferSize = 1024*1024; //接收缓冲区的大小
        private readonly string _ipAddress;
        private readonly int _maxConnection; //允许的最大连接数
        private readonly int _port; //监听的端口号
        private SocketListener _listener;

        public SocketServer(int maxConnection, string ip, int port)
        {
            _maxConnection = maxConnection;
            _ipAddress = ip;
            _port = port;
        }

        /// <summary>
        ///     处理客户端连接完成事件
        /// </summary>
        /// <param name="sender">客户端ip</param>
        /// <param name="e"></param>
        private void listener_ProcessAcceptComplete(object sender, SocketAsyncEventArgs e)
            => ProcessAcceptComplete?.Invoke(sender, e);

        private void listener_OnMsgReceived(string uid, string info, byte[] infoBytes)
            => ReceiveMsgHandler?.Invoke(uid, info, infoBytes);

        public event ClientDisconnectEventHadler OnClientDisconnect;

        private void listener_OnSended(string uid, string exception)
        {
            if (exception == "100")
            {
                return;
            }
            OnClientDisconnect?.Invoke(uid, exception);
        }

        /// <summary>
        ///     客户端断开连接的事件处理程序
        /// </summary>
        /// <param name="uid"></param>
        private void listener_OnClientClose(string uid) => OnClientDisconnect?.Invoke(uid, "Disconnect");

        private void listener_StartListenThread() => ThreadPool.QueueUserWorkItem(StartListenCallBack, true);

        private void StartListenCallBack(object listenFlag)
        {
            _listener.ListenFlag = Convert.ToBoolean(listenFlag);
            _listener.Listen();
        }

        /// <summary>
        ///     获取侦听的服务器IP地址
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static string GetIdbyIp(string ip) => ip;

        /// <summary>
        ///     启动服务
        /// </summary>
        public bool StartService()
        {
            try
            {
                //LoggingService.Debug("最大连接数：" + _maxConnection);
                //LoggingService.Debug("侦听的端口号：" + _port);
                _listener = new SocketListener(_maxConnection, BufferSize, GetIdbyIp);
                _listener.StartListenThread += listener_StartListenThread;
                _listener.ProcessAcceptComplete += listener_ProcessAcceptComplete;
                _listener.OnSended += listener_OnSended;
                _listener.OnMsgReceived += listener_OnMsgReceived;
                _listener.OnClientClose += listener_OnClientClose;
                _listener.Init();
                try
                {
                    _listener.Start(_ipAddress, _port);
                    return true;
                }
                catch (SocketException se)
                {
                    if (se.SocketErrorCode == SocketError.AddressAlreadyInUse)
                    {
                        throw new ApplicationException("socket is in used" + _port);
                    }
                    throw new ApplicationException("socket start exception");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
                //return false;
            }
        }

        /// <summary>
        ///     重启服务
        /// </summary>
        public void ReStartService()
        {
            StopService();
            StartService();
        }

        /// <summary>
        ///     停止服务
        /// </summary>
        public void StopService()
        {
            try
            {
                _listener.Stop();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("stop socket listening exception" + ex.Message);
            }
        }

        /// <summary>
        ///     发送消息至所有连接至本服务器的客户端计算机
        /// </summary>
        /// <param name="msg">消息内容</param>
        public void NetSendMsg(string msg)
        {
            foreach (var uid in _listener.OnlineUid)
            {
                _listener.Send(uid, msg);
            }
        }

        /// <summary>
        ///     发送消息至指定计算机
        /// </summary>
        /// <param name="uid">计算机IP地址</param>
        /// <param name="msg">消息内容</param>
        public void NetSendMsg(string uid, string msg) => _listener.Send(uid, msg);

        /// <summary>
        ///     发送消息至指定计算机
        /// </summary>
        /// <param name="uid">计算机IP地址</param>
        /// <param name="msgbyBytes">消息内容</param>
        public void NetSendMsg(string uid, byte[] msgbyBytes) => _listener.Send(uid, msgbyBytes);

        public void NetSendMsg(byte[] msgbyBytes)
        {
            var onlineUid = _listener.OnlineUid;
            if (onlineUid == null)
            {
                return;
            }
            foreach (var uid in _listener.OnlineUid)
            {
                _listener.Send(uid, msgbyBytes);
            }
        }

        /// <summary>
        ///     接收到信息时触发的事件
        /// </summary>
        public event SocketListener.ReceiveMsgHandler ReceiveMsgHandler;

        /// <summary>
        ///     处理接收客户端连接完成事件
        /// </summary>
        public event EventHandler<SocketAsyncEventArgs> ProcessAcceptComplete;
    }
}