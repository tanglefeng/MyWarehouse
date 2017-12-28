using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Sorter.LogicalDestinations
{
    public interface ILogicalDestinationRepository : IRepositoryForSyncDb<string, LogicalDestination>
    {
    }
}