using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;
using Kengic.Was.Domain.Entity.WorkTask;
using Kengic.Was.Operator.Common.Queues;

namespace Kengic.Was.Operator.Sorter.Queues
{
    public class SorterSubWorkTaskQueue : SmartWorkTaskQueue<string, SorterSubWorkTask>
    {
        public SorterSubWorkTaskQueue(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork, e => e.Status < WorkTaskStatus.Completed)
        {
        }

        public SorterSubWorkTaskQueue(IQueryableUnitOfWork unitOfWork, Expression<Func<SorterSubWorkTask, bool>> filter)
            : base(unitOfWork, filter)
        {
        }

        public List<SorterSubWorkTask> GetWorkTaskForReady() => Values.Where(
            e =>
                (e.TriggerMode == WorkTaskTriggerMode.Immediately) && (e.Status == WorkTaskStatus.Create))
            .OrderBy(e => e.SerialNumber)
            .ToList();

        public List<SorterSubWorkTask> GetWorkTaskForComplement(string traceId, string barcode) => Values.Where(
            e =>
                (e.TriggerMode == WorkTaskTriggerMode.Event) && (e.Status == WorkTaskStatus.Create) &&
                (e.TrackingId == traceId) && (e.ObjectToHandle == barcode) &&
                (e.WorkTaskType == WorkTaskType.Complement)).OrderBy(e => e.SerialNumber).ToList();

        public List<SorterSubWorkTask> GetWorkTaskForComplement(string traceId) => Values.Where(
            e =>
                (e.TriggerMode == WorkTaskTriggerMode.Event) && (e.Status == WorkTaskStatus.Create) &&
                (e.TrackingId == traceId) &&
                (e.WorkTaskType == WorkTaskType.Complement)).OrderBy(e => e.SerialNumber).ToList();


        public List<SorterSubWorkTask> GetUnfinishWorkTaskByExeId(string executeWorkTaskId) => Values.Where(
            e =>
                (e.TriggerMode == WorkTaskTriggerMode.Immediately) &&
                ((int) e.Status < (int) WorkTaskStatus.Completed) &&
                (e.ExecuteWorkTaskId == executeWorkTaskId)).OrderBy(e => e.SerialNumber).ToList();

        public List<SorterSubWorkTask> GetUnfinishWorkTaskByTrackId(string trackingId) => Values.Where(
            e =>
                ((int) e.Status < (int) WorkTaskStatus.Completed) &&
                (e.TrackingId == trackingId)).OrderByDescending(e => e.CreateTime).ToList();


        public List<SorterSubWorkTask> GetWorkTaskForReleaseByLogicalSortter(string logicalSortter) => Values.Where(
            e => (e.Status == WorkTaskStatus.Ready) &&
                 (e.LogicalSortter == logicalSortter))
            .OrderBy(e => e.InternalPriority)
            .ToList();

        public List<SorterSubWorkTask> GetTimeOutWorkTask(int timeoutSecond) => Values.Where(
            e =>
                ((int) e.Status < (int) WorkTaskStatus.Completed) &&
                (e.CreateTime < DateTime.Now.AddSeconds(-timeoutSecond))).ToList();
    }
}