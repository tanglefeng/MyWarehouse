using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.SystemTracing;

namespace Kengic.Was.Domain.SystemTracing
{
    public class OperationTracingRepository : RepositoryForOnlyDb<string, OperationTracing>,
        IOperationTracingRepository
    {
        public OperationTracingRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}