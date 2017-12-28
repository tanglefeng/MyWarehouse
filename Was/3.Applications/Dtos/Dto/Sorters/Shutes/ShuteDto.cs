namespace Kengic.Was.Application.WasModel.Dto.Sorters.Shutes
{
    public class ShuteDto : EntityForTimeDto<string>
    {
        public string ShuteType { get; set; }
        public string DisplayName { get; set; }
        public string DeviceName1 { get; set; }
        public string DeviceName2 { get; set; }
        public bool IsEnable { get; set; }
        public bool IsFull { get; set; }
        public bool IsActive { get; set; }
        public string PhycialSorter { get; set; }
        public string LogicalSorter { get; set; }
        public string PackageNo { get; set; }
        public string TheLastPackageNo { get; set; }
        public int PackageNumber { get; set; }
        public int TheLastPackageNumber { get; set; }
        public string PrintId { get; set; }

        public string SpecialPackageNo { get; set; }
    }
}