using System;
using System.Collections.Generic;
using System.Linq;

namespace Kengic.Was.CrossCutting.SocketsServer
{
    public sealed class SocketAsyncEventArgsPool : IDisposable
    {
        public SocketAsyncEventArgsPool(int capacity)
        {
            //keys = new string[capacity];
            Pool = new Stack<SocketAsyncEventArgsWithId>(capacity);
            BusyPool = new Dictionary<string, SocketAsyncEventArgsWithId>(capacity);
        }

        public int Count
        {
            get
            {
                lock (Pool)
                {
                    return Pool.Count;
                }
            }
        }

        public string[] OnlineUid
        {
            get
            {
                string[] keys;
                lock (BusyPool)
                {
                    //busypool.Keys.CopyTo(keys, 0);
                    keys = BusyPool.Keys.ToArray();
                }
                return keys;
            }
        }

        public Stack<SocketAsyncEventArgsWithId> Pool { get; private set; }
        public IDictionary<string, SocketAsyncEventArgsWithId> BusyPool { get; private set; }

        public void Dispose()
        {
            Pool.Clear();
            BusyPool.Clear();
            Pool = null;
            BusyPool = null;
        }

        public SocketAsyncEventArgsWithId Pop(string uid)
        {
            if (string.IsNullOrEmpty(uid))
            {
                return null;
            }
            SocketAsyncEventArgsWithId si;
            lock (Pool)
            {
                si = Pool.Pop();
            }
            si.Uid = uid;
            si.State = true; //mark the state of pool is not the initial step 
            BusyPool.Add(uid, si);
            return si;
        }

        public void Push(SocketAsyncEventArgsWithId item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (item.State)
            {
                if (BusyPool.Keys.Count != 0)
                {
                    if (BusyPool.Keys.Contains(item.Uid))
                    {
                        BusyPool.Remove(item.Uid);
                    }
                    //else
                    //    throw new ArgumentException("SocketAsyncEventWithId不在忙碌队列中");
                }
                else
                {
                    throw new ArgumentException("忙碌队列为空");
                }
            }
            item.Uid = "-1";
            item.State = false;
            lock (Pool)
            {
                Pool.Push(item);
            }
        }

        public SocketAsyncEventArgsWithId FindByUid(string uid)
        {
            if ((uid == string.Empty) || (uid == string.Empty))
            {
                return null;
            }
            SocketAsyncEventArgsWithId si = null;
            if (OnlineUid.Any(key => key == uid))
            {
                si = BusyPool[uid];
            }
            return si;
        }

        public bool BusyPoolContains(string uid)
        {
            lock (BusyPool)
            {
                return BusyPool.Keys.Contains(uid);
            }
        }
    }
}