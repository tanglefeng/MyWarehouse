using System;
using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Application.Services.AccessControl.Companys;
using Kengic.Was.Application.Services.AccessControl.Departments;
using Kengic.Was.Application.Services.AccessControl.Passwords;
using Kengic.Was.Application.Services.AccessControl.Personnels;
using Kengic.Was.Application.Services.AccessControl.Privileges;
using Kengic.Was.Application.Services.AccessControl.Roles;
using Kengic.Was.Application.Services.AccessControl.Terminals;
using Kengic.Was.Application.Services.AccessControl.Users;
using Kengic.Was.Application.Services.AccessControl.Workgroups;
using Kengic.Was.Application.Services.SystemTracing;
using Kengic.Was.Application.WasModel.Common;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Passwords;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Personnels;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Privileges;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Terminals;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.DistributedServices.Common;
using Kengic.Was.Domain.Entity.AccessControl.Companys;
using Kengic.Was.Domain.Entity.AccessControl.Departments;
using Kengic.Was.Domain.Entity.AccessControl.FunctionPrivileges;
using Kengic.Was.Domain.Entity.AccessControl.Passwords;
using Kengic.Was.Domain.Entity.AccessControl.Personnels;
using Kengic.Was.Domain.Entity.AccessControl.Roles;
using Kengic.Was.Domain.Entity.AccessControl.Terminals;
using Kengic.Was.Domain.Entity.AccessControl.Users;
using Kengic.Was.Domain.Entity.AccessControl.Workgroups;
using Kengic.Was.Wcf.IAccessControl;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace Kengic.Was.Wcf.AccessControl
{
    [ExceptionShielding("WcfServicePolicy")]
    public class AccessControlService : IAccessControlService
    {
        private readonly ICompanyApplicationServices _companyApplicationServices;
        private readonly ICompanyRepository _companyRepository;
        private readonly IDepartmentApplicationServices _departmentApplicationServices;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IFunctionPrivilegeRepository _functionPrivilegeRepository;
        private readonly IOperationTracingApplicationService _operationTracingApplicationService;
        private readonly IPasswordApplicationServices _passwordApplicationServices;
        private readonly IPasswordRepository _passwordRepository;
        private readonly IPersonnelApplicationServices _personnelApplicationServices;
        private readonly IPersonnelRepository _personnelRepository;
        private readonly IPrivilegeApplicationServices _privilegeApplicationServices;
        private readonly IRoleApplicationServices _roleApplicationServices;
        private readonly IRoleRepository _roleRepository;
        private readonly ITerminalApplicationServices _terminalApplicationServices;
        private readonly ITerminalRepository _terminalRepository;
        private readonly IUserApplicationServices _userApplicationServices;
        private readonly IUserRepository _userRepository;
        private readonly IWorkgroupApplicationServices _workgroupApplicationServices;
        private readonly IWorkgroupRepository _workgroupRepository;

        public AccessControlService(ICompanyApplicationServices companyApplicationServices,
            ICompanyRepository companyRepository,
            IDepartmentApplicationServices departmentApplicationServices, IDepartmentRepository departmentRepository,
            IPersonnelApplicationServices personnelApplicationServices, IPersonnelRepository personnelRepository,
            IPrivilegeApplicationServices privilegeApplicationServices,
            IFunctionPrivilegeRepository functionPrivilegeRepository,
            IRoleApplicationServices roleApplicationServices, IRoleRepository roleRepository,
            IUserApplicationServices userApplicationServices, IUserRepository userRepository,
            IPasswordApplicationServices passwordApplicationServices, IPasswordRepository passwordRepository,
            ITerminalApplicationServices terminalApplicationServices, ITerminalRepository terminalRepository,
            IWorkgroupApplicationServices workgroupApplicationServices, IWorkgroupRepository workgroupRepository,
            IOperationTracingApplicationService operationTracingApplicationService)
        {
            if (companyApplicationServices == null)
            {
                throw new ArgumentNullException(nameof(companyApplicationServices));
            }
            if (departmentApplicationServices == null)
            {
                throw new ArgumentNullException(nameof(departmentApplicationServices));
            }

            if (passwordApplicationServices == null)
            {
                throw new ArgumentNullException(nameof(passwordApplicationServices));
            }
            if (personnelApplicationServices == null)
            {
                throw new ArgumentNullException(nameof(personnelApplicationServices));
            }
            if (privilegeApplicationServices == null)
            {
                throw new ArgumentNullException(nameof(privilegeApplicationServices));
            }
            if (roleApplicationServices == null)
            {
                throw new ArgumentNullException(nameof(roleApplicationServices));
            }
            if (terminalApplicationServices == null)
            {
                throw new ArgumentNullException(nameof(terminalApplicationServices));
            }
            if (userApplicationServices == null)
            {
                throw new ArgumentNullException(nameof(userApplicationServices));
            }
            if (workgroupApplicationServices == null)
            {
                throw new ArgumentNullException(nameof(workgroupApplicationServices));
            }
            if (operationTracingApplicationService == null)
            {
                throw new ArgumentNullException(nameof(operationTracingApplicationService));
            }

            _operationTracingApplicationService = operationTracingApplicationService;
            _companyApplicationServices = companyApplicationServices;
            _departmentApplicationServices = departmentApplicationServices;
            _passwordApplicationServices = passwordApplicationServices;
            _personnelApplicationServices = personnelApplicationServices;
            _privilegeApplicationServices = privilegeApplicationServices;
            _roleApplicationServices = roleApplicationServices;
            _terminalApplicationServices = terminalApplicationServices;
            _userApplicationServices = userApplicationServices;
            _workgroupApplicationServices = workgroupApplicationServices;
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _functionPrivilegeRepository = functionPrivilegeRepository;
            _passwordRepository = passwordRepository;
            _personnelRepository = personnelRepository;
            _roleRepository = roleRepository;
            _terminalRepository = terminalRepository;
            _userRepository = userRepository;
            _workgroupRepository = workgroupRepository;
        }

        public string GetDataFromCompany(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_companyRepository, dynamicQueryMethods);

        public string GetDataFromDepartment(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_departmentRepository, dynamicQueryMethods);

        public string GetDataFromFunctionPrivilege(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_functionPrivilegeRepository, dynamicQueryMethods);

        public string GetDataFromPassword(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_passwordRepository, dynamicQueryMethods);

        public string GetDataFromPersonnel(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_personnelRepository, dynamicQueryMethods);

        public string GetDataFromRole(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_roleRepository, dynamicQueryMethods);

        public string GetDataFromTerminal(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_terminalRepository, dynamicQueryMethods);

        public string GetDataFromUser(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_userRepository, dynamicQueryMethods);

        public string GetDataFromWorkgroup(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_workgroupRepository, dynamicQueryMethods);

        public Tuple<bool, string> CreateCompany(CompanyDto value)
        {
            var values = value.ProjectedAs<CompanyDto, Company>();
            var returnValue = _companyApplicationServices.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                GetType().Name, StaticParameterForMessage.Create,
                StaticParameterForMessage.Company, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveCompany(CompanyDto value)
        {
            var values = value.ProjectedAs<CompanyDto, Company>();
            var returnValue = _companyApplicationServices.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Remove,
                StaticParameterForMessage.Company, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateCompany(CompanyDto value)
        {
            var values = value.ProjectedAs<CompanyDto, Company>();
            var returnValue = _companyApplicationServices.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Update,
                StaticParameterForMessage.Company, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateDepartment(DepartmentDto value)
        {
            var values = value.ProjectedAs<DepartmentDto, Department>();
            var returnValue = _departmentApplicationServices.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                GetType().Name, StaticParameterForMessage.Create,
                StaticParameterForMessage.Department, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveDepartment(DepartmentDto value)
        {
            var values = value.ProjectedAs<DepartmentDto, Department>();
            var returnValue = _departmentApplicationServices.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Remove,
                StaticParameterForMessage.Department, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateDepartment(DepartmentDto value)
        {
            var values = value.ProjectedAs<DepartmentDto, Department>();
            var returnValue = _departmentApplicationServices.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Update,
                StaticParameterForMessage.Department, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreatePersonnel(PersonnelDto value)
        {
            var values = value.ProjectedAs<PersonnelDto, Personnel>();
            var returnValue = _personnelApplicationServices.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                GetType().Name, StaticParameterForMessage.Create,
                StaticParameterForMessage.Personnel, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemovePersonnel(PersonnelDto value)
        {
            var values = value.ProjectedAs<PersonnelDto, Personnel>();
            var returnValue = _personnelApplicationServices.Remove(values);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Remove,
                StaticParameterForMessage.Personnel, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdatePersonnel(PersonnelDto value)
        {
            var values = value.ProjectedAs<PersonnelDto, Personnel>();
            var returnValue = _personnelApplicationServices.Update(values);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Update,
                StaticParameterForMessage.Personnel, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateUser(UserDto value)
        {
            var values = value.ProjectedAs<UserDto, User>();
            var returnValue = _userApplicationServices.Create(values);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                GetType().Name, StaticParameterForMessage.Create,
                StaticParameterForMessage.User, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveUser(UserDto value)
        {
            var values = value.ProjectedAs<UserDto, User>();
            var returnValue = _userApplicationServices.Remove(values);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Remove,
                StaticParameterForMessage.User, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public List<RoleDto> GetRolesByUser(string userId)
        {
            var user = _userApplicationServices.GetUserById(userId);

            return user?.Roles?.ProjectedAsCollection<Role, RoleDto>();
        }

        public List<FunctionPrivilegeDto> GetPrivilegeByUser(string userId)
        {
            var user = _userApplicationServices.GetUserById(userId);
            return user?.FunctionPrivileges?.ProjectedAsCollection<FunctionPrivilege, FunctionPrivilegeDto>();
        }

        public Tuple<bool, string> UpdateUserRoles(string userId, string roleId)
        {
            var returnValue = _userApplicationServices.UpdateUserRoles(userId, roleId);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, userId,
                GetType().Name, StaticParameterForMessage.Update,
                StaticParameterForMessage.Role, userId, roleId, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveUserRoles(string userId, string roleId)
        {
            var returnValue = _userApplicationServices.RemoveUserRoles(userId, roleId);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, userId,
                GetType().Name, StaticParameterForMessage.Remove,
                StaticParameterForMessage.Role, userId, roleId, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateUserPrivileges(string userId, string privilegeId)
        {
            var returnValue = _userApplicationServices.UpdateUserPrivileges(userId, privilegeId);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, userId,
                GetType().Name, StaticParameterForMessage.Update,
                StaticParameterForMessage.Privilege, userId, privilegeId, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveUserPrivileges(string userId, string privilegeId)
        {
            var returnValue = _userApplicationServices.RemoveUserPrivileges(userId, privilegeId);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, userId,
                GetType().Name, StaticParameterForMessage.Remove,
                StaticParameterForMessage.Privilege, userId, privilegeId, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateUser(UserDto value)
        {
            var values = value.ProjectedAs<UserDto, User>();
            var returnValue = _userApplicationServices.Update(values);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Update,
                StaticParameterForMessage.User, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateRole(RoleDto value)
        {
            var values = value.ProjectedAs<RoleDto, Role>();
            var returnValue = _roleApplicationServices.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                GetType().Name, StaticParameterForMessage.Create,
                StaticParameterForMessage.Personnel, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateRole(RoleDto value)
        {
            var values = value.ProjectedAs<RoleDto, Role>();
            var returnValue = _roleApplicationServices.Update(values);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Update,
                StaticParameterForMessage.Personnel, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveRole(RoleDto value)
        {
            var values = value.ProjectedAs<RoleDto, Role>();
            var returnValue = _roleApplicationServices.Remove(values);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Remove,
                StaticParameterForMessage.Personnel, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateRolePrivileges(string userId, string roleId, string privileageId)
        {
            var returnValue = _roleApplicationServices.UpdateRolePrivilege(roleId, privileageId);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, userId,
                GetType().Name, StaticParameterForMessage.Update,
                StaticParameterForMessage.Privilege, roleId, privileageId, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveRolePrivileges(string userId, string roleId, string privileageId)
        {
            var returnValue = _roleApplicationServices.RemoveRolePrivilege(roleId, privileageId);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, userId,
                GetType().Name, StaticParameterForMessage.Remove,
                StaticParameterForMessage.Privilege, roleId, privileageId, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateFunctionPrivilege(FunctionPrivilegeDto value)
        {
            var values = value.ProjectedAs<FunctionPrivilegeDto, FunctionPrivilege>();
            var returnValue = _privilegeApplicationServices.Create(values);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                GetType().Name, StaticParameterForMessage.Create,
                StaticParameterForMessage.Privilege, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateFunctionPrivilege(FunctionPrivilegeDto value)
        {
            var values = value.ProjectedAs<FunctionPrivilegeDto, FunctionPrivilege>();
            var returnValue = _privilegeApplicationServices.Update(values);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Update,
                StaticParameterForMessage.Privilege, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveFunctionPrivilege(FunctionPrivilegeDto value)
        {
            var values = value.ProjectedAs<FunctionPrivilegeDto, FunctionPrivilege>();
            var returnValue = _privilegeApplicationServices.Remove(values);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Remove,
                StaticParameterForMessage.Privilege, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public IEnumerable<FunctionPrivilegeDto> GetUserPrivileges(string userId)
        {
            var user = _userRepository.GetWithRoles(userId);
            var firstOrDefault = user?.Roles?.FirstOrDefault();
            var functionPrivilegeDto =
                firstOrDefault?.FunctionPrivileges.ProjectedAsCollection<FunctionPrivilege, FunctionPrivilegeDto>();
            return functionPrivilegeDto;
        }

        public Tuple<bool, string> CreateTerminal(TerminalDto value)
        {
            var values = value.ProjectedAs<TerminalDto, Terminal>();
            var returnValue = _terminalApplicationServices.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                GetType().Name, StaticParameterForMessage.Create,
                StaticParameterForMessage.Terminal, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateTerminal(TerminalDto value)
        {
            var values = value.ProjectedAs<TerminalDto, Terminal>();
            var returnValue = _terminalApplicationServices.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Update,
                StaticParameterForMessage.Terminal, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveTerminal(TerminalDto value)
        {
            var values = value.ProjectedAs<TerminalDto, Terminal>();
            var returnValue = _terminalApplicationServices.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Remove,
                StaticParameterForMessage.Terminal, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateWorkgroup(WorkgroupDto value)
        {
            var values = value.ProjectedAs<WorkgroupDto, Workgroup>();
            var returnValue = _workgroupApplicationServices.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                GetType().Name, StaticParameterForMessage.Create,
                StaticParameterForMessage.Workgroup, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateWorkgroup(WorkgroupDto value)
        {
            var values = value.ProjectedAs<WorkgroupDto, Workgroup>();
            var returnValue = _workgroupApplicationServices.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Update,
                StaticParameterForMessage.Workgroup, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveWorkgroup(WorkgroupDto value)
        {
            var values = value.ProjectedAs<WorkgroupDto, Workgroup>();
            var returnValue = _workgroupApplicationServices.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Remove,
                StaticParameterForMessage.Workgroup, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreatePassword(PasswordDto value)
        {
            if (string.IsNullOrEmpty(value.Id))
            {
                value.Id = Guid.NewGuid().ToString("N");
            }
            var values = value.ProjectedAs<PasswordDto, Password>();

            var returnValue = _passwordApplicationServices.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                GetType().Name, StaticParameterForMessage.Create,
                StaticParameterForMessage.Password, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemovePassword(PasswordDto value)
        {
            var values = value.ProjectedAs<PasswordDto, Password>();
            var returnValue = _passwordApplicationServices.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Remove,
                StaticParameterForMessage.Password, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdatePassword(PasswordDto value)
        {
            var values = value.ProjectedAs<PasswordDto, Password>();
            var returnValue = _passwordApplicationServices.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                GetType().Name, StaticParameterForMessage.Update,
                StaticParameterForMessage.Password, value.Id, value.Name, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> Login(string userid, string password,
            int passwordtype, out UserDto userDto)
        {
            var response = _passwordApplicationServices.Login(userid, password, passwordtype);
            var flagSuccess = response.Item1;
            var messageCode = response.Item2;
            var user = response.Item3;
            userDto = user?.ProjectedAs<User, UserDto>();

            var operationTracing = OperationTracingHelper.GetOperationTracing(response.Item1, userid,
                StaticParameterForMessage.LoginIn,
                GetType().Name,
                StaticParameterForMessage.Password, userid, userid, "");
            _operationTracingApplicationService.Create(operationTracing);
            return new Tuple<bool, string>(flagSuccess, messageCode);
        }

        public Tuple<bool, string> LoginOut(string userId)
        {
            var response = _passwordApplicationServices.LoginOut(userId);
            var flagSuccess = response.Item1;
            var messageCode = response.Item2;

            var operationTracing = OperationTracingHelper.GetOperationTracing(response.Item1, userId,
                StaticParameterForMessage.LoginOut,
                GetType().Name,
                response.Item3.Id, response.Item3.Id, response.Item3.Name, "");
            _operationTracingApplicationService.Create(operationTracing);
            return new Tuple<bool, string>(flagSuccess, messageCode);
        }

        public Tuple<bool, string> ChangePassword(string userId, string originPassword, string newPassword)
        {
            var response = _passwordApplicationServices.ChangePassword(userId, originPassword, newPassword);
            var flagSuccess = response.Item1;
            var messageCode = response.Item2;

            var operationTracing = OperationTracingHelper.GetOperationTracing(response.Item1, userId,
                StaticParameterForMessage.ChangePassword,
                GetType().Name,
                userId, userId, newPassword, "");
            _operationTracingApplicationService.Create(operationTracing);
            return new Tuple<bool, string>(flagSuccess, messageCode);
        }

        public Tuple<bool, string> ResetPassword(string userId)
        {
            var response = _passwordApplicationServices.ResetPassword(userId);
            var flagSuccess = response.Item1;
            var messageCode = response.Item2;

            var operationTracing = OperationTracingHelper.GetOperationTracing(response.Item1, userId,
                StaticParameterForMessage.ResetPassword,
                GetType().Name,
                userId, userId, "new password is 123456", "");
            _operationTracingApplicationService.Create(operationTracing);
            return new Tuple<bool, string>(flagSuccess, messageCode);
        }

        public List<FunctionPrivilegeDto> GetRolePrivilege(string roleId)
        {
            var role = _roleApplicationServices.GetRole(roleId);

            return role.FunctionPrivileges.ProjectedAsCollection<FunctionPrivilege, FunctionPrivilegeDto>();
        }


        public Tuple<bool, string> AddRoleToUser(UserDto userDto, RoleDto roleDto)
        {
            var user = userDto.ProjectedAs<UserDto, User>();
            var role = roleDto.ProjectedAs<RoleDto, Role>();
            return _privilegeApplicationServices.AddRoleToUser(user, role);
        }

        public Tuple<bool, string> RemoveRoleFromUser(UserDto userDto, RoleDto roleDto)
        {
            var user = userDto.ProjectedAs<UserDto, User>();
            var role = roleDto.ProjectedAs<RoleDto, Role>();
            return _privilegeApplicationServices.RemoveRoleFromUser(user, role);
        }
    }
}