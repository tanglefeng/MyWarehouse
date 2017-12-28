using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.Personnels
{
    public interface IPersonnelRepository : IRepositoryForOnlyDb<string, Personnel>
    {
        Personnel GetWithAll(string id);
    }
}