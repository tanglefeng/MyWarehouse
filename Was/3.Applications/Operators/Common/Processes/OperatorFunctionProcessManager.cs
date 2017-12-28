using System.Collections.Concurrent;

namespace Kengic.Was.Operator.Common.Processes
{
    public class OperatorFunctionProcessManager
    {
        private readonly ConcurrentDictionary<string, OperatorFunctionProcess> _processQueue =
            new ConcurrentDictionary<string, OperatorFunctionProcess>();

        public void RegisterProcess(string processName, OperatorFunctionProcess functionProcess)
            => _processQueue.AddOrUpdate(processName, functionProcess, (k, v) => functionProcess);

        public bool UnRegisterProcess(string processName)
        {
            OperatorFunctionProcess outValue;
            return _processQueue.TryRemove(processName, out outValue);
        }

        public OperatorFunctionProcess GetProcess(string processName)
        {
            OperatorFunctionProcess outValue;
            return _processQueue.TryGetValue(processName, out outValue) ? outValue : null;
        }

        public bool IsExistProcess(string processName) => _processQueue.ContainsKey(processName);
    }
}