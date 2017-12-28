using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions
{
    /// <summary>
    ///     A serializable version of <see cref="InvocationExpression" />.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class SerializableInvocationExpression : SerializableExpression
    {
        /// <summary>
        ///     Default constructor for serialization.
        /// </summary>
        public SerializableInvocationExpression()
        {
        }

        /// <summary>
        ///     Constructor with an <see cref="InvocationExpression" /> and an <see cref="ExpressionConverter" />.
        /// </summary>
        /// <param name="expression">The original, not serializable <see cref="Expression" />.</param>
        /// <param name="expConverter">
        ///     The <see cref="ExpressionConverter" /> to convert contained
        ///     <see cref="Expression">Expressions</see>.
        /// </param>
        public SerializableInvocationExpression(InvocationExpression expression, ExpressionConverter expConverter)
            : base(expression, expConverter)
        {
            Expression = expression.Expression.MakeSerializable(expConverter);
            Arguments = expression.Arguments.MakeSerializableCollection<SerializableExpression>(expConverter);
        }

        /// <summary>
        ///     See <see cref="InvocationExpression.Expression" />
        /// </summary>
        [DataMember]
        public SerializableExpression Expression { get; set; }

        /// <summary>
        ///     See <see cref="InvocationExpression.Arguments" />
        /// </summary>
        [DataMember]
        public Collection<SerializableExpression> Arguments { get; set; }

        /// <summary>
        ///     Builds a <see langword="string" /> representing the <see cref="Expression" />.
        /// </summary>
        /// <param name="builder">A <see cref="System.Text.StringBuilder" /> to add the created <see langword="string" />.</param>
        internal override void BuildString(StringBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.Append("Invoke(");
            Expression.BuildString(builder);
            var num = 0;
            var count = Arguments.Count;
            while (num < count)
            {
                builder.Append(",");
                Arguments[num].BuildString(builder);
                num++;
            }
            builder.Append(")");
        }
    }
}