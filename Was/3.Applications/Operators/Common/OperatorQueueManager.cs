using System.Collections.Concurrent;

namespace Kengic.Was.Operator.Common
{
    public class OperatorQueueManager
    {
        private readonly ConcurrentDictionary<string, object> _objQueue = new ConcurrentDictionary<string, object>();

        public void RegisterQueue(string queueName, object queueValue)
            => _objQueue.AddOrUpdate(queueName, queueValue, (k, v) => queueValue);

        public bool UnRegisterQueue(string queueName)
        {
            object outValue;
            return _objQueue.TryRemove(queueName, out outValue);
        }

        public object GetQueue(string queueName)
        {
            object outValue;
            return _objQueue.TryGetValue(queueName, out outValue) ? outValue : null;
        }

        public bool IsExistQueue(string queueName) => _objQueue.ContainsKey(queueName);
    }
}