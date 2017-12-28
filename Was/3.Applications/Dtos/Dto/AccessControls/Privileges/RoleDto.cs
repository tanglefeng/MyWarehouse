using System.Collections.Generic;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Passwords;

namespace Kengic.Was.Application.WasModel.Dto.AccessControls.Privileges
{
    public class RoleDto : EntityForTimeDto<string>
    {
        public IList<ForeignFunctionPrivilege> FunctionPrivileges { get; set; }
        public IList<ForeignUser> Users { get; set; }
        public bool IsChecked { get; set; }
        public bool IsReadOnly { get; set; }
    }
}