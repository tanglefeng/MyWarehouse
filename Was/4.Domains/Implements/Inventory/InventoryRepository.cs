using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Inventory;

namespace Kengic.Was.Domain.Inventory
{
    public class InventoryRepository : RepositoryForOnlyDb<string, Entity.Inventory.Inventory>, IInventoryRepository
    {
        public InventoryRepository(IQueryableUnitOfWork queryableUnitOfWork) : base(queryableUnitOfWork)
        {
        }
    }
}