using Kengic.Was.Domain.Entity.WorkTask.WorkTasks;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.WorkTasks
{
    public class WorkTaskEntityTypeConfiguration<TEntity>
        : EntityForTimeEntityTypeConfiguration<TEntity> where TEntity : WorkTask<string>
    {
        public WorkTaskEntityTypeConfiguration()
        {
            Property(t => t.OperatorName).HasMaxLength(256);
            Property(t => t.ObjectToHandle).HasMaxLength(256);
            Property(t => t.ReadyBy).HasMaxLength(256);
            Property(t => t.ReleaseBy).HasMaxLength(256);
            Property(t => t.ActiveBy).HasMaxLength(256);
            Property(t => t.CancelledBy).HasMaxLength(256);
            Property(t => t.TerminatedBy).HasMaxLength(256);
            Property(t => t.SuspendedBy).HasMaxLength(256);
            Property(t => t.ResumeBy).HasMaxLength(256);
            Property(t => t.Comments).HasMaxLength(256);
            Property(t => t.Results).HasMaxLength(256);
        }
    }
}