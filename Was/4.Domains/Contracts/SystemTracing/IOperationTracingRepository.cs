using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.SystemTracing
{
    public interface IOperationTracingRepository : IRepositoryForOnlyDb<string, OperationTracing>
    {
    }
}