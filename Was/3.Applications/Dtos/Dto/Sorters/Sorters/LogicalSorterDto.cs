namespace Kengic.Was.Application.WasModel.Dto.Sorters.Sorters
{
    public class LogicalSorterDto : EntityForTimeDto<string>
    {
        public string PhycialSorter { get; set; }
        public string NodeId { get; set; }
    }
}