using System;
using System.Linq;
using System.Linq.Expressions;
using Kengic.Was.CrossCutting.LinqToWcf.Communications;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions;
using Kengic.Was.CrossCutting.LinqToWcf.Types;

namespace Kengic.Was.CrossCutting.LinqToWcf.QueryProvider
{
    /// <summary>
    ///     Client implementation of the <see cref="InterLinqQueryProvider" />.
    /// </summary>
    /// <seealso cref="InterLinqQueryProvider" />
    /// <seealso cref="IQueryProvider" />
    public class ClientQueryProvider : InterLinqQueryProvider
    {
        /// <summary>
        ///     Initializes this class.
        /// </summary>
        /// <param name="queryRemoteHandler"><see cref="IQueryRemoteHandler" /> to communicate with the server.</param>
        public ClientQueryProvider(Func<SerializableExpression, object> queryRemoteHandler)
        {
            if (queryRemoteHandler == null)
            {
                throw new ArgumentNullException(nameof(queryRemoteHandler));
            }
            Handler = queryRemoteHandler;
        }

        /// <summary>
        ///     Gets the <see cref="IQueryRemoteHandler" />;
        /// </summary>
        public Func<SerializableExpression, object> Handler { get; }

        /// <summary>
        ///     Executes the query and returns the requested data.
        /// </summary>
        /// <typeparam name="TResult">Type of the return value.</typeparam>
        /// <param name="expression"><see cref="Expression" /> tree to execute.</param>
        /// <returns>Returns the requested data of Type 'TResult'.</returns>
        /// <seealso cref="InterLinqQueryProvider.Execute" />
        public override TResult Execute<TResult>(Expression expression)
            => (TResult) TypeConverter.ConvertFromSerializable(typeof (TResult), Execute(expression));

        /// <summary>
        ///     Executes the query and returns the requested data.
        /// </summary>
        /// <param name="expression"><see cref="Expression" /> tree to execute.</param>
        /// <returns>Returns the requested data of Type <see langword="object" />.</returns>
        /// <seealso cref="InterLinqQueryProvider.Execute" />
        public override object Execute(Expression expression)
        {
            var serExp = expression.MakeSerializable();
            return Handler.Invoke(serExp);
        }
    }
}