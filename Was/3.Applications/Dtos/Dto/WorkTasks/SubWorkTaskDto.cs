namespace Kengic.Was.Application.WasModel.Dto.WorkTasks
{
    public class SubWorkTaskDto<TKey> : WorkTaskDto<TKey>
    {
        public int SerialNumber { get; set; }
        public int ParallelNumber { get; set; }
        public string ExecuteWorkTaskId { get; set; }
    }
}