using System;
using Kengic.Was.Application.WasModel.Dto.WorkTasks;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;

namespace Kengic.Was.Application.WasModel.Dto.Sorters.WorkTasks
{
    public class SorterMessageWorkTaskDto : WorkTaskDto<string>
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
}