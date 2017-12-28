using Kengic.Was.Domain.Entity.WorkTask.WorkTasks;

namespace Kengic.Was.Domain.Entity.Vip
{
    public class VipPackageMessageWorkTask : WorkTask<string>
    {
        public string Whse { get; set; }
        public string MsgType { get; set; }

        public string CageCode { get; set; }

        public string TheLastCageCode { get; set; }
        public string TheNewCageCode { get; set; }
        public string ShuteCode { get; set; }
        public string Number { get; set; }

        public string Executor { get; set; }
    }
}