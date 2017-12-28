using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Deserializer
    {
        private ConstantExpression ConstantExpression(
            ExpressionType nodeType, Type type, JObject obj)
        {
            object value;

            var valueTok = Prop(obj, "value");
            if (valueTok == null || valueTok.Type == JTokenType.Null) {
                value = null;
            }
            else {
                var valueObj = (JObject) valueTok;
                var valueType = Prop(valueObj, "type", Type);
                if (typeof(IQueryable).IsAssignableFrom(valueType))
                {
                    BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
                   var  getByNameMethod = _queryHandler.GetType().GetMethod("Get", flags, null, new[] { typeof(string)}, null);
                    if (valueType.IsGenericType)
                    {
                        getByNameMethod = getByNameMethod.MakeGenericMethod(valueType.GenericTypeArguments[0]);
                    }
                    value = getByNameMethod.Invoke(_queryHandler, new object[] {string.Empty});
                }
                else
                {
                    value = Deserialize(Prop(valueObj, "value"), valueType);
                }
            }

            switch (nodeType) {
                case ExpressionType.Constant:
                    return Expr.Constant(value);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
