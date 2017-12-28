using System.Collections.Generic;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Passwords;
using Kengic.Was.Domain.Entity.AccessControl.FunctionPrivileges;

namespace Kengic.Was.Application.WasModel.Dto.AccessControls.Privileges
{
    public class FunctionPrivilegeDto : EntityForTimeDto<string>
    {
        public FunctionPrivilegeType FunObjType { get; set; }
        public int AccessModle { get; set; }
        public int FunObjParentId { get; set; }
        public string FunObjCatalog { get; set; }
        public string AuthorizationMask { get; set; }
        public int IsExtendAuthority { get; set; }
        public string ConditionExpression { get; set; }
        public string ParentId { get; set; }
        public string ParentFunctionPrivilegeName { get; set; }
        public IList<ForeignRole> Roles { get; set; }
        public IList<ForeignUser> Users { get; set; }
        public bool IsChecked { get; set; }
        public bool IsReadOnly { get; set; }
        public List<FunctionPrivilegeDto> Children { get; set; }
    }
}