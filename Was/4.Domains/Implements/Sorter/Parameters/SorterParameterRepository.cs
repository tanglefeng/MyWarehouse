using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Sorter.Parameters;

namespace Kengic.Was.Domain.Sorter.Parameters
{
    public class SorterParameterRepository : RepositoryForSyncDb<string, SorterParameter>, ISorterParameterRepository
    {
        public SorterParameterRepository(IQueryableUnitOfWork queryableUnitOfWork) : base(queryableUnitOfWork)
        {
        }
    }
}