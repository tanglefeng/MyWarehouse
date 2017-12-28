﻿using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.SerializableTypes;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions
{
    /// <summary>
    ///     A serializable version of <see cref="ListInitExpression" />.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class SerializableListInitExpression : SerializableExpression
    {
        /// <summary>
        ///     Default constructor for serialization.
        /// </summary>
        public SerializableListInitExpression()
        {
        }

        /// <summary>
        ///     Constructor with an <see cref="ListInitExpression" /> and an <see cref="ExpressionConverter" />.
        /// </summary>
        /// <param name="expression">The original, not serializable <see cref="Expression" />.</param>
        /// <param name="expConverter">
        ///     The <see cref="ExpressionConverter" /> to convert contained
        ///     <see cref="Expression">Expressions</see>.
        /// </param>
        public SerializableListInitExpression(ListInitExpression expression, ExpressionConverter expConverter)
            : base(expression, expConverter)
        {
            NewExpression = expression.NewExpression.MakeSerializable<SerializableNewExpression>(expConverter);
            Initializers =
                expConverter.ConvertToSerializableObjectCollection<SerializableElementInit>(expression.Initializers);
        }

        /// <summary>
        ///     See <see cref="ListInitExpression.NewExpression" />
        /// </summary>
        [DataMember]
        public SerializableNewExpression NewExpression { get; set; }

        /// <summary>
        ///     See <see cref="ListInitExpression.Initializers" />
        /// </summary>
        [DataMember]
        public Collection<SerializableElementInit> Initializers { get; set; }

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
            NewExpression.BuildString(builder);
            builder.Append(" {");
            var num = 0;
            var count = Initializers.Count;
            while (num < count)
            {
                if (num > 0)
                {
                    builder.Append(", ");
                }
                Initializers[num].BuildString(builder);
                num++;
            }
            builder.Append("}");
        }
    }
}