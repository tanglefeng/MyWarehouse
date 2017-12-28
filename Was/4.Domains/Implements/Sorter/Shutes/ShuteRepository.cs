using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Sorter.Shutes;

namespace Kengic.Was.Domain.Sorter.Shutes
{
    public class ShuteRepository : RepositoryForSyncDb<string, Shute>, IShuteRepository
    {
        public ShuteRepository(IQueryableUnitOfWork queryableUnitOfWork) : base(queryableUnitOfWork)
        {
        }

        public List<Shute> GetValueByPhysicalSorter(string value)
            => ValueQueue.Values.Where(e => e.PhycialSorter == value).ToList();

        public List<Shute> GetValueByLogicalSorter(string value)
            => ValueQueue.Values.Where(e => e.LogicalSorter == value).ToList();

        public List<Shute> GetValueByPrintId(string value) => ValueQueue.Values.Where(e => e.PrintId == value).ToList();

        public Shute GetValueByDeviceName(string value)
        {
            var valueList = ValueQueue.Values.Where(e => e.DeviceName1 == value).ToList();
            return valueList.Count > 0 ? valueList[0] : null;
        }

        public List<Shute> GetValueByType(string value) => ValueQueue.Values.Where(e => e.ShuteType == value).ToList();
    }
}