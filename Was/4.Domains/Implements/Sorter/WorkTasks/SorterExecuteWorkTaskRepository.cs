using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;

namespace Kengic.Was.Domain.Sorter.WorkTasks
{
    public class SorterExecuteWorkTaskRepository : RepositoryForOnlyDb<string, SorterExecuteWorkTask>,
        ISorterExecuteWorkTaskRepository
    {
        public SorterExecuteWorkTaskRepository(IQueryableUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}