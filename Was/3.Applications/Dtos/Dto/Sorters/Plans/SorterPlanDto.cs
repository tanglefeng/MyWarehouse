namespace Kengic.Was.Application.WasModel.Dto.Sorters.Plans
{
    public class SorterPlanDto : EntityForTimeDto<string>
    {
        public bool IsEnable { get; set; }
        public bool IsActive { get; set; }
        public string LogicalSorter { get; set; }
    }
}