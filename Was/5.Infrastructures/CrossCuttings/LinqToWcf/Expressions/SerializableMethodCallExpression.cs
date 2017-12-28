﻿using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers;
using Kengic.Was.CrossCutting.LinqToWcf.Types;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions
{
    /// <summary>
    ///     A serializable version of <see cref="MethodCallExpression" />.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class SerializableMethodCallExpression : SerializableExpression
    {
        /// <summary>
        ///     Default constructor for serialization.
        /// </summary>
        public SerializableMethodCallExpression()
        {
        }

        /// <summary>
        ///     Constructor with an <see cref="MethodCallExpression" /> and an <see cref="ExpressionConverter" />.
        /// </summary>
        /// <param name="expression">The original, not serializable <see cref="Expression" />.</param>
        /// <param name="expConverter">
        ///     The <see cref="ExpressionConverter" /> to convert contained
        ///     <see cref="Expression">Expressions</see>.
        /// </param>
        public SerializableMethodCallExpression(MethodCallExpression expression, ExpressionConverter expConverter)
            : base(expression, expConverter)
        {
            Arguments = expression.Arguments.MakeSerializableCollection<SerializableExpression>(expConverter);
            Object = expression.Object.MakeSerializable(expConverter);
            Method = InterLinqTypeSystem.Instance.GetInterLinqVersionOf<InterLinqMethodInfo>(expression.Method);
        }

        /// <summary>
        ///     See <see cref="MethodCallExpression.Object" />
        /// </summary>
        [DataMember]
        public SerializableExpression Object { get; set; }

        /// <summary>
        ///     See <see cref="MethodCallExpression.Arguments" />
        /// </summary>
        [DataMember]
        public Collection<SerializableExpression> Arguments { get; set; }

        /// <summary>
        ///     See <see cref="MethodCallExpression.Method" />
        /// </summary>
        [DataMember]
        public InterLinqMethodInfo Method { get; set; }

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
            const int num = 0;
            var expression = Object;
            if (expression != null)
            {
                expression.BuildString(builder);
                builder.Append(".");
            }
            builder.Append(Method.Name);
            builder.Append("(");
            var num2 = num;
            var count = Arguments.Count;
            while (num2 < count)
            {
                if (num2 > num)
                {
                    builder.Append(", ");
                }
                Arguments[num2].BuildString(builder);
                num2++;
            }
            builder.Append(")");
        }
    }
}