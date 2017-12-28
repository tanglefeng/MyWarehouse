using Kengic.Was.Domain.Entity.Executor;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.Executors
{
    public class ExecutorEntityTypeConfiguration
        : EntityForTimeEntityTypeConfiguration<WasExecutor>
    {
        public ExecutorEntityTypeConfiguration()
        {
            Property(t => t.ExecuteOperator).HasMaxLength(256);
            Property(t => t.Connection).HasMaxLength(256);
            Property(t => t.CurrentAddress).HasMaxLength(256);
        }
    }
}