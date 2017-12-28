using System;
using System.Linq.Expressions;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Serializer
    {
        private bool SwitchExpression(Expression expr)
        {
            var expression = expr as SwitchExpression;
            if (expression == null) { return false; }

            throw new NotImplementedException();
        }
    }
}
