using AutoMapper;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Passwords;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Personnels;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Privileges;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Terminals;
using Kengic.Was.Domain.Entity.AccessControl.Companys;
using Kengic.Was.Domain.Entity.AccessControl.Departments;
using Kengic.Was.Domain.Entity.AccessControl.FunctionPrivileges;
using Kengic.Was.Domain.Entity.AccessControl.Passwords;
using Kengic.Was.Domain.Entity.AccessControl.Personnels;
using Kengic.Was.Domain.Entity.AccessControl.Roles;
using Kengic.Was.Domain.Entity.AccessControl.Terminals;
using Kengic.Was.Domain.Entity.AccessControl.Users;
using Kengic.Was.Domain.Entity.AccessControl.Workgroups;

namespace Kengic.Was.Application.WasModel.Dto.Profiles
{
    internal class AccessControlProfile
        : Profile
    {
        protected override void Configure()
        {
            #region was -> wpf

            //company
            CreateMap<Company, CompanyDto>()
                .ForMember(cv => cv.ParentCompanyName,
                    cp => cp.MapFrom(cv => cv.Parent == null ? null : cv.Parent.Name));
            //department
            CreateMap<Department, DepartmentDto>()
                .ForMember(cv => cv.ParentDepartmentName,
                    cp => cp.MapFrom(cv => cv.Parent == null ? null : cv.Parent.Name))
                .ForMember(cv => cv.CompanyName,
                    cp => cp.MapFrom(cv => cv.Company == null ? null : cv.Company.Name));
            //personnel
            CreateMap<Personnel, PersonnelDto>()
                .ForMember(cv => cv.DepartmentName,
                    cp => cp.MapFrom(cv => cv.Department == null ? null : cv.Department.Name));

            //user
            CreateMap<User, UserDto>()
                .ForMember(cv => cv.PersonnelName,
                    cp => cp.MapFrom(cv => cv.Personnel == null ? null : cv.Personnel.ChineseName));
            //password
            CreateMap<Password, PasswordDto>()
                .ForMember(cv => cv.UserName,
                    cp => cp.MapFrom(cv => cv.User == null ? null : cv.User.Name));
            //role
            CreateMap<Role, RoleDto>();
            //FunctionPrivilege
            CreateMap<FunctionPrivilege, FunctionPrivilegeDto>()
                .ForMember(cv => cv.ParentFunctionPrivilegeName,
                    cp =>
                        cp.MapFrom(cv => cv.Parent == null ? null : cv.Parent.Name));
            //terminal
            CreateMap<Terminal, TerminalDto>();
            //workgroup
            CreateMap<Workgroup, WorkgroupDto>();

            #endregion

            #region was -> wpf(Foreign)

            CreateMap<User, ForeignUser>();
            CreateMap<Password, ForeginPassword>();
            CreateMap<Role, ForeignRole>();
            CreateMap<FunctionPrivilege, ForeignFunctionPrivilege>();
            CreateMap<Terminal, ForeignTerminal>();
            CreateMap<Workgroup, ForeignWorkgroup>();

            #endregion

            #region wpf -> was

            CreateMap<CompanyDto, Company>();
            CreateMap<DepartmentDto, Department>();
            CreateMap<PersonnelDto, Personnel>();
            CreateMap<UserDto, User>();
            CreateMap<PasswordDto, Password>();
            CreateMap<RoleDto, Role>();
            CreateMap<FunctionPrivilegeDto, FunctionPrivilege>();
            CreateMap<TerminalDto, Terminal>();
            CreateMap<WorkgroupDto, Workgroup>();

            #endregion

            #region wpf(foreign) -> was

            CreateMap<ForeignUser, User>();
            CreateMap<ForeginPassword, Password>();
            CreateMap<ForeignRole, Role>();
            CreateMap<ForeignFunctionPrivilege, FunctionPrivilege>();
            CreateMap<ForeignTerminal, Terminal>();
            CreateMap<ForeignWorkgroup, Workgroup>();

            #endregion
        }
    }
}