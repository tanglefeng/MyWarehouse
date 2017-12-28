namespace Kengic.Was.Application.WasModel.Dto.SystemSequences
{
    public class SystemSequenceDto : EntityForTimeDto<string>
    {
        public string Prefix { get; set; }
        public int Value { get; set; }
        public int MaxValue { get; set; }
        public int MinValue { get; set; }
        public int IncreaseRate { get; set; }
    }
}