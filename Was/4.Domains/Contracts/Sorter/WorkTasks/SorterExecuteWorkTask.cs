using Kengic.Was.Domain.Entity.WorkTask.WorkTasks;

namespace Kengic.Was.Domain.Entity.Sorter.WorkTasks
{
    public class SorterExecuteWorkTask : ExecuteWorkTask<string>
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

    public enum SortterTaskMode
    {
        Default = 0,
        Unique = 10,
        Counted = 20
    }
}