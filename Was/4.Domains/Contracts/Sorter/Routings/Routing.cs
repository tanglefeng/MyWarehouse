using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Sorter.Routings
{
    public class Routing : EntityForTime<string>
    {
        public string SorterPlan { get; set; }
        public string PhycialShute { get; set; }
        public string LogicalDestination { get; set; }

        public int? InternalPriority { get; set; }
        public int? ExternalPriority { get; set; }

        public RoutingStrategy RoutingStrategy { get; set; }
    }

    public enum RoutingStrategy
    {
        WaterFall = 10,
        Average = 20
    }
}