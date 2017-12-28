using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions.SerializableTypes
{
    /// <summary>
    ///     A serializable version of <see cref="MemberListBinding" />.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class SerializableMemberListBinding : SerializableMemberBinding
    {
        /// <summary>
        ///     Default constructor for serialization.
        /// </summary>
        public SerializableMemberListBinding()
        {
        }

        /// <summary>
        ///     Constructor with an <see cref="MemberListBinding" /> and an <see cref="ExpressionConverter" />.
        /// </summary>
        /// <param name="memberListBinding">The original, not serializable <see cref="MemberBinding" />.</param>
        /// <param name="expConverter">
        ///     The <see cref="ExpressionConverter" /> to convert contained
        ///     <see cref="Expression">Expressions</see>.
        /// </param>
        public SerializableMemberListBinding(MemberListBinding memberListBinding, ExpressionConverter expConverter)
            : base(memberListBinding, expConverter)
        {
            Initializers =
                expConverter.ConvertToSerializableObjectCollection<SerializableElementInit>(
                    memberListBinding.Initializers);
        }

        /// <summary>
        ///     See <see cref="MemberListBinding.Initializers" />
        /// </summary>
        [DataMember]
        public Collection<SerializableElementInit> Initializers { get; set; }

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