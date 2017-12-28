using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kengic.Was.CrossCutting.Common
{
    public static class JSonHelper
    {
        public static T GetValue<T>(JObject jObject, string keyValue)
        {
            var jToken = jObject[keyValue];
            var objectValue = jToken?.ToObject<object>();
            if (objectValue != null)
            {
                return (T) Convert.ChangeType(jToken, typeof (T));
            }
            return default(T);
        }

        public static T JsonToValue<T>(this string keyValue)
        {
            var result = JsonConvert.DeserializeObject<T>(keyValue);

            return result;
        }

        public static string ToShortString<T>(this T obj, Func<T, object> selector = null)
        {
            string result = null;
            if (selector != null)
            {
                var objSelctor = selector(obj);
                result = JsonConvert.SerializeObject(objSelctor);
            }
            else
            {
                if (JsonFormatters.TypeConcurrentDictionary.ContainsKey(typeof (T)))
                {
                    Delegate configurationSelector;
                    JsonFormatters.TypeConcurrentDictionary.TryGetValue(typeof (T), out configurationSelector);
                    if (configurationSelector != null)
                    {
                        result = JsonConvert.SerializeObject(configurationSelector.DynamicInvoke(obj));
                    }
                }
                if (result == null)
                {
                    result = JsonConvert.SerializeObject(obj);
                }
            }

            return result;
        }
    }
}