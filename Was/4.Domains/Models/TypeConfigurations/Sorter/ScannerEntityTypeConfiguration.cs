using Kengic.Was.Domain.Entity.Sorter.Scanners;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.Sorters
{
    internal class ScannerEntityTypeConfiguration : EntityForTimeEntityTypeConfiguration<Scanner>
    {
        public ScannerEntityTypeConfiguration()
        {
            Property(t => t.Brand).HasMaxLength(256);
        }
    }
}