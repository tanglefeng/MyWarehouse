using Kengic.Was.Domain.Entity.Sorter.Sorters;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.Sorters
{
    internal class LogicalSorterEntityTypeConfiguration : EntityForTimeEntityTypeConfiguration<LogicalSorter>
    {
        public LogicalSorterEntityTypeConfiguration()
        {
            Property(t => t.PhycialSorter).HasMaxLength(256);
            Property(t => t.NodeId).HasMaxLength(256);
        }
    }
}