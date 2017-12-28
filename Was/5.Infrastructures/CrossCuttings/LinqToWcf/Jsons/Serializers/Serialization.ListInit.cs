using System;
using System.Linq.Expressions;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Serializer
    {
        private bool ListInitExpression(Expression expr)
        {
            var expression = expr as DefaultExpression;
            if (expression == null) { return false; }

            throw new NotImplementedException();
        }
    }
}
