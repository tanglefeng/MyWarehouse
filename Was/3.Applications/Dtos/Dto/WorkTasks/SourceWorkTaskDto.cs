namespace Kengic.Was.Application.WasModel.Dto.WorkTasks
{
    public class SourceWorkTaskDto<TKey> : WorkTaskDto<TKey>
    {
        public string ExecuteWorkTaskId { get; set; }
    }
}