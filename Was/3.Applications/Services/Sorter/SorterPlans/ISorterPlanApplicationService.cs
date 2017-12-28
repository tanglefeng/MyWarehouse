using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.Sorter.Plans;

namespace Kengic.Was.Application.Services.Sorter.SorterPlans
{
    public interface ISorterPlanApplicationService :
        IEditApplicationService<SorterPlan>, IQueryApplicationService<SorterPlan>
    {
    }
}