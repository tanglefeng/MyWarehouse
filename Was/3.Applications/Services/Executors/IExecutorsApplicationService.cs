using System.Collections.Generic;
using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.Executor;

namespace Kengic.Was.Application.Services.Executors
{
    public interface IExecutorsApplicationService :
        IEditApplicationService<WasExecutor>, IQueryApplicationService<WasExecutor>
    {
        List<WasExecutor> GetValueByOperator(string value);
    }
}