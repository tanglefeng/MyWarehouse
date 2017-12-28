using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.Passwords
{
    public interface IPasswordRepository : IRepositoryForOnlyDb<string, Password>
    {
        Password GetWithUserAndType(string userid, int passwordtype);
    }
}