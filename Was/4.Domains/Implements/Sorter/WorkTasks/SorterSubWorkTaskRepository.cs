using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;

namespace Kengic.Was.Domain.Sorter.WorkTasks
{
    public class SorterSubWorkTaskRepository : RepositoryForOnlyDb<string, SorterSubWorkTask>,
        ISorterSubWorkTaskRepository
    {
        public SorterSubWorkTaskRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}