using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Sorter.Plans
{
    public class SorterPlan : EntityForTime<string>
    {
        public bool IsEnable { get; set; }
        public bool IsActive { get; set; }
        public string LogicalSorter { get; set; }
    }
}