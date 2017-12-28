namespace Kengic.Was.Application.WasModel.Dto.AlarmEvents
{
    public class AlarmEventTypeDto : EntityForTimeDto<string>
    {
        public bool CanJumpConfirm { get; set; }
        public bool CanJumpHandle { get; set; }
        public string Comments { get; set; }
    }
}