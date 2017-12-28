using System.Collections.Generic;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Sorter.Plans
{
    public interface ISorterPlanRepository : IRepositoryForSyncDb<string, SorterPlan>
    {
        List<SorterPlan> GetValueByLogicalSorter(string value);
        List<SorterPlan> GetActiveValue();
    }
}