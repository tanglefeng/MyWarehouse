using System.Collections.Generic;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Sorter.Shutes
{
    public interface IShuteRepository : IRepositoryForSyncDb<string, Shute>
    {
        List<Shute> GetValueByPhysicalSorter(string value);
        List<Shute> GetValueByLogicalSorter(string value);
        List<Shute> GetValueByPrintId(string value);
        Shute GetValueByDeviceName(string value);
        List<Shute> GetValueByType(string value);
    }
}