using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.AccessControl.FunctionPrivileges;
using Kengic.Was.Domain.Entity.AccessControl.Roles;
using Kengic.Was.Domain.Entity.AccessControl.Users;

namespace Kengic.Was.Application.Services.AccessControl.Privileges
{
    public class PrivilegeApplicationServices : IPrivilegeApplicationServices
    {
        private readonly IFunctionPrivilegeRepository _privilegeRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public PrivilegeApplicationServices(IFunctionPrivilegeRepository privilegeRepository,
            IUserRepository userRepository, IRoleRepository roleRepository)
        {
            if (privilegeRepository == null)
            {
                throw new ArgumentNullException(nameof(privilegeRepository));
            }
            if (userRepository == null)
            {
                throw new ArgumentNullException(nameof(userRepository));
            }
            if (roleRepository == null)
            {
                throw new ArgumentNullException(nameof(roleRepository));
            }
            _privilegeRepository = privilegeRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public IEnumerable<FunctionPrivilege> GetChildrenValue(FunctionPrivilege functionprivilege)
        {
            var privilegelist = _privilegeRepository.GetAll();
            IList<FunctionPrivilege> chiledrenList = new List<FunctionPrivilege>();
            foreach (var temp in privilegelist)
            {
                if (temp.Parent == null)
                {
                    continue;
                }
                if (temp.Parent.Id == functionprivilege.Id)
                {
                    chiledrenList.Add(temp);
                }
            }
            return chiledrenList;
        }

        public IEnumerable<FunctionPrivilege> GetUserPrivileges(string userId)
        {
            var privilegelist = _userRepository.GetWithFunctionPrivileges(userId).FunctionPrivileges.ToList();
            return privilegelist;
        }

        public Tuple<bool, string> Create(FunctionPrivilege value)
        {
            var isAlreadyExist = _privilegeRepository.TryGetValue(value.Id);
            if (isAlreadyExist != null)
            {
                const string messageCode = StaticParameterForMessage.ObjectIsExist;
                LogRepository.WriteInfomationLog(_privilegeRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            return _privilegeRepository.Create(value);
        }

        public Tuple<bool, string> Update(FunctionPrivilege value)
        {
            string messageCode;
            if (value.Id == value.ParentId)
            {
                messageCode = StaticParameterForMessage.ParentIsNotEqualSelf;
                LogRepository.WriteInfomationLog(_privilegeRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            if (GetChildrenValue(value).Contains(_privilegeRepository.TryGetValue(value.ParentId)))
            {
                messageCode = StaticParameterForMessage.ParentIsNotChildren;
                LogRepository.WriteInfomationLog(_privilegeRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            return _privilegeRepository.Update(value);
        }

        public Tuple<bool, string> Remove(FunctionPrivilege value)
        {
            //还需要级联删除UserFunctionprivilege、RoleFunctionprivilege表中与FunctionPrivilege相关的内容
            if (GetChildrenValue(value) != null)
            {
                if (GetChildrenValue(value).Any())
                {
                    const string messageCode = StaticParameterForMessage.ChildrenIsNotNull;
                    LogRepository.WriteInfomationLog(_privilegeRepository.LogName, messageCode, value.Id);
                    return new Tuple<bool, string>(false, messageCode);
                }
            }
            return _privilegeRepository.Remove(value);
        }

        public IEnumerable<Role> GetRoleListAboutUser(User user)
        {
            IList<Role> roleList = new List<Role>();
            var userWithRole = _userRepository.GetWithRoles(user.Id);
            if (userWithRole.Roles != null)
            {
                foreach (var role in userWithRole.Roles)
                {
                    roleList.Add(role);
                }
            }
            return roleList;
        }

        public Tuple<bool, string> AddRoleToUser(User user, Role role)
        {
            string messageCode;
            var userWithRoles = _userRepository.GetWithRoles(user.Id);
            var roleoracle = _roleRepository.TryGetValue(role.Id);
            if (userWithRoles.Roles.Contains(roleoracle))
            {
                messageCode = StaticParameterForMessage.UserAlreadyHasThisRole;
                LogRepository.WriteInfomationLog(_privilegeRepository.LogName, messageCode, user.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            userWithRoles.Roles.Add(roleoracle);

            //if (roleoracle.FunctionPrivileges != null)
            //{
            //    foreach (var functionPrivilege in roleoracle.FunctionPrivileges)
            //    {
            //        RemoveFunctionPrivilegeRelationFromUserNoParent(user, functionPrivilege);
            //    }
            //}
            messageCode = StaticParameterForMessage.UserAddRoleSuccess;
            _userRepository.UnitOfWork.Commit();
            return new Tuple<bool, string>(true, messageCode);
        }

        public Tuple<bool, string> RemoveRoleFromUser(User user, Role role)
        {
            string messageCode;
            var userWithRoles = _userRepository.GetWithRoles(user.Id);
            var roleoracle = _roleRepository.TryGetValue(role.Id);
            if (!userWithRoles.Roles.Contains(roleoracle))
            {
                messageCode = StaticParameterForMessage.UserDontHasThisRole;
                LogRepository.WriteInfomationLog(_privilegeRepository.LogName, messageCode, user.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            userWithRoles.Roles.Remove(roleoracle);
            _userRepository.UnitOfWork.Commit();

            messageCode = StaticParameterForMessage.UserRemoveRoleSuccess;
            LogRepository.WriteInfomationLog(_privilegeRepository.LogName, messageCode, user.Id);
            return new Tuple<bool, string>(true, messageCode);
        }

        public IQueryable<FunctionPrivilege> GetAll() => _privilegeRepository.GetAll();
    }
}