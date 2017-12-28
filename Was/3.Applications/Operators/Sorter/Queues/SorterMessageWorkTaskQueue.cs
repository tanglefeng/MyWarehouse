using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;
using Kengic.Was.Domain.Entity.WorkTask;
using Kengic.Was.Operator.Common.Queues;

namespace Kengic.Was.Operator.Sorter.Queues
{
    public class SorterMessageWorkTaskQueue : SmartWorkTaskQueue<string, SorterMessageWorkTask>
    {
        public SorterMessageWorkTaskQueue(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork, e => e.Status < WorkTaskStatus.Completed)
        {
        }

        public List<SorterMessageWorkTask> GetWorkTaskForComplement() => Values.Where(
            e =>
                (e.TriggerMode == WorkTaskTriggerMode.Immediately) && (e.Status == WorkTaskStatus.Create) &&
                (e.WorkTaskType == WorkTaskType.Complement)).ToList();


        public SorterMessageWorkTask GetWorkTaskByTracingId(string tracingId) => Values.FirstOrDefault(
            e => e.TrackingId == tracingId);
    }
}