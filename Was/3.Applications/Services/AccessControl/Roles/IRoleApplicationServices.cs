using System;
using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.AccessControl.Roles;

namespace Kengic.Was.Application.Services.AccessControl.Roles
{
    public interface IRoleApplicationServices : IEditApplicationService<Role>, IQueryApplicationService<Role>
    {
        Role GetRole(string id);

        Tuple<bool, string> UpdateRolePrivilege(string roleId, string privileageId);


        Tuple<bool, string> RemoveRolePrivilege(string roleId, string privileageId);
    }
}