namespace Kengic.Was.Application.WasModel.Dto.WorkTasks
{
    public class ExecuteWorkTaskDto<TKey> : WorkTaskDto<TKey>
    {
        public int CurrentSerialNumber { get; set; }
        public int SumSerialNumber { get; set; }
    }
}