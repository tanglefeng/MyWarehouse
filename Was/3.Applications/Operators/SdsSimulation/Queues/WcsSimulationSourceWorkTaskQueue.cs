using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.SdsSimulation;
using Kengic.Was.Domain.Entity.WorkTask;
using Kengic.Was.Operator.Common.Queues;

namespace Kengic.Was.Operator.SdsSimulation.Queues
{
    public class SdsSimulationSourceWorkTaskQueue : SmartWorkTaskQueue<string, SdsSimulationSourceWorkTask>
    {
        public SdsSimulationSourceWorkTaskQueue(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork, e => e.Status < WorkTaskStatus.Completed)
        {
        }

        public List<SdsSimulationSourceWorkTask> GetUnfinishWorkTaskByExeId(string exeucteWorkTaskId) => Values.Where(
            e =>
                (e.ExecuteWorkTaskId == exeucteWorkTaskId) && ((int) e.Status < (int) WorkTaskStatus.Completed))
            .OrderBy(e => e.InternalPriority)
            .ToList();
    }
}