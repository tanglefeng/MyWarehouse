using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions.SerializableTypes
{
    /// <summary>
    ///     A serializable version of <see cref="MemberMemberBinding" />.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class SerializableMemberMemberBinding : SerializableMemberBinding
    {
        /// <summary>
        ///     Default constructor for serialization.
        /// </summary>
        public SerializableMemberMemberBinding()
        {
        }

        /// <summary>
        ///     Constructor with an <see cref="MemberMemberBinding" /> and an <see cref="ExpressionConverter" />.
        /// </summary>
        /// <param name="memberMemberBinding">The original, not serializable <see cref="MemberBinding" />.</param>
        /// <param name="expConverter">
        ///     The <see cref="ExpressionConverter" /> to convert contained
        ///     <see cref="Expression">Expressions</see>.
        /// </param>
        public SerializableMemberMemberBinding(MemberMemberBinding memberMemberBinding, ExpressionConverter expConverter)
            : base(memberMemberBinding, expConverter)
        {
            Bindings =
                expConverter.ConvertToSerializableObjectCollection<SerializableMemberBinding>(
                    memberMemberBinding.Bindings);
        }

        /// <summary>
        ///     See <see cref="MemberMemberBinding.Bindings" />
        /// </summary>
        [DataMember]
        public Collection<SerializableMemberBinding> Bindings { get; set; }

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
            builder.Append(" = {");
            var num = 0;
            var count = Bindings.Count;
            while (num < count)
            {
                if (num > 0)
                {
                    builder.Append(", ");
                }
                Bindings[num].BuildString(builder);
                num++;
            }
            builder.Append("}");
        }
    }
}