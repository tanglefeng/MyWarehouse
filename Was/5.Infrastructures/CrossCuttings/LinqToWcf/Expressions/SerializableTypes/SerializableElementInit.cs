using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions.SerializableTypes
{
    /// <summary>
    ///     A serializable version of <see cref="ElementInit" />.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class SerializableElementInit
    {
        /// <summary>
        ///     Default constructor for serialization.
        /// </summary>
        public SerializableElementInit()
        {
        }

        /// <summary>
        ///     Constructor with an <see cref="ElementInit" /> and an <see cref="ExpressionConverter" />.
        /// </summary>
        /// <param name="elementInit">The original, not serializable <see cref="ElementInit" />.</param>
        /// <param name="expConverter">
        ///     The <see cref="ExpressionConverter" /> to convert contained
        ///     <see cref="Expression">Expressions</see>.
        /// </param>
        public SerializableElementInit(ElementInit elementInit, ExpressionConverter expConverter)
        {
            Arguments = elementInit.Arguments.MakeSerializableCollection<SerializableExpression>(expConverter);
            AddMethod = elementInit.AddMethod;
        }

        /// <summary>
        ///     See <see cref="ElementInit.Arguments" />
        /// </summary>
        [DataMember]
        public Collection<SerializableExpression> Arguments { get; set; }

        /// <summary>
        ///     See <see cref="ElementInit.AddMethod" />
        /// </summary>
        [DataMember]
        public MethodInfo AddMethod { get; set; }

        /// <summary>
        ///     Builds a <see langword="string" /> representing the <see cref="ElementInit" />.
        /// </summary>
        /// <param name="builder">A <see cref="System.Text.StringBuilder" /> to add the created <see langword="string" />.</param>
        internal void BuildString(StringBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.Append(AddMethod);
            builder.Append("(");
            var flag = true;
            foreach (var expression in Arguments)
            {
                if (flag)
                {
                    flag = false;
                }
                else
                {
                    builder.Append(",");
                }
                expression.BuildString(builder);
            }
            builder.Append(")");
        }

        /// <summary>
        ///     Returns a <see langword="string" /> representing the <see cref="ElementInit" />.
        /// </summary>
        /// <returns>Returns a <see langword="string" /> representing this object.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            BuildString(builder);
            return builder.ToString();
        }
    }
}