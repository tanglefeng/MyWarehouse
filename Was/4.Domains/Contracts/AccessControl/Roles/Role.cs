using System.Collections.Generic;
using Kengic.Was.Domain.Entity.AccessControl.FunctionPrivileges;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.Roles
{
    public class Role : EntityForTime<string>
    {
        public virtual ICollection<FunctionPrivilege> FunctionPrivileges { get; set; }
    }
}