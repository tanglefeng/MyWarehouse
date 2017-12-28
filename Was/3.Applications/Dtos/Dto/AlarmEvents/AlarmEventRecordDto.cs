using Kengic.Was.Domain.Entity.AlarmEvent;

namespace Kengic.Was.Application.WasModel.Dto.AlarmEvents
{
    public class AlarmEventRecordDto : EntityForTimeDto<string>
    {
        public string Type { get; set; }
        public string Source { get; set; }
        public string Object { get; set; }
        public AlarmEventStatus Status { get; set; }
        public AlarmEventGrade Grade { get; set; }
        public string Comments { get; set; }
    }
}