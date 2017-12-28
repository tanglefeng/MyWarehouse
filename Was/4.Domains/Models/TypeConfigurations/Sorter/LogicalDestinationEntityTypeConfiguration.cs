using Kengic.Was.Domain.Entity.Sorter.LogicalDestinations;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.Sorters
{
    internal class LogicalDestinationEntityTypeConfiguration : EntityForTimeEntityTypeConfiguration<LogicalDestination>
    {
        public LogicalDestinationEntityTypeConfiguration()
        {
            Property(t => t.ParentId).HasMaxLength(256);
            Property(t => t.DisplayName).HasMaxLength(256);
        }
    }
}