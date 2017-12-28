using System.Collections.Concurrent;

namespace Kengic.Was.Operator.Common.Methods
{
    public class OperatorFunctionMethodManager
    {
        private readonly ConcurrentDictionary<string, OperatorFunctionMethod> _methodQueue =
            new ConcurrentDictionary<string, OperatorFunctionMethod>();

        public void RegisterMethod(string methodName, OperatorFunctionMethod functionProcess)
            => _methodQueue.AddOrUpdate(methodName, functionProcess, (k, v) => functionProcess);

        public bool UnRegisterMethod(string methodName)
        {
            OperatorFunctionMethod outValue;
            return _methodQueue.TryRemove(methodName, out outValue);
        }

        public OperatorFunctionMethod GetMethod(string methodName)
        {
            OperatorFunctionMethod outValue;
            return _methodQueue.TryGetValue(methodName, out outValue) ? outValue : null;
        }

        public bool IsExistMethod(string methodName) => _methodQueue.ContainsKey(methodName);
    }
}