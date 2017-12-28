using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AlarmEvent
{
    public class AlarmEventRecord : EntityForTime<string>
    {
        public string Type { get; set; }
        public string Source { get; set; }
        public string Object { get; set; }
        public AlarmEventStatus Status { get; set; }
        public AlarmEventGrade Grade { get; set; }
        public string Comments { get; set; }
    }

    public enum AlarmEventStatus
    {
        Default = 0,
        Create = 10,
        Confirmed = 20,
        ToHandle = 30,
        Handled = 40,
        Removed = 50
    }

    public enum AlarmEventGrade
    {
        Default = 0,
        Common = 10,
        Important = 20,
        Urgent = 30,
        Critical = 40
    }
}