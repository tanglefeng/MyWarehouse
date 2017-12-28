using System;
using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.AccessControl.Users;

namespace Kengic.Was.Application.Services.AccessControl.Users
{
    public interface IUserApplicationServices :
        IEditApplicationService<User>, IQueryApplicationService<User>
    {
        User GetUserById(string userId);
        Tuple<bool, string> UpdateUserRoles(string userId, string roleId);
        Tuple<bool, string> RemoveUserRoles(string userId, string roleId);

        Tuple<bool, string> UpdateUserPrivileges(string userId, string privilegeId);

        Tuple<bool, string> RemoveUserPrivileges(string userId, string privilegeId);
    }
}