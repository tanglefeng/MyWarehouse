using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Deserializer
    {
        private DefaultExpression DefaultExpression(
            ExpressionType nodeType, Type type, JObject obj)
        {
            switch (nodeType) {
                case ExpressionType.Default:
                    return Expr.Default(type);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
