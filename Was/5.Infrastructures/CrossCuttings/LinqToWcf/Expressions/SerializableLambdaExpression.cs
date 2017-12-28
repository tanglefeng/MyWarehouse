﻿using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions
{
    /// <summary>
    ///     A serializable version of <see cref="LambdaExpression" />.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    [KnownType(typeof (SerializableExpressionTyped))]
    public class SerializableLambdaExpression : SerializableExpression
    {
        /// <summary>
        ///     Default constructor for serialization.
        /// </summary>
        public SerializableLambdaExpression()
        {
        }

        /// <summary>
        ///     Constructor with an <see cref="LambdaExpression" /> and an <see cref="ExpressionConverter" />.
        /// </summary>
        /// <param name="expression">The original, not serializable <see cref="Expression" />.</param>
        /// <param name="expConverter">
        ///     The <see cref="ExpressionConverter" /> to convert contained
        ///     <see cref="Expression">Expressions</see>.
        /// </param>
        public SerializableLambdaExpression(LambdaExpression expression, ExpressionConverter expConverter)
            : base(expression, expConverter)
        {
            Body = expression.Body.MakeSerializable(expConverter);
            Parameters = expression.Parameters.MakeSerializableCollection<SerializableParameterExpression>(expConverter);
        }

        /// <summary>
        ///     See <see cref="LambdaExpression.Body" />
        /// </summary>
        [DataMember]
        public SerializableExpression Body { get; set; }

        /// <summary>
        ///     See <see cref="LambdaExpression.Parameters" />
        /// </summary>
        [DataMember]
        public Collection<SerializableParameterExpression> Parameters { get; set; }

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
            if (Parameters.Count == 1)
            {
                Parameters[0].BuildString(builder);
            }
            else
            {
                builder.Append("(");
                var num = 0;
                var count = Parameters.Count;
                while (num < count)
                {
                    if (num > 0)
                    {
                        builder.Append(", ");
                    }
                    Parameters[num].BuildString(builder);
                    num++;
                }
                builder.Append(")");
            }
            builder.Append(" => ");
            Body.BuildString(builder);
        }
    }
}