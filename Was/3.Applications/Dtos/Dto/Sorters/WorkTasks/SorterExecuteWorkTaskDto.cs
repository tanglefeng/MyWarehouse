using Kengic.Was.Application.WasModel.Dto.WorkTasks;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;

namespace Kengic.Was.Application.WasModel.Dto.Sorters.WorkTasks
{
    public class SorterExecuteWorkTaskDto : ExecuteWorkTaskDto<string>
    {
        public string Wave { get; set; }
        public string Order { get; set; }
        public string Shipment { get; set; }
        public string LogisticsBarcode { get; set; }
        public string ArticalBarcode { get; set; }
        public string LogicalDestination { get; set; }
        public int Priority { get; set; }
        public decimal PickingQuantity { get; set; }
        public decimal ReserveQuantity { get; set; }
        public decimal FinishQuantity { get; set; }
        public SortterTaskMode SortterTaskMode { get; set; }
    }
}