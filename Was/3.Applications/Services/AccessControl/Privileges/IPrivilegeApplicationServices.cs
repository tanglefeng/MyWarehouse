using System;
using System.Collections.Generic;
using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.AccessControl.FunctionPrivileges;
using Kengic.Was.Domain.Entity.AccessControl.Roles;
using Kengic.Was.Domain.Entity.AccessControl.Users;

namespace Kengic.Was.Application.Services.AccessControl.Privileges
{
    public interface IPrivilegeApplicationServices :
        IEditApplicationService<FunctionPrivilege>, IQueryApplicationService<FunctionPrivilege>
    {
        IEnumerable<FunctionPrivilege> GetChildrenValue(FunctionPrivilege functionprivilege);
        IEnumerable<Role> GetRoleListAboutUser(User user);
        Tuple<bool, string> AddRoleToUser(User user, Role role);
        Tuple<bool, string> RemoveRoleFromUser(User user, Role role);
        IEnumerable<FunctionPrivilege> GetUserPrivileges(string userId);
    }
}