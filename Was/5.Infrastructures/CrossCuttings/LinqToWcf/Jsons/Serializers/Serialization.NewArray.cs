using System.Linq.Expressions;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Serializer
    {
        private bool NewArrayExpression(Expression expr)
        {
            var expression = expr as NewArrayExpression;
            if (expression == null) { return false; }

            Prop("typeName", "newArray");
            Prop("elementType", Type(expression.Type.GetElementType()));
            Prop("expressions", Enumerable(expression.Expressions, Expression));

            return true;
        }
    }
}
