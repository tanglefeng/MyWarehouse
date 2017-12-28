using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions
{
    /// <summary>
    ///     A serializable version of <see cref="ParameterExpression" />.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class SerializableParameterExpression : SerializableExpression
    {
        /// <summary>
        ///     Default constructor for serialization.
        /// </summary>
        public SerializableParameterExpression()
        {
        }

        /// <summary>
        ///     Constructor with an <see cref="ParameterExpression" /> and an <see cref="ExpressionConverter" />.
        /// </summary>
        /// <param name="expression">The original, not serializable <see cref="Expression" />.</param>
        /// <param name="expConverter">
        ///     The <see cref="ExpressionConverter" /> to convert contained
        ///     <see cref="Expression">Expressions</see>.
        /// </param>
        public SerializableParameterExpression(ParameterExpression expression, ExpressionConverter expConverter)
            : base(expression, expConverter)
        {
            Name = expression.Name;
        }

        /// <summary>
        ///     See <see cref="ParameterExpression.Name" />
        /// </summary>
        [DataMember]
        public string Name { get; set; }

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
            if (Name != null)
            {
                builder.Append(Name);
            }
            else
            {
                builder.Append("<param>");
            }
        }
    }
}