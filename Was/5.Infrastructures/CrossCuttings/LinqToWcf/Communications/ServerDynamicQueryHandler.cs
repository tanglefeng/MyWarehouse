using System;
using System.Linq;
using System.Linq.Expressions;
using Kengic.Was.CrossCutting.LinqToWcf.Jsons;
using Newtonsoft.Json;

namespace Kengic.Was.CrossCutting.LinqToWcf.Communications
{
    /// <summary>
    ///     Server implementation of the <see cref="IQueryRemoteHandler" />.
    /// </summary>
    /// <seealso cref="IQueryRemoteHandler" />
    public class ServerQueryHandler : IQueryRemoteHandler, IDisposable
    {
        /// <summary>
        ///     Initializes this class.
        /// </summary>
        /// <param name="queryHandler"><see cref="IQueryHandler" /> instance.</param>
        public ServerQueryHandler(IQueryHandler queryHandler)
        {
            if (queryHandler == null)
            {
                throw new ArgumentNullException("queryHandler");
            }
            QueryHandler = queryHandler;
        }

        /// <summary>
        ///     Gets the <see cref="IQueryHandler" />.
        /// </summary>
        public IQueryHandler QueryHandler { get; protected set; }

        /// <summary>
        ///     Disposes the server instance.
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        ///     Retrieves data from the server by an <see cref="SerializableExpression">Expression</see> tree.
        /// </summary>
        /// <remarks>
        ///     This method's return type depends on the submitted
        ///     <see cref="SerializableExpression">Expression</see> tree.
        ///     Here some examples ('T' is the requested type):
        ///     <list type="list">
        ///         <listheader>
        ///             <term>Method</term>
        ///             <description>Return Type</description>
        ///         </listheader>
        ///         <item>
        ///             <term>Select(...)</term>
        ///             <description>T[;</description>
        ///         </item>
        ///         <item>
        ///             <term>First(...), Last(...)</term>
        ///             <description>T</description>
        ///         </item>
        ///         <item>
        ///             <term>Count(...)</term>
        ///             <description>
        ///                 <see langword="int" />
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Contains(...)</term>
        ///             <description>
        ///                 <see langword="bool" />
        ///             </description>
        ///         </item>
        ///     </list>
        /// </remarks>
        /// <param name="expression">
        ///     <see cref="SerializableExpression">Expression</see> tree
        ///     containing selection and projection.
        /// </param>
        /// <returns>Returns requested data.</returns>
        /// <seealso cref="IQueryRemoteHandler.Retrieve" />
        public object Retrieve(string expression)
        {
            try
            {
                //#if DEBUG
                //                Console.WriteLine(expression);
                //                Console.WriteLine();
                //#endif
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new ExpressionJsonConverter(QueryHandler));
                var expressionCall = JsonConvert.DeserializeObject<MethodCallExpression>(expression, settings);
                var invokeResult = Expression.Lambda(expressionCall).Compile().DynamicInvoke();
                var returnValue = JsonConvert.SerializeObject(invokeResult, Formatting.None);
                return returnValue;
            }
            catch (Exception ex)
            {
                if (!HandleExceptionInRetrieve(ex))
                    throw;
                return null;
            }
        }

        /// <summary>
        ///     Retrieves data from the server by an <see cref="SerializableExpression">Expression</see> tree.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IQueryable" />.</typeparam>
        /// <param name="serializableExpression">
        ///     <see cref="SerializableExpression">Expression</see> tree
        ///     containing selection and projection.
        /// </param>
        /// <returns>Returns requested data.</returns>
        /// <seealso cref="IQueryRemoteHandler.Retrieve" />
        /// <remarks>
        ///     This method's return type depends on the submitted
        ///     <see cref="SerializableExpression">Expression</see> tree.
        ///     Here some examples ('T' is the requested type):
        ///     <list type="list">
        ///         <listheader>
        ///             <term>Method</term>
        ///             <description>Return Type</description>
        ///         </listheader>
        ///         <item>
        ///             <term>Select(...)</term>
        ///             <description>T[;</description>
        ///         </item>
        ///         <item>
        ///             <term>First(...), Last(...)</term>
        ///             <description>T</description>
        ///         </item>
        ///         <item>
        ///             <term>Count(...)</term>
        ///             <description>
        ///                 <see langword="int" />
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Contains(...)</term>
        ///             <description>
        ///                 <see langword="bool" />
        ///             </description>
        ///         </item>
        ///     </list>
        /// </remarks>
        /// <summary>
        ///     Retrieves data from the server by an <see cref="SerializableExpression">Expression</see> tree.
        /// </summary>
        /// <param name="serializableExpression">
        ///     <see cref="SerializableExpression">Expression</see> tree
        ///     containing selection and projection.
        /// </param>
        /// <returns>Returns requested data.</returns>
        /// <remarks>
        ///     This method's return type depends on the submitted
        ///     <see cref="SerializableExpression">Expression</see> tree.
        ///     Here some examples ('T' is the requested type):
        ///     <list type="list">
        ///         <listheader>
        ///             <term>Method</term>
        ///             <description>Return Type</description>
        ///         </listheader>
        ///         <item>
        ///             <term>Select(...)</term>
        ///             <description>T[;</description>
        ///         </item>
        ///         <item>
        ///             <term>First(...), Last(...)</term>
        ///             <description>T</description>
        ///         </item>
        ///         <item>
        ///             <term>Count(...)</term>
        ///             <description>
        ///                 <see langword="int" />
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Contains(...)</term>
        ///             <description>
        ///                 <see langword="bool" />
        ///             </description>
        ///         </item>
        ///     </list>
        /// </remarks>
        /// <seealso cref="IQueryRemoteHandler.Retrieve" />
        /// <summary>
        ///     Handles an <see cref="Exception" /> occured in the
        ///     <see cref="IQueryRemoteHandler.Retrieve" /> Method.
        /// </summary>
        /// <param name="exception">
        ///     Thrown <see cref="Exception" />
        ///     in <see cref="IQueryRemoteHandler.Retrieve" /> Method.
        /// </param>
        protected virtual bool HandleExceptionInRetrieve(Exception exception)
        {
            return false;
        }
    }
}