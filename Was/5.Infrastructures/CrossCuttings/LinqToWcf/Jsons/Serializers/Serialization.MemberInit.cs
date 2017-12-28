using System;
using System.Linq.Expressions;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Serializer
    {
        private bool MemberInitExpression(Expression expr)
        {
            var expression = expr as MemberInitExpression;
            if (expression == null) { return false; }

            throw new NotImplementedException();
        }
    }
}
