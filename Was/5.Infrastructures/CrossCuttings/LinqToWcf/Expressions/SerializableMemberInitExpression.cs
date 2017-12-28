using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.SerializableTypes;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions
{
    /// <summary>
    ///     A serializable version of <see cref="MemberInitExpression" />.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class SerializableMemberInitExpression : SerializableExpression
    {
        /// <summary>
        ///     Default constructor for serialization.
        /// </summary>
        public SerializableMemberInitExpression()
        {
        }

        /// <summary>
        ///     Constructor with an <see cref="MemberInitExpression" /> and an <see cref="ExpressionConverter" />.
        /// </summary>
        /// <param name="expression">The original, not serializable <see cref="Expression" />.</param>
        /// <param name="expConverter">
        ///     The <see cref="ExpressionConverter" /> to convert contained
        ///     <see cref="Expression">Expressions</see>.
        /// </param>
        public SerializableMemberInitExpression(MemberInitExpression expression, ExpressionConverter expConverter)
            : base(expression, expConverter)
        {
            NewExpression = expression.NewExpression.MakeSerializable<SerializableNewExpression>(expConverter);
            Bindings = expConverter.ConvertToSerializableObjectCollection<SerializableMemberBinding>(expression.Bindings);
        }

        /// <summary>
        ///     See <see cref="MemberInitExpression.NewExpression" />
        /// </summary>
        [DataMember]
        public SerializableNewExpression NewExpression { get; set; }

        /// <summary>
        ///     See <see cref="MemberInitExpression.Bindings" />
        /// </summary>
        [DataMember]
        public Collection<SerializableMemberBinding> Bindings { get; set; }

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
            if ((NewExpression.Arguments.Count == 0) && NewExpression.Type.Name.Contains("<"))
            {
                builder.Append("new");
            }
            else
            {
                NewExpression.BuildString(builder);
            }
            builder.Append(" {");
            var num = 0;
            var count = Bindings.Count;
            while (num < count)
            {
                var binding = Bindings[num];
                if (num > 0)
                {
                    builder.Append(", ");
                }
                binding.BuildString(builder);
                num++;
            }
            builder.Append("}");
        }
    }
}