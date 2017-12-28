using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    internal sealed partial class Deserializer
    {
        private IQueryHandler _queryHandler;
        public static Expression Deserialize(IQueryHandler queryHandler, JToken token)
        {
            var d = new Deserializer(queryHandler);
            return d.Expression(token);
        }


        private Deserializer(IQueryHandler queryHandler)
        {
            _queryHandler = queryHandler;
        }

        private object Deserialize(JToken token, Type type)
        {
            return token.ToObject(type);
        }

        private T Prop<T>(JObject obj, string name, Func<JToken, T> result)
        {
            var prop = obj.Property(name);
            return result(prop == null ? null : prop.Value);
        }

        private JToken Prop(JObject obj, string name)
        {
            return obj.Property(name).Value;
        }

        private T Enum<T>(JToken token)
        {
            return (T) System.Enum.Parse(typeof(T), token.Value<string>());
        }

        private Func<JToken, IEnumerable<T>> Enumerable<T>(Func<JToken, T> result)
        {
            return token => {
                if (token == null || token.Type != JTokenType.Array) {
                    return null;
                }
                var array = (JArray) token;
                return array.Select(result);
            };
        }

        private Expression Expression(JToken token)
        {
            if (token == null || token.Type != JTokenType.Object) {
                return null;
            }

            var obj = (JObject) token;
            var nodeType = Prop(obj, "nodeType", Enum<ExpressionType>);
            var type = Prop(obj, "type", Type);
            var typeName = Prop(obj, "typeName", t => t.Value<string>());

            switch (typeName) {
                case "binary":              return BinaryExpression(nodeType, type, obj);
                case "block":               return BlockExpression(nodeType, type, obj);
                case "conditional":         return ConditionalExpression(nodeType, type, obj);
                case "constant":            return ConstantExpression(nodeType, type, obj);
                case "debugInfo":           return DebugInfoExpression(nodeType, type, obj);
                case "default":             return DefaultExpression(nodeType, type, obj);
                case "dynamic":             return DynamicExpression(nodeType, type, obj);
                case "goto":                return GotoExpression(nodeType, type, obj);
                case "index":               return IndexExpression(nodeType, type, obj);
                case "invocation":          return InvocationExpression(nodeType, type, obj);
                case "label":               return LabelExpression(nodeType, type, obj);
                case "lambda":              return LambdaExpression(nodeType, type, obj);
                case "listInit":            return ListInitExpression(nodeType, type, obj);
                case "loop":                return LoopExpression(nodeType, type, obj);
                case "member":              return MemberExpression(nodeType, type, obj);
                case "memberInit":          return MemberInitExpression(nodeType, type, obj);
                case "methodCall":          return MethodCallExpression(nodeType, type, obj);
                case "newArray":            return NewArrayExpression(nodeType, type, obj);
                case "new":                 return NewExpression(nodeType, type, obj);
                case "parameter":           return ParameterExpression(nodeType, type, obj);
                case "runtimeVariables":    return RuntimeVariablesExpression(nodeType, type, obj);
                case "switch":              return SwitchExpression(nodeType, type, obj);
                case "try":                 return TryExpression(nodeType, type, obj);
                case "typeBinary":          return TypeBinaryExpression(nodeType, type, obj);
                case "unary":               return UnaryExpression(nodeType, type, obj);
            }
            throw new NotSupportedException();
        }
    }
}
