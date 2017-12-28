using System;
using System.Linq;
using System.Linq.Expressions;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions;
using Kengic.Was.CrossCutting.LinqToWcf.Types;

namespace Kengic.Was.CrossCutting.LinqToWcf.Communications
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
                throw new ArgumentNullException("queryRemoteHandler");
            }
            Handler = queryRemoteHandler;
        }

        /// <summary>
        ///     Gets the <see cref="IQueryRemoteHandler" />;
        /// </summary>
        public Func<SerializableExpression, object> Handler { get; }

        public ClientQueryHandler.ExceptionOccuredHandler ExceptionOccured { get; set; }

        /// <summary>
        ///     Executes the query and returns the requested data.
        /// </summary>
        /// <typeparam name="TResult">Type of the return value.</typeparam>
        /// <param name="expression"><see cref="Expression" /> tree to execute.</param>
        /// <returns>Returns the requested data of Type 'TResult'.</returns>
        /// <seealso cref="InterLinqQueryProvider.Execute" />
        public override TResult Execute<TResult>(Expression expression)
        {
            return (TResult) TypeConverter.ConvertFromSerializable(typeof (TResult), Execute(expression));
        }

        /// <summary>
        ///     Executes the query and returns the requested data.
        /// </summary>
        /// <param name="expression"><see cref="Expression" /> tree to execute.</param>
        /// <returns>Returns the requested data of Type <see langword="object" />.</returns>
        /// <seealso cref="InterLinqQueryProvider.Execute" />
        public override object Execute(Expression expression)
        {
            var serExp = expression.MakeSerializable();
#if !SILVERLIGHT
            try
            {
                return Handler.Invoke(serExp);
            }
            catch (Exception ex)
            {
                if (ExceptionOccured != null)
                    ExceptionOccured(ex);
                throw;
            }
#else
			IAsyncResult asyncResult = Handler.BeginRetrieve(serExp, null, null);
            object receivedObject = null;

            if (!asyncResult.CompletedSynchronously)
            {
                asyncResult.AsyncWaitHandle.WaitOne();
            }

            try
            {
                return Handler.EndRetrieve(asyncResult);
            }
            catch (Exception ex)
            {
	            if (ExceptionOccured != null)
					ExceptionOccured(ex);
                throw;
            }
            finally
            {
#if !NETFX_CORE
                asyncResult.AsyncWaitHandle.Close();
#else
                asyncResult.AsyncWaitHandle.Dispose();
#endif
            }
#endif
        }
    }
}