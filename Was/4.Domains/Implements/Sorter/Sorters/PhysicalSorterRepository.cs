using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Sorter.Sorters;

namespace Kengic.Was.Domain.Sorter.Sorters
{
    public class PhysicalSorterRepository : RepositoryForSyncDb<string, PhysicalSorter>, IPhysicalSorterRepository
    {
        public PhysicalSorterRepository(IQueryableUnitOfWork queryableUnitOfWork) : base(queryableUnitOfWork)
        {
        }
    }
}