using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Serializer
    {
        private readonly Dictionary<ParameterExpression, string>
            _parameterExpressions = new Dictionary<ParameterExpression, string>();

        private bool ParameterExpression(Expression expr)
        {
            var expression = expr as ParameterExpression;
            if (expression == null) { return false; }

            string name;
            if (!_parameterExpressions.TryGetValue(expression, out name)) {
                name = expression.Name ?? "p_" + Guid.NewGuid().ToString("N");
                _parameterExpressions[expression] = name;
            }

            Prop("typeName", "parameter");
            Prop("name", name);

            return true;
        }
    }
}
