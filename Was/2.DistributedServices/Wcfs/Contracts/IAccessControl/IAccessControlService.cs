using System;
using System.Collections.Generic;
using System.ServiceModel;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Passwords;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Personnels;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Privileges;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Terminals;
using Kengic.Was.DistributedServices.Common.ExceptionHandings;

namespace Kengic.Was.Wcf.IAccessControl
{
    [ServiceContract]
    [StandardFaults]
    public interface IAccessControlService
    {
        [OperationContract]
        string GetDataFromCompany(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromDepartment(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromFunctionPrivilege(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromPassword(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromPersonnel(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromRole(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromTerminal(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromUser(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromWorkgroup(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        Tuple<bool, string> CreateCompany(CompanyDto value);

        [OperationContract]
        Tuple<bool, string> RemoveCompany(CompanyDto value);

        [OperationContract]
        Tuple<bool, string> UpdateCompany(CompanyDto value);

        [OperationContract]
        Tuple<bool, string> CreateDepartment(DepartmentDto value);

        [OperationContract]
        Tuple<bool, string> RemoveDepartment(DepartmentDto value);

        [OperationContract]
        Tuple<bool, string> UpdateDepartment(DepartmentDto value);

        [OperationContract]
        Tuple<bool, string> CreatePersonnel(PersonnelDto value);

        [OperationContract]
        Tuple<bool, string> RemovePersonnel(PersonnelDto value);

        [OperationContract]
        Tuple<bool, string> UpdatePersonnel(PersonnelDto value);

        [OperationContract]
        Tuple<bool, string> CreateUser(UserDto value);

        [OperationContract]
        Tuple<bool, string> UpdateUser(UserDto userDto);

        [OperationContract]
        Tuple<bool, string> RemoveUser(UserDto userDto);

        [OperationContract]
        List<RoleDto> GetRolesByUser(string userId);

        [OperationContract]
        List<FunctionPrivilegeDto> GetPrivilegeByUser(string userId);

        [OperationContract]
        Tuple<bool, string> UpdateUserRoles(string userId, string roleId);

        [OperationContract]
        Tuple<bool, string> RemoveUserRoles(string userId, string roleId);

        [OperationContract]
        Tuple<bool, string> UpdateUserPrivileges(string userId, string privilegeId);

        [OperationContract]
        Tuple<bool, string> RemoveUserPrivileges(string userId, string privilegeId);

        [OperationContract]
        Tuple<bool, string> CreateRole(RoleDto value);

        [OperationContract]
        Tuple<bool, string> UpdateRole(RoleDto roleDto);

        [OperationContract]
        Tuple<bool, string> RemoveRole(RoleDto roleDto);

        [OperationContract]
        Tuple<bool, string> UpdateRolePrivileges(string userId, string roleId, string privileageId);

        [OperationContract]
        Tuple<bool, string> RemoveRolePrivileges(string userId, string roleId, string privileageId);

        [OperationContract]
        Tuple<bool, string> CreateFunctionPrivilege(FunctionPrivilegeDto functionPrivilegeDto);

        [OperationContract]
        Tuple<bool, string> UpdateFunctionPrivilege(FunctionPrivilegeDto functionPrivilegeDto);

        [OperationContract]
        Tuple<bool, string> RemoveFunctionPrivilege(FunctionPrivilegeDto functionPrivilegeDto);

        [OperationContract]
        IEnumerable<FunctionPrivilegeDto> GetUserPrivileges(string userId);

        [OperationContract]
        Tuple<bool, string> CreateTerminal(TerminalDto terminalDto);

        [OperationContract]
        Tuple<bool, string> UpdateTerminal(TerminalDto terminalDto);

        [OperationContract]
        Tuple<bool, string> RemoveTerminal(TerminalDto terminalDto);

        [OperationContract]
        Tuple<bool, string> CreateWorkgroup(WorkgroupDto workgroupDto);

        [OperationContract]
        Tuple<bool, string> UpdateWorkgroup(WorkgroupDto workgroupDto);

        [OperationContract]
        Tuple<bool, string> RemoveWorkgroup(WorkgroupDto workgroupDto);

        [OperationContract]
        Tuple<bool, string> CreatePassword(PasswordDto passwordDto);

        [OperationContract]
        Tuple<bool, string> RemovePassword(PasswordDto passwordDto);

        [OperationContract]
        Tuple<bool, string> UpdatePassword(PasswordDto passwordDto);

        [OperationContract]
        Tuple<bool, string> Login(string username, string password, int passwordtype,
            out UserDto usreDto);

        [OperationContract]
        Tuple<bool, string> LoginOut(string userId);

        [OperationContract]
        Tuple<bool, string> ChangePassword(string userId, string originPassword, string newPassword);

        [OperationContract]
        Tuple<bool, string> ResetPassword(string userId);

        [OperationContract]
        List<FunctionPrivilegeDto> GetRolePrivilege(string roleId);

        [OperationContract]
        Tuple<bool, string> AddRoleToUser(UserDto userDto, RoleDto roleDto);

        [OperationContract]
        Tuple<bool, string> RemoveRoleFromUser(UserDto userDto, RoleDto roleDto);
    }
}