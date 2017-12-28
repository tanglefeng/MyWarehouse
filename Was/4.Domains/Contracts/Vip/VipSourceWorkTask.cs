using Kengic.Was.Domain.Entity.WorkTask.WorkTasks;

namespace Kengic.Was.Domain.Entity.Vip
{
    public class VipSourceWorkTask : SourceWorkTask<string>
    {
        public string Whse { get; set; }
        public string MsgType { get; set; }
        public string ToteNbr { get; set; }
        public string PackingCode { get; set; }
        public string WmsDestLocn { get; set; }
        public string WcsDestLocn { get; set; }
        public string CageCode { get; set; }
        public string BindingMode { get; set; }
        public string Induction { get; set; }
        public int Recirccount { get; set; }
        public string FailReason { get; set; }
        public string Cube { get; set; }
        public string Weight { get; set; }
        public string WmsMsgid { get; set; }
        public string Executor { get; set; }
    }
}