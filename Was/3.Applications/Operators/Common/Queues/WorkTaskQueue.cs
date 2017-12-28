using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.WorkTask;
using Kengic.Was.Domain.Entity.WorkTask.WorkTasks;

namespace Kengic.Was.Operator.Common.Queues
{
    public class WorkTaskQueue<TKey, TValue> : ConcurrentQueueSyncDb<TKey, TValue> where TValue : WorkTask<TKey>, new()
    {
        public WorkTaskQueue(IQueryableUnitOfWork unitOfWork, Expression<Func<TValue, bool>> filter)
            : base(unitOfWork, filter)
        {
        }

        public bool Create(TValue tValue, Func<TValue, bool> existFunction)
        {
            var existWorkTask = Values.Where(existFunction);

            if (existWorkTask.Any())
            {
                return false;
            }
            tValue.Status = WorkTaskStatus.Create;
            return TryAdd(tValue);
        }

        public bool Update(TKey tKey, Func<TValue, TValue> tNewValueFunction, Func<TValue, bool> validateFunction,
            List<string> modifiedProperties = null)
        {
            var tOldValue = ValidQueue(tKey, validateFunction);
            var newTValue = tNewValueFunction(tOldValue);
            return modifiedProperties == null ? TryUpdate(newTValue) : TryUpdate(newTValue, modifiedProperties);
        }

        public bool Remove(TKey tKey, Func<TValue, bool> validateFunction)
        {
            ValidQueue(tKey, validateFunction);
            return TryRemove(tKey);
        }

        public void Finish(TKey tKey, TValue tValue, Func<TValue, bool> existFunction)
        {
            var filterWorkTask = Values.Where(existFunction).ToList();

            if (!filterWorkTask.Any())
            {
                throw new Exception("Can't find WorkTask with filter");
            }

            foreach (var workTask in filterWorkTask)
            {
                workTask.CompleteTime = DateTime.Now;
                TryUpdate(tValue);
            }
        }

        public void Release(Func<TValue, bool> filterFunction)
        {
            var filterWorkTask = Values.Where(filterFunction).ToList();

            if (filterWorkTask.Any())
            {
                throw new Exception("Can't find WorkTask with filter");
            }
        }

        public void Terminate(TKey tKey, Func<TValue, bool> validateFunction)
        {
            var tValue = ValidQueue(tKey, validateFunction);
            tValue.TerminatedTime = DateTime.Now;
            TryUpdate(tValue);
        }

        protected TValue ValidQueue(TKey tKey, Func<TValue, bool> validateFunction)
        {
            var tValue = TryGetValue(tKey);
            if (tValue != null)
            {
                if (!validateFunction(tValue))
                {
                    throw new Exception("condiction of object is not valid");
                }
            }
            else
            {
                throw new Exception("Can't find object");
            }
            return tValue;
        }

        public List<TValue> GetWorkTaskForRelease() => Values.Where(
            e => (e.TriggerMode == WorkTaskTriggerMode.Immediately) &&
                 ((e.Status == WorkTaskStatus.Create) || (e.Status == WorkTaskStatus.Ready)))
            .OrderBy(e => e.InternalPriority)
            .ToList();

        public List<TValue> GetEventWorkTaskForRelease() => Values.Where(
            e => (e.TriggerMode == WorkTaskTriggerMode.Event) &&
                 ((e.Status == WorkTaskStatus.Create) || (e.Status == WorkTaskStatus.Ready)))
            .OrderBy(e => e.InternalPriority)
            .ToList();

        public List<TValue> GetActiveReleaseWorkTask(string objectToHandle) => Values.Where(
            e =>
                ((e.Status == WorkTaskStatus.Active) || (e.Status == WorkTaskStatus.Release)) &&
                (e.ObjectToHandle == objectToHandle)).OrderBy(e => e.InternalPriority).ToList();

        public List<TValue> GetActiveWorkTask(string objectToHandle) => Values.Where(
            e =>
                (e.Status == WorkTaskStatus.Active) &&
                (e.ObjectToHandle == objectToHandle)).OrderBy(e => e.InternalPriority).ToList();

        public List<TValue> GetReleaseWorkTask(string objectToHandle) => Values.Where(
            e =>
                (e.Status == WorkTaskStatus.Release) &&
                (e.ObjectToHandle == objectToHandle)).OrderBy(e => e.InternalPriority).ToList();

        public List<TValue> GetUnFinishWorkTask(string objectToHandle) => Values.Where(
            e =>
                (e.Status < WorkTaskStatus.Completed) &&
                (e.ObjectToHandle == objectToHandle)).OrderBy(e => e.InternalPriority).ToList();

        public List<TValue> GetReadyWorkTask() => Values.Where(
            e =>
                e.Status == WorkTaskStatus.Ready).OrderBy(e => e.InternalPriority).ToList();

        public List<TValue> GetCreateWorkTask() => Values.Where(
            e =>
                e.Status == WorkTaskStatus.Create).OrderBy(e => e.InternalPriority).ToList();

        public List<TValue> GetReleaseWorkTask() => Values.Where(
            e =>
                e.Status == WorkTaskStatus.Release).OrderBy(e => e.InternalPriority).ToList();
    }
}