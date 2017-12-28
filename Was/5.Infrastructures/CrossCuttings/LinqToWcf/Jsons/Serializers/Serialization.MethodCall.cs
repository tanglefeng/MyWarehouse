using System.Linq.Expressions;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Serializer
    {
        private bool MethodCallExpression(Expression expr)
        {
            var expression = expr as MethodCallExpression;
            if (expression == null) { return false; }

            Prop("typeName", "methodCall");
            Prop("object", Expression(expression.Object));
            Prop("method", Method(expression.Method));
            Prop("arguments", Enumerable(expression.Arguments, Expression));

            return true;
        }
    }
}
