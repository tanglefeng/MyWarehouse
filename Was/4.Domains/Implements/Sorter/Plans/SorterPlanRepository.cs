using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Sorter.Plans;

namespace Kengic.Was.Domain.Sorter.Plans
{
    public class SorterPlanRepository : RepositoryForSyncDb<string, SorterPlan>, ISorterPlanRepository
    {
        public SorterPlanRepository(IQueryableUnitOfWork queryableUnitOfWork) : base(queryableUnitOfWork)
        {
        }

        public List<SorterPlan> GetValueByLogicalSorter(string value)
            => ValueQueue.Values.Where(e => e.LogicalSorter == value).ToList();

        public List<SorterPlan> GetActiveValue() => ValueQueue.Values.Where(e => e.IsActive).ToList();
    }
}