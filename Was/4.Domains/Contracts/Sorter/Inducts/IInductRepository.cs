using System.Collections.Generic;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Sorter.Inducts
{
    public interface IInductRepository : IRepositoryForSyncDb<string, Induct>
    {
        List<Induct> GetValueByPhysicalSorter(string value);
        List<Induct> GetValueByLogicalSorter(string value);
    }
}