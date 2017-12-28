using System;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.Roles
{
    public interface IRoleRepository : IRepositoryForOnlyDb<string, Role>
    {
        Role GetValues(string id);
    }
}