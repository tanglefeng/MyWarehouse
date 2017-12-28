namespace Kengic.Was.Application.WasModel.Dto.Executors
{
    public class WasExecutorDto : EntityForTimeDto<string>
    {
        public string ExecuteOperator { get; set; }
        public string Connection { get; set; }
        public string CurrentAddress { get; set; }
    }
}