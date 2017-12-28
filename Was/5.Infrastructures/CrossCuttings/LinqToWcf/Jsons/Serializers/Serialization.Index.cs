using System.Linq.Expressions;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Serializer
    {
        private bool IndexExpression(Expression expr)
        {
            var expression = expr as IndexExpression;
            if (expression == null) { return false; }

            Prop("typeName", "index");
            Prop("object", Expression(expression.Object));
            Prop("indexer", Property(expression.Indexer));
            Prop("arguments", Enumerable(expression.Arguments, Expression));

            return true;
        }
    }
}
