using System.Collections.Generic;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Sorter.Scanners
{
    public interface IScannerRepository : IRepositoryForSyncDb<string, Scanner>
    {
        List<Scanner> GetValueByPhysicalSorter(string value);
        List<Scanner> GetValueByLogicalSorter(string value);
    }
}