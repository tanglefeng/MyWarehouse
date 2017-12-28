using System.Linq.Expressions;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Serializer
    {
        private bool BlockExpression(Expression expr)
        {
            var expression = expr as BlockExpression;
            if (expression == null) { return false; }

            Prop("typeName", "block");
            Prop("expressions", Enumerable(expression.Expressions, Expression));
            Prop("variables", Enumerable(expression.Variables, Expression));

            return true;
        }
    }
}
