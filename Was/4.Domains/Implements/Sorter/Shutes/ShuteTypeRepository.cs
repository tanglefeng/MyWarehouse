using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Sorter.Shutes;

namespace Kengic.Was.Domain.Sorter.Shutes
{
    public class ShuteTypeRepository : RepositoryForSyncDb<string, ShuteType>, IShuteTypeRepository
    {
        public ShuteTypeRepository(IQueryableUnitOfWork queryableUnitOfWork) : base(queryableUnitOfWork)
        {
        }
    }
}