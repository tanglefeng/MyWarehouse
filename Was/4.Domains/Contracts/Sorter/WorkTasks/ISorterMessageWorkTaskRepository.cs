using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Sorter.WorkTasks
{
    public interface ISorterMessageWorkTaskRepository : IQueryRepository<string, SorterMessageWorkTask>
    {
    }
}