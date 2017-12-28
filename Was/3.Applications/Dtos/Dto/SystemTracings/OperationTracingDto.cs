namespace Kengic.Was.Application.WasModel.Dto.SystemTracings
{
    public class OperationTracingDto : EntityForTimeDto<string>
    {
        public string User { get; set; }
        public string Source { get; set; }
        public string Operation { get; set; }
        public string Object { get; set; }
        public string Result { get; set; }
        public string ObjectValue { get; set; }
        public string Context { get; set; }
        public string Comments { get; set; }
    }
}