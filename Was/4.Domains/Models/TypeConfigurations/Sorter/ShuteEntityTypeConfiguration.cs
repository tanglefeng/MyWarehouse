using Kengic.Was.Domain.Entity.Sorter.Shutes;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.Sorters
{
    internal class ShuteEntityTypeConfiguration : EntityForTimeEntityTypeConfiguration<Shute>
    {
        public ShuteEntityTypeConfiguration()
        {
            Property(t => t.ShuteType).HasMaxLength(256);
            Property(t => t.DisplayName).HasMaxLength(256);
            Property(t => t.DeviceName1).HasMaxLength(256);
            Property(t => t.DeviceName2).HasMaxLength(256);
            Property(t => t.PhycialSorter).HasMaxLength(256);
            Property(t => t.LogicalSorter).HasMaxLength(256);
        }
    }
}