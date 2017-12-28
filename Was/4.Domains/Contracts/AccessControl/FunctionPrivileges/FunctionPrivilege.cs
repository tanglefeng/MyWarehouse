using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.FunctionPrivileges
{
    public class FunctionPrivilege : EntityForTime<string>
    {
        public FunctionPrivilegeType FunObjType { get; set; }
        public int AccessModle { get; set; }
        public int FunObjParentId { get; set; }
        public string FunObjCatalog { get; set; }
        public string AuthorizationMask { get; set; }
        public int IsExtendAuthority { get; set; }
        public string ConditionExpression { get; set; }
        public virtual FunctionPrivilege Parent { get; set; }
        public string ParentId { get; set; }
    }

    public enum FunctionPrivilegeType
    {
        Menu,
        MenuItem,
        Window,
        Page,
        Button,
        Field,
        Function
    }
}