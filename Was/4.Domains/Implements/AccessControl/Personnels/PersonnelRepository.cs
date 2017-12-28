using System.Data.Entity;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.AccessControl.Personnels;

namespace Kengic.Was.Domain.AccessControl.Personnels
{
    public class PersonnelRepository : RepositoryForOnlyDb<string, Personnel>, IPersonnelRepository
    {
        public PersonnelRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public Personnel GetWithAll(string id)
            =>
                id != null
                    ? GetSet().Where(r => r.Id == id).Include(r => r.Department).FirstOrDefault()
                    : null;
    }
}