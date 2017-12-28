using Kengic.Was.Domain.Entity.WorkTask.WorkTasks;

namespace Kengic.Was.Model.TypeConfiguration.WorkTasks
{
    public class SubWorkTaskEntityTypeConfiguration<TEntity>
        : WorkTaskEntityTypeConfiguration<TEntity> where TEntity : SubWorkTask<string>
    {
        public SubWorkTaskEntityTypeConfiguration()
        {
            Property(t => t.ExecuteWorkTaskId).HasMaxLength(256);
        }
    }
}