using System;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.Domain.Entity.Common;
using Newtonsoft.Json.Linq;

namespace Kengic.Was.Domain.Entity.WorkTask.WorkTasks
{
    public class WorkTask<TKey> : EntityForTime<TKey>
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

        public virtual void FormateWorkTask(JObject jObject)
        {
            Id = JSonHelper.GetValue<TKey>(jObject, StaticParameterForWorkTask.Id);
            OperatorName = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.OperatorName);
            ObjectToHandle = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.ObjectToHandle);
            Status = (WorkTaskStatus) JSonHelper.GetValue<int>(jObject, StaticParameterForWorkTask.Status);
            SourceStatus =
                (WorkTaskStatus) JSonHelper.GetValue<int>(jObject, StaticParameterForWorkTask.Status);
            ExternalPriority = JSonHelper.GetValue<int>(jObject, StaticParameterForWorkTask.ExternalPriority);
            TriggerMode =
                (WorkTaskTriggerMode) JSonHelper.GetValue<int>(jObject, StaticParameterForWorkTask.TriggerMode);
            SourceTriggerMode =
                (WorkTaskTriggerMode) JSonHelper.GetValue<int>(jObject, StaticParameterForWorkTask.TriggerMode);
            CreateBy = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.CreateBy);
            ReadyBy = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.ReadyBy);
            ReleaseBy = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.ReleaseBy);
            CancelledBy = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.CancelledBy);
            TerminatedBy = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.TerminatedBy);
            Comments = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Comments);
        }
    }
}