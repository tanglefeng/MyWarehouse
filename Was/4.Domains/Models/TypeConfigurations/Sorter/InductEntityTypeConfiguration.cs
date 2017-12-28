using Kengic.Was.Domain.Entity.Sorter.Inducts;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.Sorters
{
    internal class InductEntityTypeConfiguration : EntityForTimeEntityTypeConfiguration<Induct>
    {
        public InductEntityTypeConfiguration()
        {
            Property(t => t.PhycialSorter).HasMaxLength(256);
            Property(t => t.LogicalSorter).HasMaxLength(256);
        }
    }
}