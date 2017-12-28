using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Kengic.Was.CrossCutting.SocketsServer
{
    public sealed class BufferManager : IDisposable
    {
        private readonly int _bufferSize;
        private readonly int _numSize;
        private byte[] _buffer;
        private int _currentIndex;
        private Stack<int> _freeIndexPool;

        public BufferManager(int numSize, int bufferSize)
        {
            _bufferSize = bufferSize;
            _numSize = numSize;
            _currentIndex = 0;
            _freeIndexPool = new Stack<int>();
        }

        public void Dispose()
        {
            _buffer = null;
            _freeIndexPool = null;
        }

        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            _freeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }

        public void InitBuffer() => _buffer = new byte[_numSize];

        public bool SetBuffer(SocketAsyncEventArgs args)
        {
            if (_freeIndexPool.Count > 0)
            {
                args.SetBuffer(_buffer, _freeIndexPool.Pop(), _bufferSize);
            }
            else
            {
                if (_numSize - _bufferSize < _currentIndex)
                {
                    return false;
                }
                args.SetBuffer(_buffer, _currentIndex, _bufferSize);
                _currentIndex += _bufferSize;
            }
            return true;
        }

        public bool SetBuffer(SocketAsyncEventArgs args, int currentIndex, int bufferSize)
        {
            args.SetBuffer(_buffer, currentIndex, bufferSize);
            return true;
        }
    }
}