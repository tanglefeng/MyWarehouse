using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Sorter.Sorters
{
    public class LogicalSorter : EntityForTime<string>
    {
        public string PhycialSorter { get; set; }
        public string NodeId { get; set; }
    }
}