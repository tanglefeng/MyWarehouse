using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Sorter.Scanners;

namespace Kengic.Was.Domain.Sorter.Scanners
{
    public class ScannerRepository : RepositoryForSyncDb<string, Scanner>, IScannerRepository
    {
        public ScannerRepository(IQueryableUnitOfWork queryableUnitOfWork) : base(queryableUnitOfWork)
        {
        }

        public List<Scanner> GetValueByPhysicalSorter(string value)
            => ValueQueue.Values.Where(e => e.PhycialSorter == value).ToList();

        public List<Scanner> GetValueByLogicalSorter(string value)
            => ValueQueue.Values.Where(e => e.LogicalSorter == value).ToList();
    }
}