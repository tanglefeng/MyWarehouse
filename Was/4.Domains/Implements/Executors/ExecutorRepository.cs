using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Executor;

namespace Kengic.Was.Domain.Executors
{
    public class WasExecutorRepository : RepositoryForSyncDb<string, WasExecutor>, IWasExecutorRepository
    {
        public WasExecutorRepository(IQueryableUnitOfWork queryableUnitOfWork) : base(queryableUnitOfWork)
        {
        }

        public List<WasExecutor> GetValueByOperator(string executorOperator)
            => ValueQueue.Values.Where(e => e.ExecuteOperator == executorOperator).ToList();
    }
}