using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions.SerializableTypes
{
    /// <summary>
    ///     A serializable version of <see cref="MemberBinding" />.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    [KnownType(typeof (SerializableMemberMemberBinding))]
    [KnownType(typeof (SerializableMemberAssignment))]
    [KnownType(typeof (SerializableMemberListBinding))]
    public abstract class SerializableMemberBinding
    {
        /// <summary>
        ///     Default constructor for serialization.
        /// </summary>
        protected SerializableMemberBinding()
        {
        }

        /// <summary>
        ///     Constructor with an <see cref="MemberBinding" /> and an <see cref="ExpressionConverter" />.
        /// </summary>
        /// <param name="memberBinding">The original, not serializable <see cref="MemberBinding" />.</param>
        /// <param name="expConverter">
        ///     The <see cref="ExpressionConverter" /> to convert contained
        ///     <see cref="Expression">Expressions</see>.
        /// </param>
        protected SerializableMemberBinding(MemberBinding memberBinding, ExpressionConverter expConverter)
        {
            Member = memberBinding.Member;
            BindingType = memberBinding.BindingType;
        }

        /// <summary>
        ///     See <see cref="MemberBinding.Member" />
        /// </summary>
        [DataMember]
        public MemberInfo Member { get; set; }

        /// <summary>
        ///     See <see cref="MemberBinding.BindingType" />
        /// </summary>
        [DataMember]
        public MemberBindingType BindingType { get; set; }

        /// <summary>
        ///     Builds a <see langword="string" /> representing the <see cref="MemberBinding" />.
        /// </summary>
        /// <param name="builder">A <see cref="System.Text.StringBuilder" /> to add the created <see langword="string" />.</param>
        internal abstract void BuildString(StringBuilder builder);

        /// <summary>
        ///     Returns a <see langword="string" /> representing the <see cref="MemberBinding" />.
        /// </summary>
        /// <returns>Returns a <see langword="string" /> representing the <see cref="MemberBinding" />.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            BuildString(builder);
            return builder.ToString();
        }
    }
}