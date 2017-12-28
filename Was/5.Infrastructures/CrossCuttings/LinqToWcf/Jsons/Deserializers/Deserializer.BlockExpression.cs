using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Deserializer
    {
        private BlockExpression BlockExpression(
            ExpressionType nodeType, Type type, JObject obj)
        {
            var expressions = Prop(obj, "expressions", Enumerable(Expression));
            var variables = Prop(obj, "variables", Enumerable(ParameterExpression));

            switch (nodeType) {
                case ExpressionType.Block:
                    return Expr.Block(type, variables, expressions);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
