using System;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    public class ExpressionJsonConverter : JsonConverter
    {
        private static readonly Type TypeOfExpression = typeof (Expression);
        private IQueryHandler _queryHandler;
        public ExpressionJsonConverter(IQueryHandler queryHandler)
        {
            _queryHandler = queryHandler;
        }
        public ExpressionJsonConverter()
        {
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == TypeOfExpression
                || objectType.IsSubclassOf(TypeOfExpression);
        }

        public override void WriteJson(
            JsonWriter writer, object value, JsonSerializer serializer)
        {
            Serializer.Serialize(writer, serializer, (Expression) value);
        }

        public override object ReadJson(
            JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            return Deserializer.Deserialize(_queryHandler, JToken.ReadFrom(reader));
        }

    }
}
