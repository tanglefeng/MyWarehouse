using System.Collections.Generic;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Executor
{
    public interface IWasExecutorRepository : IRepositoryForSyncDb<string, WasExecutor>
    {
        List<WasExecutor> GetValueByOperator(string executorOperator);
    }
}