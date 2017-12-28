using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.SystemTracing;

namespace Kengic.Was.Application.Services.SystemTracing
{
    public interface IOperationTracingApplicationService :
        IEditApplicationService<OperationTracing>, IQueryApplicationService<OperationTracing>
    {
    }
}