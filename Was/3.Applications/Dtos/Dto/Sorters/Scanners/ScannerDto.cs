namespace Kengic.Was.Application.WasModel.Dto.Sorters.Scanners
{
    public class ScannerDto : EntityForTimeDto<string>
    {
        public string PhycialSorter { get; set; }
        public string LogicalSorter { get; set; }
        public string Brand { get; set; }
    }
}