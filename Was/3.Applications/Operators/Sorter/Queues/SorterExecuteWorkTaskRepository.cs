using System;
using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;
using Kengic.Was.Domain.Entity.WorkTask;
using Kengic.Was.Operator.Common.Repositorys;

namespace Kengic.Was.Operator.Sorter.Queues
{
    public class SorterExecuteWorkTaskRepository : WorkTaskRepository<string, SorterExecuteWorkTask>
    {
        public SorterExecuteWorkTaskRepository(IQueryableUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public List<SorterExecuteWorkTask> GetValueByObject(string objectToHandle)
            => GetFiltered(r => r.ObjectToHandle == objectToHandle).OrderByDescending(r=>r.CreateTime).ToList();

        public List<SorterExecuteWorkTask> GetValueByLatestObject(string objectToHandle)
            => GetFiltered(r => r.ObjectToHandle == objectToHandle).OrderByDescending(e => e.CreateTime).ToList();

        public List<SorterExecuteWorkTask> GetTimeOutWorkTask(double timeoutDay)
        {
            var timeoutDateTime = DateTime.Now.AddDays(-timeoutDay);
            return GetFiltered(r => (r.Status < WorkTaskStatus.Completed) && (r.CreateTime < timeoutDateTime))
                .ToList();
        }
    }
}