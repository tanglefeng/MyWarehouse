﻿using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Deserializer
    {
        private NewArrayExpression NewArrayExpression(
            ExpressionType nodeType, Type type, JObject obj)
        {
            var elementType = Prop(obj, "elementType", Type);
            var expressions = Prop(obj, "expressions", Enumerable(Expression));

            switch (nodeType) {
                case ExpressionType.NewArrayInit:
                    return Expr.NewArrayInit(elementType, expressions);
                case ExpressionType.NewArrayBounds:
                    return Expr.NewArrayBounds(elementType, expressions);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
