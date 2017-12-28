using System.Net.Sockets;

namespace Kengic.Was.CrossCutting.SocketsServer
{
    public sealed class MySocketAsyncEventArgs : SocketAsyncEventArgs
    {
        /// <summary>
        ///     构造方法
        /// </summary>
        /// <param name="property">读写操作类型:Receive/Send</param>
        public MySocketAsyncEventArgs(string property)
        {
            Property = property;
        }

        public string Property { get; private set; }

        /// <summary>
        ///     用户标识
        /// </summary>
        public string Uid { get; set; }
    }
}