using System;

namespace Kengic.Was.CrossCutting.SocketsServer
{
    public sealed class SocketAsyncEventArgsWithId : IDisposable
    {
        private string _uid = "-1";

        public SocketAsyncEventArgsWithId()
        {
            State = false;
            ReceiveSaea = new MySocketAsyncEventArgs("Receive");
            SendSaea = new MySocketAsyncEventArgs("Send");
        }

        public string Uid
        {
            get { return _uid; }
            set
            {
                _uid = value;
                ReceiveSaea.Uid = value;
                SendSaea.Uid = value;
            }
        }

        public bool State { get; set; }

        /// <summary>
        ///     是否允许接收
        /// </summary>
        public bool IsAllowReceive { get; set; } = true;

        public MySocketAsyncEventArgs ReceiveSaea { get; set; }
        public MySocketAsyncEventArgs SendSaea { get; set; }

        public void Dispose()
        {
            ReceiveSaea.Dispose();
            SendSaea.Dispose();
        }
    }
}