using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;

namespace Kengic.Was.Domain.Sorter.WorkTasks
{
    public class SorterMessageWorkTaskRepository : RepositoryForOnlyDb<string, SorterMessageWorkTask>,
        ISorterMessageWorkTaskRepository
    {
        public SorterMessageWorkTaskRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}