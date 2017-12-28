using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers;
using Kengic.Was.CrossCutting.LinqToWcf.Types;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions
{
    /// <summary>
    ///     A serializable version of <see cref="LambdaExpression" />.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class SerializableExpressionTyped : SerializableLambdaExpression
    {
        /// <summary>
        ///     Default constructor for serialization.
        /// </summary>
        public SerializableExpressionTyped()
        {
        }

        /// <summary>
        ///     Constructor with an <see cref="LambdaExpression" /> and an <see cref="ExpressionConverter" />.
        /// </summary>
        /// <param name="expression">The original, not serializable <see cref="Expression" />.</param>
        /// <param name="delegateType"><see cref="Type" /> of the delegate.</param>
        /// <param name="expConverter">
        ///     The <see cref="ExpressionConverter" /> to convert contained
        ///     <see cref="Expression">Expressions</see>.
        /// </param>
        public SerializableExpressionTyped(LambdaExpression expression, Type delegateType,
            ExpressionConverter expConverter)
            : base(expression, expConverter)
        {
            Type = InterLinqTypeSystem.Instance.GetInterLinqVersionOf<InterLinqType>(delegateType);
        }
    }
}