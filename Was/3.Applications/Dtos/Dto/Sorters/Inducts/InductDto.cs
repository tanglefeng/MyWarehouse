namespace Kengic.Was.Application.WasModel.Dto.Sorters.Inducts
{
    public class InductDto : EntityForTimeDto<string>
    {
        public string PhycialSorter { get; set; }
        public string LogicalSorter { get; set; }
    }
}