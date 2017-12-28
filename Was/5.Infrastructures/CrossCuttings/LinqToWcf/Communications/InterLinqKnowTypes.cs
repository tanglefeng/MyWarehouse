using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.SerializableTypes;
using Kengic.Was.CrossCutting.LinqToWcf.QueryProvider;
using Kengic.Was.CrossCutting.LinqToWcf.Types;
using Kengic.Was.CrossCutting.LinqToWcf.Types.Anonymous;

namespace Kengic.Was.CrossCutting.LinqToWcf.Communications
{
    /// <summary>
    ///     Static Class for keeping all the known types for interlinq's framework.
    /// </summary>
    public static class InterLinqKnowTypes
    {
        private static readonly Type[] KnownTypes;

        static InterLinqKnowTypes()
        {
            var types = new List<Type>
            {
                typeof (SerializableInvocationExpression),
                typeof (SerializableNewArrayExpression),
                typeof (SerializableConstantExpression),
                typeof (SerializableListInitExpression),
                typeof (SerializableNewExpression),
                typeof (SerializableMemberExpression),
                typeof (SerializableMemberInitExpression),
                typeof (SerializableMethodCallExpression),
                typeof (SerializableLambdaExpression),
                typeof (SerializableParameterExpression),
                typeof (SerializableExpressionTyped),
                typeof (SerializableTypeBinaryExpression),
                typeof (SerializableUnaryExpression),
                typeof (SerializableBinaryExpression),
                typeof (SerializableConditionalExpression),
                typeof (AnonymousMetaType),
                typeof (InterLinqType),
                typeof (InterLinqMemberInfo),
                typeof (InterLinqMethodBase),
                typeof (List<InterLinqType>),
                typeof (InterLinqMethodInfo),
                typeof (InterLinqConstructorInfo),
                typeof (InterLinqPropertyInfo),
                typeof (InterLinqFieldInfo),
                typeof (List<InterLinqMemberInfo>),
                typeof (List<AnonymousMetaProperty>),
                typeof (AnonymousMetaProperty),
                typeof (AnonymousObject),
                typeof (List<AnonymousProperty>),
                typeof (AnonymousProperty),
                typeof (List<AnonymousObject>),
                typeof (SerializableExpression),
                typeof (List<SerializableExpression>),
                typeof (List<SerializableParameterExpression>),
                typeof (ExpressionType),
                typeof (MemberBindingType),
                typeof (List<SerializableElementInit>),
                typeof (SerializableElementInit),
                typeof (List<SerializableMemberBinding>),
                typeof (SerializableMemberBinding),
                typeof (MethodInfo),
                typeof (MethodBase),
                typeof (MemberInfo),
                typeof (Exception),
                typeof (object[]),
                typeof (InterLinqQuery<string>)
            };

            KnownTypes = types.ToArray();
        }

        /// <summary>
        ///     Return a list of all known types for the interlinq framework.
        /// </summary>
        /// <param name="provider">The instance of the object that support custom known types.</param>
        /// <returns>A <see cref="IEnumerable{T}" /> of all known <see cref="Type" />.</returns>
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider) => KnownTypes;
    }
}