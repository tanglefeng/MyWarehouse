using System;
using System.Linq;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions;
using Kengic.Was.CrossCutting.LinqToWcf.QueryProvider;

namespace Kengic.Was.CrossCutting.LinqToWcf.Communications
{
    /// <summary>
    ///     Client implementation of the <see cref="InterLinqQueryHandler" />.
    /// </summary>
    /// <seealso cref="InterLinqQueryHandler" />
    /// <seealso cref="IQueryHandler" />
    public class ClientQueryHandler : InterLinqQueryHandler
    {
        /// <summary>
        ///     <see cref="IQueryRemoteHandler" /> instance.
        /// </summary>
        protected Func<SerializableExpression, object> QueryRemoteHandler;

        /// <summary>
        ///     Initializes this class.
        /// </summary>
        /// <param name="queryRemoteHandler"><see cref="IQueryRemoteHandler" /> to communicate with the server.</param>
        public ClientQueryHandler(Func<SerializableExpression, object> queryRemoteHandler)
        {
            QueryRemoteHandler = queryRemoteHandler;
        }

        /// <summary>
        ///     Gets the <see cref="IQueryProvider" />.
        /// </summary>
        /// <seealso cref="InterLinqQueryHandler.QueryProvider" />
        public override IQueryProvider QueryProvider => new ClientQueryProvider(QueryRemoteHandler);
    }
}