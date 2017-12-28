using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.SdsSimulation;

namespace Kengic.Was.Domain.SdsSimulation
{
    public class SdsSimulationSourceWorkTaskRepository : RepositoryForOnlyDb<string, SdsSimulationSourceWorkTask>,
        ISdsSimulationSourceWorkTaskRepository
    {
        public SdsSimulationSourceWorkTaskRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}