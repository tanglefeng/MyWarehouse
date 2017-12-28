using Kengic.Was.CrossCutting.LinqToWcf.Communications;
using Kengic.Was.CrossCutting.LinqToWcf.Expressions;
using Kengic.Was.DistributedServices.Common.QueryHandlers;
using Kengic.Was.Wcf.IDynamicQuery;

namespace Kengic.Was.Wcf.DynamicQuery
{
    public class DynamicQueryService : IDynamicQueryService
    {
        private static readonly ApplicationServiceQueryHandler ApplicationServiceQueryHandler =
            new ApplicationServiceQueryHandler();

        private static readonly ServerQueryHandler ServerQueryHandler =
            new ServerQueryHandler(ApplicationServiceQueryHandler);

        public object Retrieve(SerializableExpression expression)
        {
            var result = ServerQueryHandler.Retrieve(expression);
            return result;
        }
    }
}