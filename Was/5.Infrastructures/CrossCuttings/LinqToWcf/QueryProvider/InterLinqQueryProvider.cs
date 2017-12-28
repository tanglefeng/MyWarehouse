﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Kengic.Was.CrossCutting.LinqToWcf.Types;

namespace Kengic.Was.CrossCutting.LinqToWcf.QueryProvider
{
    /// <summary>
    ///     Abstract implementation of an <see cref="System.Linq.IQueryProvider" />.
    ///     Defines methods to create and execute queries that are described
    ///     by an <see cref="System.Linq.IQueryable" /> object.
    /// </summary>
    /// <seealso cref="System.Linq.IQueryProvider" />
    public abstract class InterLinqQueryProvider : IQueryProvider
    {
        /// <summary>
        ///     Constructs an <see cref="IQueryable{T}" /> object that can evaluate the query
        ///     represented by a specified <see cref="Expression" /> tree.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The <see cref="Type" /> of the elements of the <see cref="IQueryable{T}" /> that is returned.
        /// </typeparam>
        /// <param name="expression">An <see cref="Expression" /> that represents a LINQ query.</param>
        /// <returns>
        ///     An <see cref="IQueryable{T}" /> that can evaluate the query represented by the
        ///     specified <see cref="Expression" /> tree.
        /// </returns>
        /// <seealso cref="IQueryProvider.CreateQuery" />
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            => new InterLinqQuery<TElement>(this, expression);

        /// <summary>
        ///     Constructs an <see cref="IQueryable" /> object that can evaluate the query represented
        ///     by a specified <see cref="Expression" /> tree.
        /// </summary>
        /// <param name="expression">An <see cref="Expression" /> that represents a LINQ query.</param>
        /// <returns>
        ///     An <see cref="IQueryable" /> that can evaluate the query represented by the
        ///     specified <see cref="Expression" /> tree.
        /// </returns>
        /// <seealso cref="IQueryProvider.CreateQuery" />
        public IQueryable CreateQuery(Expression expression)
        {
            var elementType = InterLinqTypeSystem.GetElementType(expression.Type);
            try
            {
                return
                    (IQueryable)
                        Activator.CreateInstance(typeof (InterLinqQuery<>).MakeGenericType(elementType), this,
                            expression);
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        /// <summary>
        ///     Executes the strongly-typed query represented by a specified <see cref="Expression" /> tree.
        /// </summary>
        /// <typeparam name="TResult">The <see cref="Type" /> of the value that is returned by the query execution.</typeparam>
        /// <param name="expression">An <see cref="Expression" /> that represents a LINQ query.</param>
        /// <returns>A value of type TResult that results from executing the specified query.</returns>
        /// <seealso cref="IQueryProvider.Execute" />
        public virtual TResult Execute<TResult>(Expression expression) => (TResult) Execute(expression);

        /// <summary>
        ///     Executes the query represented by a specified <see cref="Expression" /> tree.
        /// </summary>
        /// <param name="expression">An <see cref="Expression" /> that represents a LINQ query.</param>
        /// <returns>An <see langword="object" /> that represents the result of executing the specified query.</returns>
        /// <seealso cref="IQueryProvider.Execute" />
        public abstract object Execute(Expression expression);
    }
}