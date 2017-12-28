using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Sorter.Inducts;

namespace Kengic.Was.Domain.Sorter.Inducts
{
    public class InductRepository : RepositoryForSyncDb<string, Induct>, IInductRepository
    {
        public InductRepository(IQueryableUnitOfWork queryableUnitOfWork) : base(queryableUnitOfWork)
        {
        }

        public List<Induct> GetValueByPhysicalSorter(string value)
            => ValueQueue.Values.Where(e => e.PhycialSorter == value).ToList();

        public List<Induct> GetValueByLogicalSorter(string value)
            => ValueQueue.Values.Where(e => e.LogicalSorter == value).ToList();
    }
}