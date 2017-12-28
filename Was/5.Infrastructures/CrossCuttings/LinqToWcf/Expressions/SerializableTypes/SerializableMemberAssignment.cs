using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions.SerializableTypes
{
    /// <summary>
    ///     A serializable version of <see cref="MemberAssignment" />.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class SerializableMemberAssignment : SerializableMemberBinding
    {
        /// <summary>
        ///     Default constructor for serialization.
        /// </summary>
        public SerializableMemberAssignment()
        {
        }

        /// <summary>
        ///     Constructor with an <see cref="MemberAssignment" /> and an <see cref="ExpressionConverter" />.
        /// </summary>
        /// <param name="memberAssignment">The original, not serializable <see cref="MemberBinding" />.</param>
        /// <param name="expConverter">
        ///     The <see cref="ExpressionConverter" /> to convert contained
        ///     <see cref="Expression">Expressions</see>.
        /// </param>
        public SerializableMemberAssignment(MemberAssignment memberAssignment, ExpressionConverter expConverter)
            : base(memberAssignment, expConverter)
        {
            Expression = memberAssignment.Expression.MakeSerializable(expConverter);
        }

        /// <summary>
        ///     See <see cref="MemberAssignment.Expression" />
        /// </summary>
        [DataMember]
        public SerializableExpression Expression { get; set; }

        /// <summary>
        ///     Builds a <see langword="string" /> representing the <see cref="MemberBinding" />.
        /// </summary>
        /// <param name="builder">A <see cref="System.Text.StringBuilder" /> to add the created <see langword="string" />.</param>
        internal override void BuildString(StringBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.Append(Member.Name);
            builder.Append(" = ");
            Expression.BuildString(builder);
        }
    }
}