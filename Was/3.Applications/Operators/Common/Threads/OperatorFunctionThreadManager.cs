using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Kengic.Was.Operator.Common.Threads
{
    public class OperatorFunctionThreadManager
    {
        private readonly ConcurrentDictionary<string, OperatorFunctionThread> _threadQueue =
            new ConcurrentDictionary<string, OperatorFunctionThread>();

        public void RegisterThread(string threadName, OperatorFunctionThread functionThread)
            => _threadQueue.AddOrUpdate(threadName, functionThread, (k, v) => functionThread);

        public bool UnRegisterThread(string threadName)
        {
            OperatorFunctionThread outValue;
            return _threadQueue.TryRemove(threadName, out outValue);
        }

        public OperatorFunctionThread GetThread(string threadName)
        {
            OperatorFunctionThread outValue;
            return _threadQueue.TryGetValue(threadName, out outValue) ? outValue : null;
        }

        public List<OperatorFunctionThread> GetThreadList() => _threadQueue.Values.ToList();
    }
}