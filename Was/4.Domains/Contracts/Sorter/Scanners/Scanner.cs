using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Sorter.Scanners
{
    public class Scanner : EntityForTime<string>
    {
        public string PhycialSorter { get; set; }
        public string LogicalSorter { get; set; }
        public string Brand { get; set; }
    }
}