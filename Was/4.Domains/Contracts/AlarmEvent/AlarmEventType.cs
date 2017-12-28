using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AlarmEvent
{
    public class AlarmEventType : EntityForTime<string>
    {
        public bool CanJumpConfirm { get; set; }
        public bool CanJumpHandle { get; set; }
        public string Comments { get; set; }
    }
}