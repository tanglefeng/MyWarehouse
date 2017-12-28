using System;
using System.Collections.Generic;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.Users
{
    public interface IUserRepository : IRepositoryForOnlyDb<string, User>
    {
        User TryGetName(string name);
        User GetWithRoles(string id);
        User GetWithFunctionPrivileges(string id);
        User GetWithTerminals(string id);
        User GetWithWorkgroups(string id);
        User GetWithPersonnel(string id);
        User GetWithPasswords(string id);
        User GetAllWithId(string id);
        IEnumerable<User> GetWithAll();
    }
}