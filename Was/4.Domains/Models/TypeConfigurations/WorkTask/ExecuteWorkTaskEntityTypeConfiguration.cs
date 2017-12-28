using Kengic.Was.Domain.Entity.WorkTask.WorkTasks;

namespace Kengic.Was.Model.TypeConfiguration.WorkTasks
{
    public class ExecuteWorkTaskEntityTypeConfiguration<TEntity>
        : WorkTaskEntityTypeConfiguration<TEntity> where TEntity : ExecuteWorkTask<string>
    {
    }
}