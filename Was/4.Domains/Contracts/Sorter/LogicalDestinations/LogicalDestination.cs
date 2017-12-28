using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Sorter.LogicalDestinations
{
    public class LogicalDestination : EntityForTime<string>
    {
        public string ParentId { get; set; }
        public string DisplayName { get; set; }
        public bool IsEnable { get; set; }
        public bool IsActive { get; set; }
    }
}