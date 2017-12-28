using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security;
using Kengic.Was.CrossCutting.LinqToWcf.Communications;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.SerializableTypes;
using Kengic.Was.CrossCutting.LinqToWcf.QueryProvider;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers
{
    /// <summary>
    ///     Converter class to convert <see cref="SerializableExpression">SerializableExpression's</see>
    ///     to <see cref="Expression">Expression's</see>.
    /// </summary>
    public class SerializableExpressionConverter : SerializableExpressionVisitor
    {
        /// <summary>
        ///     Initializes this class.
        /// </summary>
        /// <param name="expression"><see cref="SerializableExpression" /> to convert.</param>
        /// <param name="queryHandler"><see cref="IQueryHandler" />.</param>
        public SerializableExpressionConverter(SerializableExpression expression, IQueryHandler queryHandler)
            : base(expression)
        {
            if (queryHandler == null)
            {
                throw new ArgumentNullException(nameof(queryHandler));
            }
            QueryHandler = queryHandler;
        }

        /// <summary>
        ///     Gets the <see cref="IQueryHandler">QueryHandler</see>.
        /// </summary>
        public IQueryHandler QueryHandler { get; }

        /// <summary>
        ///     Visits a <see cref="SerializableBinaryExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableBinaryExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected override Expression VisitSerializableBinaryExpression(SerializableBinaryExpression expression)
        {
            var left = Visit(expression.Left);
            var right = Visit(expression.Right);
            var conversion = (LambdaExpression) Visit(expression.Conversion);
            var method = expression.Method != null ? (MethodInfo) expression.Method.GetClrVersion() : null;
            return Expression.MakeBinary(expression.NodeType, left, right, expression.IsLiftedToNull, method, conversion);
        }

        /// <summary>
        ///     Visits a <see cref="SerializableConditionalExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableConditionalExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected override Expression VisitSerializableConditionalExpression(
            SerializableConditionalExpression expression)
            => Expression.Condition(Visit(expression.Test), Visit(expression.IfTrue), Visit(expression.IfFalse));

        /// <summary>
        ///     Visits a <see cref="SerializableConstantExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableConstantExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected override Expression VisitSerializableConstantExpression(SerializableConstantExpression expression)
        {
            if (expression.Value != null)
            {
                var t = expression.Value.GetType();
                if (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof (InterLinqQuery<>)))

                {
                    var qry = expression.Value as InterLinqQueryBase;
                    var newQry = QueryHandler.Get(qry.ElementType);
                    return Expression.Constant(newQry);
                }
                return Expression.Constant(expression.Value, expression.Type.GetClrVersion() as Type);
            }
            return Expression.Constant(null, (Type) expression.Type.GetClrVersion());
        }

        /// <summary>
        ///     Visits a <see cref="SerializableInvocationExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableInvocationExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected override Expression VisitSerializableInvocationExpression(SerializableInvocationExpression expression)
            => Expression.Invoke(Visit(expression.Expression),
                VisitCollection<Expression>(expression.Arguments).ToArray());

        /// <summary>
        ///     Visits a <see cref="SerializableExpressionTyped" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableExpressionTyped" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected override Expression VisitSerializableExpressionTyped<T>(SerializableExpressionTyped expression)
        {
            var body = Visit(expression.Body);
            var parameters = VisitCollection<ParameterExpression>(expression.Parameters);
            return Expression.Lambda<T>(body, parameters);
        }

        /// <summary>
        ///     Visits a <see cref="SerializableLambdaExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableLambdaExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected override Expression VisitSerializableLambdaExpression(SerializableLambdaExpression expression)
            => Expression.Lambda(Visit(expression.Body),
                VisitCollection<ParameterExpression>(expression.Parameters).ToArray());

        /// <summary>
        ///     Visits a <see cref="SerializableListInitExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableListInitExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected override Expression VisitSerializableListInitExpression(SerializableListInitExpression expression)
            => Expression.ListInit(Visit(expression.NewExpression) as NewExpression,
                VisitObjectCollection<ElementInit>(expression.Initializers));

        /// <summary>
        ///     Visits a <see cref="SerializableMemberExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableMemberExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected override Expression VisitSerializableMemberExpression(SerializableMemberExpression expression)
            => Expression.MakeMemberAccess(Visit(expression.Expression), expression.Member.GetClrVersion());

        /// <summary>
        ///     Visits a <see cref="SerializableMemberInitExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableMemberInitExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected override Expression VisitSerializableMemberInitExpression(SerializableMemberInitExpression expression)
        {
            var bindings = VisitObjectCollection<MemberBinding>(expression.Bindings);
            return Expression.MemberInit(Visit(expression.NewExpression) as NewExpression, bindings);
        }

        /// <summary>
        ///     Visits a <see cref="SerializableMethodCallExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableMethodCallExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected override Expression VisitSerializableMethodCallExpression(SerializableMethodCallExpression expression)
            => Expression.Call(Visit(expression.Object), (MethodInfo) expression.Method.GetClrVersion(),
                VisitCollection<Expression>(expression.Arguments));

        /// <summary>
        ///     Visits a <see cref="SerializableNewArrayExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableNewArrayExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected override Expression VisitSerializableNewArrayExpression(SerializableNewArrayExpression expression)
        {
            if (expression.NodeType == ExpressionType.NewArrayBounds)
            {
                return Expression.NewArrayBounds((Type) expression.Type.GetClrVersion(),
                    VisitCollection<Expression>(expression.Expressions));
            }
            var t = (Type) expression.Type.GetClrVersion();

            // Expression must be an Array
            Debug.Assert(t.HasElementType);

            return Expression.NewArrayInit(t.GetElementType(), VisitCollection<Expression>(expression.Expressions));
        }

        /// <summary>
        ///     Visits a <see cref="SerializableNewExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableNewExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected override Expression VisitSerializableNewExpression(SerializableNewExpression expression)
        {
            if (expression.Members == null)
            {
                return Expression.New((ConstructorInfo) expression.Constructor.GetClrVersion(),
                    VisitCollection<Expression>(expression.Arguments));
            }
            return Expression.New((ConstructorInfo) expression.Constructor.GetClrVersion(),
                VisitCollection<Expression>(expression.Arguments), expression.Members.Select(m => m.GetClrVersion()));
        }

        /// <summary>
        ///     Visits a <see cref="SerializableParameterExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableParameterExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected override Expression VisitSerializableParameterExpression(SerializableParameterExpression expression)
        {
            return Expression.Parameter((Type) expression.Type.GetClrVersion(), expression.Name);
        }

        /// <summary>
        ///     Visits a <see cref="SerializableTypeBinaryExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableTypeBinaryExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected override Expression VisitSerializableTypeBinaryExpression(SerializableTypeBinaryExpression expression)
        {
            return Expression.TypeIs(Visit(expression.Expression), (Type) expression.TypeOperand.GetClrVersion());
        }

        /// <summary>
        ///     Visits a <see cref="SerializableUnaryExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableUnaryExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected override Expression VisitSerializableUnaryExpression(SerializableUnaryExpression expression)
        {
            var operand = Visit(expression.Operand);
            var type = (Type) expression.Type.GetClrVersion();
            var method = expression.Method != null ? (MethodInfo) expression.Method.GetClrVersion() : null;
            return Expression.MakeUnary(expression.NodeType, operand, type, method);
        }

        /// <summary>
        ///     Visits a <see cref="SerializableExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected override Expression VisitUnknownSerializableExpression(SerializableExpression expression)
        {
            throw new Exception($"Expression \"{expression}\" could not be handled.");
        }

        /// <summary>
        ///     Visits a <see cref="SerializableElementInit" />.
        /// </summary>
        /// <param name="elementInit"><see cref="SerializableElementInit" /> to visit.</param>
        /// <returns>Returns the converted <see langword="object" />.</returns>
        protected override object VisitSerializableElementInit(SerializableElementInit elementInit)
            => Expression.ElementInit(elementInit.AddMethod,
                VisitCollection<Expression>(elementInit.Arguments).ToArray());

        /// <summary>
        ///     Visits a <see cref="SerializableMemberAssignment" />.
        /// </summary>
        /// <param name="memberAssignment"><see cref="SerializableMemberAssignment" /> to visit.</param>
        /// <returns>Returns the converted <see langword="object" />.</returns>
        protected override object VisitSerializableMemberAssignment(SerializableMemberAssignment memberAssignment)
            => Expression.Bind(memberAssignment.Member, Visit(memberAssignment.Expression));

        /// <summary>
        ///     Visits a <see cref="SerializableMemberListBinding" />.
        /// </summary>
        /// <param name="memberListBinding"><see cref="SerializableMemberListBinding" /> to visit.</param>
        /// <returns>Returns the converted <see langword="object" />.</returns>
        protected override object VisitSerializableMemberListBinding(SerializableMemberListBinding memberListBinding)
            => Expression.ListBind(memberListBinding.Member,
                VisitObjectCollection<ElementInit>(memberListBinding.Initializers));

        /// <summary>
        ///     Visits a <see cref="SerializableMemberMemberBinding" />.
        /// </summary>
        /// <param name="memberMemberBinding"><see cref="SerializableMemberMemberBinding" /> to visit.</param>
        /// <returns>Returns the converted <see langword="object" />.</returns>
        protected override object VisitSerializableMemberMemberBinding(
            SerializableMemberMemberBinding memberMemberBinding) => Expression.MemberBind(memberMemberBinding.Member,
                VisitObjectCollection<MemberBinding>(memberMemberBinding.Bindings));

        /// <summary>
        ///     Executes a <see cref="SerializableConstantExpression" /> and returns the result.
        /// </summary>
        /// <param name="expression"><see cref="SerializableConstantExpression" /> to convert.</param>
        /// <returns>Returns the result of a <see cref="SerializableConstantExpression" />.</returns>
        protected override object GetResultConstantExpression(SerializableConstantExpression expression)
        {
            if (expression.Value == null)
            {
                return null;
            }
            if (expression.Value is InterLinqQueryBase)
            {
                var baseQuery = (InterLinqQueryBase) expression.Value;

                var type = ((InterLinqQueryBase) expression.Value).ElementType;

                return QueryHandler.Get(type);
                //return QueryHandler.Get(type, baseQuery.AdditionalObject, baseQuery.QueryName, (!string.IsNullOrEmpty(baseQuery.QueryName) && baseQuery.QueryParameters != null && baseQuery.QueryParameters.Count != 0) ? baseQuery.QueryParameters.Select(s => VisitResult(s)).ToArray() : null);
            }
            return expression.Value;
        }

        /// <summary>
        ///     Executes a <see cref="SerializableMethodCallExpression" /> and returns the result.
        /// </summary>
        /// <param name="expression"><see cref="SerializableMethodCallExpression" /> to convert.</param>
        /// <returns>Returns the result of a <see cref="SerializableMethodCallExpression" />.</returns>
        protected override object GetResultMethodCallExpression(SerializableMethodCallExpression expression)
            => InvokeMethodCall(expression);

        /// <summary>
        ///     Returns the return value of the method call in <paramref name="ex" />.
        /// </summary>
        /// <param name="ex"><see cref="SerializableMethodCallExpression" /> to invoke.</param>
        /// <returns>Returns the return value of the method call in <paramref name="ex" />.</returns>
        protected object InvokeMethodCall(SerializableMethodCallExpression ex)
        {
            if (ex.Method.DeclaringType.GetClrVersion() == typeof (Queryable))

            {
                var args = new List<object>();
                var parameterTypes = ex.Method.ParameterTypes.Select(p => (Type) p.GetClrVersion()).ToArray();

                for (var i = 0; (i < ex.Arguments.Count) && (i < parameterTypes.Length); i++)
                {
                    var currentArg = ex.Arguments[i];
                    var currentParameterType = parameterTypes[i];
                    if (typeof (Expression).IsAssignableFrom(currentParameterType))
                    {
                        args.Add(((UnaryExpression) Visit(currentArg)).Operand);
                    }
                    else
                    {
                        args.Add(VisitResult(currentArg));
                    }
                }

                var ret = ((MethodInfo) ex.Method.GetClrVersion()).Invoke(ex.Object, args.ToArray());


                return ret;
            }


            // If the method is not of DeclaringType "Queryable", it mustn't be invoked.
            // Without this check, we were able to delete files from the server disk
            // using System.IO.File.Delete( ... )!
            throw new SecurityException(
                $"Could not call method '{ex.Method.Name}' of type '{ex.Method.DeclaringType.Name}'. Type must be Queryable.");
        }
    }
}