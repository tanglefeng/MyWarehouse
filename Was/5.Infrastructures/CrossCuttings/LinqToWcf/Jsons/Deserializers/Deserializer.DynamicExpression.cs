using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Deserializer
    {
        private DynamicExpression DynamicExpression(
            ExpressionType nodeType, Type type, JObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
