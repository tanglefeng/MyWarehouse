using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Sorter.Inducts
{
    public class Induct : EntityForTime<string>
    {
        public string PhycialSorter { get; set; }
        public string LogicalSorter { get; set; }
    }
}