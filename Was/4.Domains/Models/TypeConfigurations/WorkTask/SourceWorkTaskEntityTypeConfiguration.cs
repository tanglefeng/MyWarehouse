using Kengic.Was.Domain.Entity.WorkTask.WorkTasks;

namespace Kengic.Was.Model.TypeConfiguration.WorkTasks
{
    public class SourceWorkTaskEntityTypeConfiguration<TEntity>
        : WorkTaskEntityTypeConfiguration<TEntity> where TEntity : SourceWorkTask<string>
    {
        public SourceWorkTaskEntityTypeConfiguration()
        {
            Property(t => t.ExecuteWorkTaskId).HasMaxLength(256);
        }
    }
}