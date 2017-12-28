using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.AccessControl.Passwords;

namespace Kengic.Was.Domain.AccessControl.Passwords
{
    public class PasswordRepository : RepositoryForOnlyDb<string, Password>, IPasswordRepository
    {
        public PasswordRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public Password GetWithUserAndType(string userid, int passwordtype)
        {
            if (userid != null)
            {
                return
                    GetSet()
                        .FirstOrDefault(r => (r.PasswordType == (PasswordType) passwordtype) && (r.UserId == userid));
            }
            return null;
        }
    }
}