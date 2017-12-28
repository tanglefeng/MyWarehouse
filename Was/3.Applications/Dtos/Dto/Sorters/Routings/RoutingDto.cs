namespace Kengic.Was.Application.WasModel.Dto.Sorters.Routings
{
    public class RoutingDto : EntityForTimeDto<string>
    {
        public string SorterPlan { get; set; }
        public string PhycialShute { get; set; }
        public string LogicalDestination { get; set; }
    }
}