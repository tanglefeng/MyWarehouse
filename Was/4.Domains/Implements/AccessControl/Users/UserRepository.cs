using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.AccessControl.Users;

namespace Kengic.Was.Domain.AccessControl.Users
{
    public class UserRepository : RepositoryForOnlyDb<string, User>, IUserRepository
    {
        public UserRepository(IQueryableUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public User TryGetName(string name) => name != null ? GetSet().FirstOrDefault(r => r.Name == name) : null;

        public User GetWithRoles(string id)
            => id != null ? GetSet().Where(r => r.Id == id).Include(r => r.Roles.Select(v=>v.FunctionPrivileges)).FirstOrDefault() : null;

        public User GetWithFunctionPrivileges(string id)
            => id != null ? GetSet().Where(r => r.Id == id).Include(r => r.FunctionPrivileges).FirstOrDefault() : null;

        public User GetWithTerminals(string id)
            => id != null ? GetSet().Where(r => r.Id == id).Include(r => r.Terminals).FirstOrDefault() : null;

        public User GetWithWorkgroups(string id)
            => id != null ? GetSet().Where(r => r.Id == id).Include(r => r.Workgroups).FirstOrDefault() : null;

        public User GetWithPersonnel(string id)
            => id != null ? GetSet().Where(r => r.Id == id).Include(r => r.Personnel).FirstOrDefault() : null;

        public User GetWithPasswords(string id)
            => id != null ? GetSet().Where(r => r.Id == id).Include(r => r.Passwords).FirstOrDefault() : null;

        public IEnumerable<User> GetWithAll()
            => GetSet().Include(r => r.Roles).Include(r => r.FunctionPrivileges).Include(r => r.Terminals)
                .Include(r => r.Workgroups).Include(r => r.Personnel).Include(r => r.Passwords);

        public User GetAllWithId(string id)
            =>
                id != null
                    ? GetSet()
                        .Where(r => r.Id == id)
                        .Include(r => r.Roles)
                        .Include(r => r.FunctionPrivileges)
                        .Include(r => r.Terminals)
                        .Include(r => r.Workgroups).Include(r => r.Personnel).Include(r => r.Passwords).FirstOrDefault()
                    : null;
    }
}