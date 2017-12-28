﻿using System.Linq.Expressions;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Serializer
    {
        private bool InvocationExpression(Expression expr)
        {
            var expression = expr as InvocationExpression;
            if (expression == null) { return false; }

            Prop("typeName", "invocation");
            Prop("expression", Expression(expression.Expression));
            Prop("arguments", Enumerable(expression.Arguments, Expression));

            return true;
        }
    }
}
