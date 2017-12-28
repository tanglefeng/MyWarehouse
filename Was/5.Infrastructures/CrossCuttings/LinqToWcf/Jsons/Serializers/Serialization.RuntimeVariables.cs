using System.Linq.Expressions;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Serializer
    {
        private bool RuntimeVariablesExpression(Expression expr)
        {
            var expression = expr as RuntimeVariablesExpression;
            if (expression == null) { return false; }

            Prop("typeName", "runtimeVariables");
            Prop("variables", Enumerable(expression.Variables, Expression));

            return true;
        }
    }
}
