using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers
{
    /// <summary>
    ///     A visitor for <see cref="Expression">Expressions</see>.
    /// </summary>
    public abstract class ExpressionVisitor
    {
        private Dictionary<int, object> _executedValue;

        /// <summary>
        ///     Creates a new <see cref="ExpressionVisitor" /> with an <see cref="Expression" />.
        /// </summary>
        /// <param name="expression"><see cref="Expression" /> to initialize the visitor.</param>
        protected ExpressionVisitor(Expression expression)
        {
            Expression = expression;
        }

        /// <summary>
        ///     The <see cref="Expression" /> to visit.
        /// </summary>
        public Expression Expression { get; }

        /// <summary>
        ///     Visit the <see cref="Expression" /> in this visitor.
        /// </summary>
        /// <returns>Returns the result of the visit.</returns>
        public object Visit()
        {
            _executedValue = new Dictionary<int, object>();
            return Visit(Expression);
        }

        /// <summary>
        ///     Visits an <see cref="Expression" /> and returns a result of the type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T"><see cref="Type" /> of the returned value.</typeparam>
        /// <param name="expression"><see cref="Expression" /> to visit.</param>
        /// <returns>Returns a result of the type <typeparamref name="T" />.</returns>
        protected T Visit<T>(Expression expression) => (T) Visit(expression);

        /// <summary>
        ///     Visit an <see cref="Expression" />.
        /// </summary>
        /// <param name="expression"><see cref="Expression" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected object Visit(Expression expression)
        {
            if (!VisitPrecondition(expression))
            {
                throw new ArgumentException(
                    $"The expression \"{expression}\" could not be executed because of the precondition.");
            }

            if (expression == null)
            {
                return VisitObjectHandleNull();
            }

            if (_executedValue.ContainsKey(expression.GetHashCode()))
            {
                return _executedValue[expression.GetHashCode()];
            }

            object returnValue;

            if (expression is BinaryExpression)
            {
                returnValue = VisitBinaryExpression(expression as BinaryExpression);
            }
            else if (expression is ConditionalExpression)
            {
                returnValue = VisitConditionalExpression(expression as ConditionalExpression);
            }
            else if (expression is ConstantExpression)
            {
                returnValue = VisitConstantExpression(expression as ConstantExpression);
            }
            else if (expression is InvocationExpression)
            {
                returnValue = VisitInvocationExpression(expression as InvocationExpression);
            }
            else if (expression is LambdaExpression)
            {
                var fromType = expression.GetType();
                if (fromType.IsGenericType)

                {
                    var genericTypes = fromType.GetGenericArguments();
                    var executeMethod = GetType()
                        .GetMethod("VisitTypedExpression", BindingFlags.NonPublic | BindingFlags.Instance);
                    var genericExecuteMethod = executeMethod.MakeGenericMethod(genericTypes);
                    returnValue = genericExecuteMethod.Invoke(this, new object[] {expression});
                }
                else
                {
                    returnValue = VisitLambdaExpression(expression as LambdaExpression);
                }
            }
            else if (expression is ListInitExpression)
            {
                returnValue = VisitListInitExpression(expression as ListInitExpression);
            }
            else if (expression is MemberExpression)
            {
                returnValue = VisitMemberExpression(expression as MemberExpression);
            }
            else if (expression is MemberInitExpression)
            {
                returnValue = VisitMemberInitExpression(expression as MemberInitExpression);
            }
            else if (expression is MethodCallExpression)
            {
                returnValue = VisitMethodCallExpression(expression as MethodCallExpression);
            }
            else if (expression is NewArrayExpression)
            {
                returnValue = VisitNewArrayExpression(expression as NewArrayExpression);
            }
            else if (expression is NewExpression)
            {
                returnValue = VisitNewExpression(expression as NewExpression);
            }
            else if (expression is ParameterExpression)
            {
                returnValue = VisitParameterExpression(expression as ParameterExpression);
            }
            else if (expression is TypeBinaryExpression)
            {
                returnValue = VisitTypeBinaryExpression(expression as TypeBinaryExpression);
            }
            else if (expression is UnaryExpression)
            {
                returnValue = VisitUnaryExpression(expression as UnaryExpression);
            }
            else
            {
                returnValue = VisitUnknownExpression(expression);
            }

            if (!_executedValue.ContainsKey(expression.GetHashCode()))
                _executedValue.Add(expression.GetHashCode(), returnValue);
            return returnValue;
        }

        /// <summary>
        ///     Visit a collection of <see cref="Expression">expressions</see>.
        /// </summary>
        /// <typeparam name="T">Type of the visit results.</typeparam>
        /// <param name="enumerable">Collection to visit.</param>
        /// <returns>Returns a list of visit results.</returns>
        protected IList<T> VisitCollection<T>(IEnumerable enumerable)
        {
            if (enumerable == null)
            {
                return null;
            }
            var returnValues = (from Expression item in enumerable select Visit<T>(item)).ToList();
            return returnValues;
        }

        /// <summary>
        ///     Visits an <see langword="object" /> and returns a result of the type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T"><see cref="Type" /> of the returned value.</typeparam>
        /// <param name="objectToVisit"><see langword="object" /> to visit.</param>
        /// <returns>Returns a result of the type <typeparamref name="T" />.</returns>
        protected T VisitObject<T>(object objectToVisit) => (T) VisitObject(objectToVisit);

        /// <summary>
        ///     Visit an <see langword="object" />.
        /// </summary>
        /// <param name="objectToVisit"><see langword="object" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected object VisitObject(object objectToVisit)
        {
            if (!VisitObjectPrecondition(objectToVisit))
            {
                throw new ArgumentException(
                    $"The object \"{objectToVisit}\" could not be executed because of the precondition.");
            }
            if (objectToVisit == null)
            {
                return VisitObjectHandleNull();
            }
            if (objectToVisit is ElementInit)
            {
                return VisitElementInit((ElementInit) objectToVisit);
            }
            if (objectToVisit is MemberAssignment)
            {
                return VisitMemberAssignment((MemberAssignment) objectToVisit);
            }
            if (objectToVisit is MemberListBinding)
            {
                return VisitMemberListBinding((MemberListBinding) objectToVisit);
            }
            if (objectToVisit is MemberMemberBinding)
            {
                return VisitMemberMemberBinding((MemberMemberBinding) objectToVisit);
            }
            return VisitUnkownObject(objectToVisit);
        }

        /// <summary>
        ///     Visit a collection of <see langword="object">objects</see>.
        /// </summary>
        /// <typeparam name="T">Type of the visit results.</typeparam>
        /// <param name="enumerable">Collection to visit.</param>
        /// <returns>Returns a list of visit results.</returns>
        protected IList<T> VisitObjectCollection<T>(IEnumerable enumerable)
        {
            if (enumerable == null)
            {
                return null;
            }
            var returnValues = (from object item in enumerable select VisitObject<T>(item)).ToList();
            return returnValues;
        }

        /// <summary>
        ///     Visit a <see cref="BinaryExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="BinaryExpression" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitBinaryExpression(BinaryExpression expression);

        /// <summary>
        ///     Visit a <see cref="ConditionalExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="ConditionalExpression" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitConditionalExpression(ConditionalExpression expression);

        /// <summary>
        ///     Visit a <see cref="ConstantExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="ConstantExpression" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitConstantExpression(ConstantExpression expression);

        /// <summary>
        ///     Visit a <see cref="System.Linq.Expressions.Expression{T}" />.
        /// </summary>
        /// <param name="expression"><see cref="System.Linq.Expressions.Expression{T}" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitTypedExpression<T>(Expression<T> expression);


        /// <summary>
        ///     Visit a <see cref="InvocationExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="InvocationExpression" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitInvocationExpression(InvocationExpression expression);

        /// <summary>
        ///     Visit a <see cref="LambdaExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="LambdaExpression" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitLambdaExpression(LambdaExpression expression);

        /// <summary>
        ///     Visit a <see cref="ListInitExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="ListInitExpression" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitListInitExpression(ListInitExpression expression);

        /// <summary>
        ///     Visit a <see cref="MemberExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="MemberExpression" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitMemberExpression(MemberExpression expression);

        /// <summary>
        ///     Visit a <see cref="MemberInitExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="MemberInitExpression" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitMemberInitExpression(MemberInitExpression expression);

        /// <summary>
        ///     Visit a <see cref="MethodCallExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="MethodCallExpression" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitMethodCallExpression(MethodCallExpression expression);

        /// <summary>
        ///     Visit a <see cref="NewArrayExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="NewArrayExpression" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitNewArrayExpression(NewArrayExpression expression);

        /// <summary>
        ///     Visit a <see cref="NewExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="NewExpression" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitNewExpression(NewExpression expression);

        /// <summary>
        ///     Visit a <see cref="ParameterExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="ParameterExpression" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitParameterExpression(ParameterExpression expression);

        /// <summary>
        ///     Visit a <see cref="TypeBinaryExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="TypeBinaryExpression" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitTypeBinaryExpression(TypeBinaryExpression expression);

        /// <summary>
        ///     Visit a <see cref="UnaryExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="UnaryExpression" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitUnaryExpression(UnaryExpression expression);

        /// <summary>
        ///     Visit a <see cref="ElementInit" />.
        /// </summary>
        /// <param name="elementInit"><see cref="ElementInit" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitElementInit(ElementInit elementInit);

        /// <summary>
        ///     Visit a <see cref="MemberAssignment" />.
        /// </summary>
        /// <param name="memberAssignment"><see cref="MemberAssignment" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitMemberAssignment(MemberAssignment memberAssignment);

        /// <summary>
        ///     Visit a <see cref="MemberListBinding" />.
        /// </summary>
        /// <param name="memberListBinding"><see cref="MemberListBinding" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitMemberListBinding(MemberListBinding memberListBinding);

        /// <summary>
        ///     Visit a <see cref="MemberMemberBinding" />.
        /// </summary>
        /// <param name="memberMemberBinding"><see cref="MemberMemberBinding" /> to visit.</param>
        /// <returns>Returns the result of the visit.</returns>
        protected abstract object VisitMemberMemberBinding(MemberMemberBinding memberMemberBinding);

        /// <summary>
        ///     Precondition of the method <see cref="VisitObject" />.
        /// </summary>
        /// <param name="objectToVisit"><see langword="object" /> to visit.</param>
        /// <returns>True, if the condition is true. False, if not.</returns>
        protected virtual bool VisitObjectPrecondition(object objectToVisit) => true;

        /// <summary>
        ///     Handle when the method <see cref="VisitObject" /> is <see langword="null" />.
        /// </summary>
        /// <returns>Returns the result of a <see langword="null" /> input.</returns>
        protected virtual object VisitObjectHandleNull() => null;

        /// <summary>
        ///     Handle when the method <see cref="Visit(System.Linq.Expressions.Expression)" /> is <see langword="null" />.
        /// </summary>
        /// <returns>Returns the result of a <see langword="null" /> input.</returns>
        protected virtual object VisitHandleNull() => null;

        /// <summary>
        ///     Precondition of the method <see cref="Visit(System.Linq.Expressions.Expression)" />.
        /// </summary>
        /// <param name="exp"><see cref="Expression" /> to visit.</param>
        /// <returns>True, if the condition is true. False, if not.</returns>
        private bool VisitPrecondition(Expression exp) => true;

        /// <summary>
        ///     Handles the case, when the type of the <see cref="Expression" /> is unkonwn.
        /// </summary>
        /// <param name="expression">The unknown <see cref="Expression" />.</param>
        /// <returns>Returns the result of a unknown <see cref="Expression" />.</returns>
        protected virtual object VisitUnknownExpression(Expression expression)
        {
            throw new Exception($"Expression \"{expression}\" could not be handled.");
        }

        /// <summary>
        ///     Handles the case, when the type of the <see langword="object" /> is unkonwn.
        /// </summary>
        /// <param name="unknownObject">The unknown <see langword="object" />.</param>
        /// <returns>Returns the result of a unknown <see langword="object" />.</returns>
        protected virtual object VisitUnkownObject(object unknownObject)
        {
            throw new Exception($"Conversion failed for object {unknownObject}");
        }
    }
}