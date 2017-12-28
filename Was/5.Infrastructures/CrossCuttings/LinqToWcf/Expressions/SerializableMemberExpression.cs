﻿using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers;
using Kengic.Was.CrossCutting.LinqToWcf.Types;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions
{
    /// <summary>
    ///     A serializable version of <see cref="MemberExpression" />.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class SerializableMemberExpression : SerializableExpression
    {
        /// <summary>
        ///     Default constructor for serialization.
        /// </summary>
        public SerializableMemberExpression()
        {
        }

        /// <summary>
        ///     Constructor with an <see cref="MemberExpression" /> and an <see cref="ExpressionConverter" />.
        /// </summary>
        /// <param name="expression">The original, not serializable <see cref="Expression" />.</param>
        /// <param name="expConverter">
        ///     The <see cref="ExpressionConverter" /> to convert contained
        ///     <see cref="Expression">Expressions</see>.
        /// </param>
        public SerializableMemberExpression(MemberExpression expression, ExpressionConverter expConverter)
            : base(expression, expConverter)
        {
            Expression = expression.Expression.MakeSerializable(expConverter);
            Member = InterLinqTypeSystem.Instance.GetInterLinqMemberInfo(expression.Member);
        }

        /// <summary>
        ///     See <see cref="MemberExpression.Expression" />
        /// </summary>
        [DataMember]
        public SerializableExpression Expression { get; set; }

        /// <summary>
        ///     See <see cref="MemberExpression.Member" />
        /// </summary>
        [DataMember]
        public InterLinqMemberInfo Member { get; set; }

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
            if (Expression != null)
            {
                Expression.BuildString(builder);
            }
            else
            {
                builder.Append(Member.DeclaringType.Name);
            }
            builder.Append(".");
            builder.Append(Member.Name);
        }
    }
}