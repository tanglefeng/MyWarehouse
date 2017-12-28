using System.Collections.Concurrent;

namespace Kengic.Was.Operator.Common
{
    public class OperatorMessageManager
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, ProcessMessage>> _messageQueue =
            new ConcurrentDictionary<string, ConcurrentDictionary<string, ProcessMessage>>();

        public void RegisterMessageQueue(string messageQueueName,
            ConcurrentDictionary<string, ProcessMessage> queueValue)
            => _messageQueue.AddOrUpdate(messageQueueName, queueValue, (k, v) => queueValue);

        public bool UnRegisterMessageQueue(string messageQueueName)
        {
            ConcurrentDictionary<string, ProcessMessage> outValue;
            return _messageQueue.TryRemove(messageQueueName, out outValue);
        }

        public ConcurrentDictionary<string, ProcessMessage> GetMessageQueue(string messageQueueName)
        {
            ConcurrentDictionary<string, ProcessMessage> outValue;
            return _messageQueue.TryGetValue(messageQueueName, out outValue) ? outValue : null;
        }
    }
}