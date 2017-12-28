using System;
using Kengic.Was.Domain.Entity.WorkTask.WorkTasks;

namespace Kengic.Was.Domain.Entity.Sorter.WorkTasks
{
    public class SorterMessageWorkTask : WorkTask<string>
    {
        public SorterMessageType Type { get; set; }
        public string Connect { get; set; }
        public string Induct { get; set; }
        public string InductMode { get; set; }
        public DateTime? InductTime { get; set; }
        public string TrackingId { get; set; }

        public string Result { get; set; }
        public string CurrentShuteAddr { get; set; }

        public decimal Weight { get; set; }
    }

    public enum SorterMessageType
    {
        Default = 0,
        InductSuccess = 10,
        InductFailure = 20,
        UnknowPackage = 30,
        PreCreatePackage = 40,
        CreatePackage = 50,
        CancelPackage = 60,
        Complement = 70
    }
}