using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions;
using Kengic.Was.CrossCutting.LinqToWcf.Types;

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
                throw new ArgumentNullException(nameof(queryHandler));
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
        public object Retrieve(SerializableExpression expression)
        {
#if DEBUG
            Console.WriteLine(expression);
#endif

            MethodInfo mInfo;
            var realType = (Type) expression.Type.GetClrVersion();
            if (typeof (IQueryable).IsAssignableFrom(realType) &&
                (realType.GetGenericArguments().Length == 1))
            {
                // Find Generic Retrieve Method
                mInfo = typeof (ServerQueryHandler).GetMethod("RetrieveGeneric");
                mInfo = mInfo.MakeGenericMethod(realType.GetGenericArguments()[0]);
            }
            else
            {
                // Find Non-Generic Retrieve Method
                mInfo = typeof (ServerQueryHandler).GetMethod("RetrieveNonGenericObject");
            }

            var returnValue = mInfo.Invoke(this, new object[] {expression});

#if !SILVERLIGHT && DEBUG
            var ms = new MemoryStream();
            new NetDataContractSerializer().Serialize(ms, returnValue);
#endif
            return returnValue;
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
        public object RetrieveGeneric<T>(SerializableExpression serializableExpression)
        {
            var query = serializableExpression.Convert(QueryHandler) as IQueryable<T>;
            if (query != null)
            {
                var returnValue = query.ToArray();
                var convertedReturnValue = TypeConverter.ConvertToSerializable(returnValue);
                return convertedReturnValue;
            }
            return null;
        }

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
        public object RetrieveNonGenericObject(SerializableExpression serializableExpression)
        {
            var returnValue = serializableExpression.Convert(QueryHandler);
            var convertedReturnValue = TypeConverter.ConvertToSerializable(returnValue);
            return convertedReturnValue;
        }
    }
}