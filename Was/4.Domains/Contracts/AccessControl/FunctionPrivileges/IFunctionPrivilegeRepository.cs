using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.FunctionPrivileges
{
    public interface IFunctionPrivilegeRepository : IRepositoryForOnlyDb<string, FunctionPrivilege>
    {
        FunctionPrivilege GetWithParentFunctionPrivilege(string id);
    }
}