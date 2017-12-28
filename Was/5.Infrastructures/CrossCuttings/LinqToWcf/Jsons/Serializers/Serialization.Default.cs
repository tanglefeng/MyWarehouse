using System.Linq.Expressions;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Serializer
    {
        private bool DefaultExpression(Expression expr)
        {
            var expression = expr as DefaultExpression;
            if (expression == null) { return false; }

            Prop("typeName", "default");

            return true;
        }
    }
}
