using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.SerializableTypes;
using Kengic.Was.CrossCutting.LinqToWcf.QueryProvider;
using Kengic.Was.CrossCutting.LinqToWcf.Types;
using Kengic.Was.CrossCutting.LinqToWcf.Types.Anonymous;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions
{
    /// <summary>
    ///     A serializable version of <see cref="Expression" />.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    [KnownType(typeof (SerializableBinaryExpression))]
    [KnownType(typeof (SerializableConditionalExpression))]
    [KnownType(typeof (SerializableConstantExpression))]
    [KnownType(typeof (SerializableExpression))]
    [KnownType(typeof (SerializableExpressionTyped))]
    [KnownType(typeof (SerializableInvocationExpression))]
    [KnownType(typeof (SerializableLambdaExpression))]
    [KnownType(typeof (SerializableListInitExpression))]
    [KnownType(typeof (SerializableMemberExpression))]
    [KnownType(typeof (SerializableMemberInitExpression))]
    [KnownType(typeof (SerializableMethodCallExpression))]
    [KnownType(typeof (SerializableNewArrayExpression))]
    [KnownType(typeof (SerializableNewExpression))]
    [KnownType(typeof (SerializableParameterExpression))]
    [KnownType(typeof (SerializableTypeBinaryExpression))]
    [KnownType(typeof (SerializableElementInit))]
    [KnownType(typeof (SerializableMemberAssignment))]
    [KnownType(typeof (SerializableMemberBinding))]
    [KnownType(typeof (SerializableMemberListBinding))]
    [KnownType(typeof (SerializableMemberMemberBinding))]
    [KnownType(typeof (SerializableUnaryExpression))]
    [KnownType(typeof (InterLinqQueryBase))]
    [KnownType(typeof (AnonymousMetaProperty))]
    [KnownType(typeof (AnonymousMetaType))]
    [KnownType(typeof (AnonymousObject))]
    [KnownType(typeof (AnonymousProperty))]
    [KnownType(typeof (InterLinqGroupingBase))]
    [KnownType(typeof (InterLinqConstructorInfo))]
    [KnownType(typeof (InterLinqFieldInfo))]
    [KnownType(typeof (InterLinqMemberInfo))]
    [KnownType(typeof (InterLinqMethodBase))]
    [KnownType(typeof (InterLinqMethodInfo))]
    [KnownType(typeof (InterLinqPropertyInfo))]
    [KnownType(typeof (InterLinqType))]
    public abstract class SerializableExpression
    {
        /// <summary>
        ///     Default constructor for serialization.
        /// </summary>
        protected SerializableExpression()
        {
        }

        /// <summary>
        ///     Constructor with an <see cref="Expression" /> and an <see cref="ExpressionConverter" />.
        /// </summary>
        /// <param name="expression">The original, not serializable <see cref="Expression" />.</param>
        /// <param name="expConverter">
        ///     The <see cref="ExpressionConverter" /> to convert contained
        ///     <see cref="Expression">Expressions</see>.
        /// </param>
        protected SerializableExpression(Expression expression, ExpressionConverter expConverter)
            : this(expression.NodeType, expression.Type, expConverter)
        {
            HashCode = expression.GetHashCode();
        }

        /// <summary>
        ///     Constructor with an <see cref="ExpressionType" />, a <see cref="Type" /> and an <see cref="ExpressionConverter" />.
        /// </summary>
        /// <param name="nodeType">The <see cref="ExpressionType" /> of the <see cref="Expression" />.</param>
        /// <param name="type">The <see cref="Type" /> of the <see cref="Expression" />.</param>
        /// <param name="expConverter">
        ///     The <see cref="ExpressionConverter" /> to convert contained
        ///     <see cref="Expression">Expressions</see>.
        /// </param>
        private SerializableExpression(ExpressionType nodeType, Type type, ExpressionConverter expConverter)
            : this()
        {
            NodeType = nodeType;
            Type = InterLinqTypeSystem.Instance.GetInterLinqVersionOf<InterLinqType>(type);
        }

        /// <summary>
        ///     The hashcode of the original <see cref="Expression" />.
        /// </summary>
        [DataMember]
        public int HashCode { get; set; }

        /// <summary>
        ///     See <see cref="Expression.NodeType" />
        /// </summary>
        [DataMember]
        public ExpressionType NodeType { get; set; }

        /// <summary>
        ///     See <see cref="Expression.Type" />
        /// </summary>
        [DataMember]
        public InterLinqType Type { get; set; }

        /// <summary>
        ///     Builds a <see langword="string" /> representing the <see cref="Expression" />.
        /// </summary>
        /// <returns>A <see langword="string" /> representing the <see cref="Expression" />.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            BuildString(sb);
            return sb.ToString();
        }

        /// <summary>
        ///     Builds a <see langword="string" /> representing the <see cref="Expression" />.
        /// </summary>
        /// <param name="builder">A <see cref="System.Text.StringBuilder" /> to add the created <see langword="string" />.</param>
        internal virtual void BuildString(StringBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.Append("[");
            builder.Append(NodeType);
            builder.Append("]");
        }
    }
}