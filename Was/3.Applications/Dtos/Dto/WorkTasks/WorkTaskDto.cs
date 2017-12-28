using System;
using Kengic.Was.Domain.Entity.WorkTask;

namespace Kengic.Was.Application.WasModel.Dto.WorkTasks
{
    public class WorkTaskDto<TKey> : EntityForTimeDto<TKey>
    {
        public string OperatorName { get; set; }
        public string ObjectToHandle { get; set; }
        public WorkTaskType WorkTaskType { get; set; }
        public WorkTaskStatus Status { get; set; }
        public WorkTaskStatus SourceStatus { get; set; }
        public int? InternalPriority { get; set; }
        public int? ExternalPriority { get; set; }
        public WorkTaskTriggerMode TriggerMode { get; set; }
        public WorkTaskTriggerMode SourceTriggerMode { get; set; }
        public DateTime? TriggerFixedTime { get; set; }
        public int? TriggerIntervalTime { get; set; }
        public string ReadyBy { get; set; }
        public DateTime? ReadyTime { get; set; }
        public string ReleaseBy { get; set; }
        public DateTime? ReleaseTime { get; set; }
        public string ActiveBy { get; set; }
        public DateTime? ActiveTime { get; set; }
        public string CancelledBy { get; set; }
        public DateTime? CancelledTime { get; set; }
        public string TerminatedBy { get; set; }
        public DateTime? TerminatedTime { get; set; }
        public string SuspendedBy { get; set; }
        public DateTime? SuspendedTime { get; set; }
        public string ResumeBy { get; set; }
        public DateTime? ResumeTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        public DateTime? FaultTime { get; set; }
        public DateTime? TimeOutTime { get; set; }
        public string Comments { get; set; }
        public string Results { get; set; }
    }
}