using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Sorter.LogicalDestinations;

namespace Kengic.Was.Domain.Sorter.LogicalDestinations
{
    public class LogicalDestinationRepository : RepositoryForSyncDb<string, LogicalDestination>,
        ILogicalDestinationRepository
    {
        public LogicalDestinationRepository(IQueryableUnitOfWork queryableUnitOfWork) : base(queryableUnitOfWork)
        {
        }
    }
}