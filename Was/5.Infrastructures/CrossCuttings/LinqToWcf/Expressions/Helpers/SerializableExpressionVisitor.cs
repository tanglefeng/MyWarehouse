using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions.SerializableTypes;

namespace Kengic.Was.CrossCutting.LinqToWcf.Expressions.Helpers
{
    /// <summary>
    ///     This is a basic visitor for serializable expressions.
    /// </summary>
    public abstract class SerializableExpressionVisitor
    {
        private Dictionary<int, object> _convertedObjects = new Dictionary<int, object>();

        /// <summary>
        ///     Initializes this class.
        /// </summary>
        /// <param name="expression"><see cref="SerializableExpression" /> to convert.</param>
        protected SerializableExpressionVisitor(SerializableExpression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            ExpressionToConvert = expression;
        }

        /// <summary>
        ///     Gets the <see cref="SerializableExpression">ExpressionToConvert</see>.
        /// </summary>
        public SerializableExpression ExpressionToConvert { get; }

        /// <summary>
        ///     Visits the <see cref="SerializableExpression" /> tree to convert and
        ///     returns the converted <see cref="Expression" />.
        /// </summary>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        public object Visit()
        {
            _convertedObjects = new Dictionary<int, object>();
            return VisitResult(ExpressionToConvert);
        }

        /// <summary>
        ///     Returns an <see cref="Expression" /> by visiting and converting <paramref name="expression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableExpression" /> to visit.</param>
        /// <returns>Returns an <see cref="Expression" /> by visiting and converting <paramref name="expression" />.</returns>
        public Expression Visit(SerializableExpression expression)
        {
            if (expression == null)
            {
                return null;
            }
            if (_convertedObjects.ContainsKey(expression.HashCode))
            {
                if (_convertedObjects[expression.HashCode] is Expression)
                    return (Expression) _convertedObjects[expression.HashCode];
            }

            Expression returnValue;

            if (expression is SerializableBinaryExpression)
            {
                returnValue = VisitSerializableBinaryExpression(expression as SerializableBinaryExpression);
            }
            else if (expression is SerializableConditionalExpression)
            {
                returnValue = VisitSerializableConditionalExpression(expression as SerializableConditionalExpression);
            }
            else if (expression is SerializableConstantExpression)
            {
                returnValue = VisitSerializableConstantExpression(expression as SerializableConstantExpression);
            }
            else if (expression is SerializableInvocationExpression)
            {
                returnValue = VisitSerializableInvocationExpression(expression as SerializableInvocationExpression);
            }
            else if (expression is SerializableLambdaExpression)
            {
                if (expression is SerializableExpressionTyped)
                {
                    var executeMethod = GetType()
                        .GetMethod("VisitSerializableExpressionTyped", BindingFlags.NonPublic | BindingFlags.Instance);

                    var genericExecuteMethod = executeMethod.MakeGenericMethod((Type) expression.Type.GetClrVersion());

                    returnValue = (Expression) genericExecuteMethod.Invoke(this, new object[] {expression});
                }
                else
                {
                    returnValue = VisitSerializableLambdaExpression(expression as SerializableLambdaExpression);
                }
            }
            else if (expression is SerializableListInitExpression)
            {
                returnValue = VisitSerializableListInitExpression(expression as SerializableListInitExpression);
            }
            else if (expression is SerializableMemberExpression)
            {
                returnValue = VisitSerializableMemberExpression(expression as SerializableMemberExpression);
            }
            else if (expression is SerializableMemberInitExpression)
            {
                returnValue = VisitSerializableMemberInitExpression(expression as SerializableMemberInitExpression);
            }
            else if (expression is SerializableMethodCallExpression)
            {
                returnValue = VisitSerializableMethodCallExpression(expression as SerializableMethodCallExpression);
            }
            else if (expression is SerializableNewArrayExpression)
            {
                returnValue = VisitSerializableNewArrayExpression(expression as SerializableNewArrayExpression);
            }
            else if (expression is SerializableNewExpression)
            {
                returnValue = VisitSerializableNewExpression(expression as SerializableNewExpression);
            }
            else if (expression is SerializableParameterExpression)
            {
                returnValue = VisitSerializableParameterExpression(expression as SerializableParameterExpression);
            }
            else if (expression is SerializableTypeBinaryExpression)
            {
                returnValue = VisitSerializableTypeBinaryExpression(expression as SerializableTypeBinaryExpression);
            }
            else if (expression is SerializableUnaryExpression)
            {
                returnValue = VisitSerializableUnaryExpression(expression as SerializableUnaryExpression);
            }
            else
            {
                returnValue = VisitUnknownSerializableExpression(expression);
            }

            if (!_convertedObjects.ContainsKey(expression.HashCode))
                _convertedObjects.Add(expression.HashCode, returnValue);

            return returnValue;
        }

        /// <summary>
        ///     Returns an <see cref="IEnumerable{T}" />. Each element in <paramref name="enumerable" />
        ///     will be visited by calling Visit&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">Generic argument of the returned <see cref="IEnumerable" />.</typeparam>
        /// <param name="enumerable"><see cref="IEnumerable{T}" /> to visit.</param>
        /// <returns>Returns an <see cref="IEnumerable{T}" />.</returns>
        public IEnumerable<T> VisitCollection<T>(IEnumerable enumerable) where T : Expression
        {
            if (enumerable == null)
            {
                return null;
            }

            var returnValues =
                (from SerializableExpression expression in enumerable select (T) Visit(expression)).ToList();
            return returnValues;
        }

        /// <summary>
        ///     Returns the value of the <see langword="object" />.
        /// </summary>
        /// <typeparam name="T">Return <see cref="Type" />.</typeparam>
        /// <param name="otherObject"><see langword="object" /> to visit.</param>
        /// <returns>Returns the value of the <see langword="object" />.</returns>
        protected T VisitObject<T>(object otherObject) where T : class
        {
            if (otherObject == null)
            {
                return null;
            }

            object foundObject = null;
            if (otherObject is SerializableElementInit)
            {
                foundObject = VisitSerializableElementInit((SerializableElementInit) otherObject);
            }
            else if (otherObject is SerializableMemberAssignment)
            {
                foundObject = VisitSerializableMemberAssignment((SerializableMemberAssignment) otherObject);
            }
            else if (otherObject is SerializableMemberListBinding)
            {
                foundObject = VisitSerializableMemberListBinding((SerializableMemberListBinding) otherObject);
            }
            else if (otherObject is SerializableMemberMemberBinding)
            {
                foundObject = VisitSerializableMemberMemberBinding((SerializableMemberMemberBinding) otherObject);
            }

            if ((foundObject == null) || !(foundObject is T))
            {
                throw new NotImplementedException();
            }
            return foundObject as T;
        }

        /// <summary>
        ///     Returns an <see cref="IEnumerable{T}" />. Each element in <paramref name="enumerable" />
        ///     will be visited by calling <see cref="SerializableExpressionVisitor.VisitObject{T}" />.
        /// </summary>
        /// <typeparam name="T">Generic argument of the returned <see cref="IEnumerable" />.</typeparam>
        /// <param name="enumerable"><see cref="IEnumerable{T}" /> to visit.</param>
        /// <returns>Returns an <see cref="IEnumerable{T}" />.</returns>
        protected IEnumerable<T> VisitObjectCollection<T>(IEnumerable enumerable) where T : class
        {
            if (enumerable == null)
            {
                return null;
            }

            var returnValues = new List<T>();
            foreach (var obj in enumerable)
            {
                returnValues.Add(VisitObject<T>(obj));
            }
            return returnValues;
        }

        /// <summary>
        ///     Returns the value of the <see cref="Expression" />.
        /// </summary>
        /// <param name="expression"><see cref="Expression" /> to visit.</param>
        /// <returns>Returns the value of the <see cref="Expression" />.</returns>
        public object VisitResult(SerializableExpression expression)
        {
            if (expression == null)
            {
                return null;
            }
            if (_convertedObjects.ContainsKey(expression.HashCode))
            {
                return _convertedObjects[expression.HashCode];
            }

            object foundObject;

            if (expression is SerializableConstantExpression)
            {
                foundObject = GetResultConstantExpression((SerializableConstantExpression) expression);
            }
            else if (expression is SerializableMethodCallExpression)
            {
                foundObject = GetResultMethodCallExpression((SerializableMethodCallExpression) expression);
            }
            else
            {
                throw new NotImplementedException();
            }

            _convertedObjects[expression.HashCode] = foundObject;

            return foundObject;
        }

        /// <summary>
        ///     Visits a <see cref="SerializableBinaryExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableBinaryExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected abstract Expression VisitSerializableBinaryExpression(SerializableBinaryExpression expression);

        /// <summary>
        ///     Visits a <see cref="SerializableConditionalExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableConditionalExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected abstract Expression VisitSerializableConditionalExpression(
            SerializableConditionalExpression expression);

        /// <summary>
        ///     Visits a <see cref="SerializableConstantExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableConstantExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected abstract Expression VisitSerializableConstantExpression(SerializableConstantExpression expression);

        /// <summary>
        ///     Visits a <see cref="SerializableInvocationExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableInvocationExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected abstract Expression VisitSerializableInvocationExpression(SerializableInvocationExpression expression);

        /// <summary>
        ///     Visits a <see cref="SerializableExpressionTyped" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableExpressionTyped" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected abstract Expression VisitSerializableExpressionTyped<T>(SerializableExpressionTyped expression);

        /// <summary>
        ///     Visits a <see cref="SerializableLambdaExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableLambdaExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected abstract Expression VisitSerializableLambdaExpression(SerializableLambdaExpression expression);

        /// <summary>
        ///     Visits a <see cref="SerializableListInitExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableListInitExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected abstract Expression VisitSerializableListInitExpression(SerializableListInitExpression expression);

        /// <summary>
        ///     Visits a <see cref="SerializableMemberExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableMemberExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected abstract Expression VisitSerializableMemberExpression(SerializableMemberExpression expression);

        /// <summary>
        ///     Visits a <see cref="SerializableMemberInitExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableMemberInitExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected abstract Expression VisitSerializableMemberInitExpression(SerializableMemberInitExpression expression);

        /// <summary>
        ///     Visits a <see cref="SerializableMethodCallExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableMethodCallExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected abstract Expression VisitSerializableMethodCallExpression(SerializableMethodCallExpression expression);

        /// <summary>
        ///     Visits a <see cref="SerializableNewArrayExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableNewArrayExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected abstract Expression VisitSerializableNewArrayExpression(SerializableNewArrayExpression expression);

        /// <summary>
        ///     Visits a <see cref="SerializableNewExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableNewExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected abstract Expression VisitSerializableNewExpression(SerializableNewExpression expression);

        /// <summary>
        ///     Visits a <see cref="SerializableParameterExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableParameterExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected abstract Expression VisitSerializableParameterExpression(SerializableParameterExpression expression);

        /// <summary>
        ///     Visits a <see cref="SerializableTypeBinaryExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableTypeBinaryExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected abstract Expression VisitSerializableTypeBinaryExpression(SerializableTypeBinaryExpression expression);

        /// <summary>
        ///     Visits a <see cref="SerializableUnaryExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableUnaryExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected abstract Expression VisitSerializableUnaryExpression(SerializableUnaryExpression expression);

        /// <summary>
        ///     Visits a <see cref="SerializableExpression" />.
        /// </summary>
        /// <param name="expression"><see cref="SerializableExpression" /> to visit.</param>
        /// <returns>Returns the converted <see cref="Expression" />.</returns>
        protected abstract Expression VisitUnknownSerializableExpression(SerializableExpression expression);

        /// <summary>
        ///     Visits a <see cref="SerializableElementInit" />.
        /// </summary>
        /// <param name="elementInit"><see cref="SerializableElementInit" /> to visit.</param>
        /// <returns>Returns the converted <see langword="object" />.</returns>
        protected abstract object VisitSerializableElementInit(SerializableElementInit elementInit);

        /// <summary>
        ///     Visits a <see cref="SerializableMemberAssignment" />.
        /// </summary>
        /// <param name="memberAssignment"><see cref="SerializableMemberAssignment" /> to visit.</param>
        /// <returns>Returns the converted <see langword="object" />.</returns>
        protected abstract object VisitSerializableMemberAssignment(SerializableMemberAssignment memberAssignment);

        /// <summary>
        ///     Visits a <see cref="SerializableMemberListBinding" />.
        /// </summary>
        /// <param name="memberListBinding"><see cref="SerializableMemberListBinding" /> to visit.</param>
        /// <returns>Returns the converted <see langword="object" />.</returns>
        protected abstract object VisitSerializableMemberListBinding(SerializableMemberListBinding memberListBinding);

        /// <summary>
        ///     Visits a <see cref="SerializableMemberMemberBinding" />.
        /// </summary>
        /// <param name="memberMemberBinding"><see cref="SerializableMemberMemberBinding" /> to visit.</param>
        /// <returns>Returns the converted <see langword="object" />.</returns>
        protected abstract object VisitSerializableMemberMemberBinding(
            SerializableMemberMemberBinding memberMemberBinding);

        /// <summary>
        ///     Executes a <see cref="SerializableConstantExpression" /> and returns the result.
        /// </summary>
        /// <param name="expression"><see cref="SerializableConstantExpression" /> to convert.</param>
        /// <returns>Returns the result of a <see cref="SerializableConstantExpression" />.</returns>
        protected abstract object GetResultConstantExpression(SerializableConstantExpression expression);

        /// <summary>
        ///     Executes a <see cref="SerializableMethodCallExpression" /> and returns the result.
        /// </summary>
        /// <param name="expression"><see cref="SerializableMethodCallExpression" /> to convert.</param>
        /// <returns>Returns the result of a <see cref="SerializableMethodCallExpression" />.</returns>
        protected abstract object GetResultMethodCallExpression(SerializableMethodCallExpression expression);
    }
}