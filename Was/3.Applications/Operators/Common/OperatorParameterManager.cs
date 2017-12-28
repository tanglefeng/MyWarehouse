using System;
using System.Collections.Concurrent;

namespace Kengic.Was.Operator.Common
{
    public class OperatorParameterManager
    {
        private readonly ConcurrentDictionary<string, Tuple<Type, object>> _parameterQueue =
            new ConcurrentDictionary<string, Tuple<Type, object>>();

        public void RegisterParameter(string parameterName, Type type, object parameterValue)
            => _parameterQueue.AddOrUpdate(parameterName, new Tuple<Type, object>(type, parameterValue),
                (k, v) => new Tuple<Type, object>(type, parameterValue));

        public bool UnregisterParameter(string parameterName)
        {
            Tuple<Type, object> outValue;
            return _parameterQueue.TryRemove(parameterName, out outValue);
        }

        public T GetParameter<T>(string parameterName)
        {
            Tuple<Type, object> outValue;
            _parameterQueue.TryGetValue(parameterName, out outValue);
            if (outValue != null)
            {
                return (T) Convert.ChangeType(outValue.Item2, typeof (T));
            }

            return default(T);
        }
    }
}