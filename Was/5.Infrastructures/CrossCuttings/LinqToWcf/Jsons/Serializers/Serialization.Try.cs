using System;
using System.Linq.Expressions;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Serializer
    {
        private bool TryExpression(Expression expr)
        {
            var expression = expr as TryExpression;
            if (expression == null) { return false; }

            throw new NotImplementedException();
        }
    }
}
