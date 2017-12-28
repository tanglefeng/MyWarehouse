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

namespace Kengic.Was.Application.Services.AccessControl.Users
{
    public class UserApplicationServices : IUserApplicationServices
    {
        private readonly IFunctionPrivilegeRepository _functionPrivilegeRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _theRepository;

        public UserApplicationServices(IUserRepository theRepository, IRoleRepository roleRepository,
            IFunctionPrivilegeRepository functionPrivilegeRepository)
        {
            if (theRepository == null)
            {
                throw new ArgumentNullException(nameof(theRepository));
            }
            _theRepository = theRepository;
            _roleRepository = roleRepository;
            _functionPrivilegeRepository = functionPrivilegeRepository;
        }

        public Tuple<bool, string> Create(User value) => _theRepository.Create(value);

        public Tuple<bool, string> Update(User value)
        {
            if (value.Id != value.PersonnelId)
            {
                const string messageCode = StaticParameterForMessage.IdShouldntChange;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            return _theRepository.Update(value);
        }

        public Tuple<bool, string> Remove(User value) => _theRepository.Remove(value);
        public IQueryable<User> GetAll() => _theRepository.GetAll();
        public User GetUserById(string userId) => _theRepository.GetWithRoles(userId);

        public Tuple<bool, string> UpdateUserRoles(string userId, string roleId)
        {
            var user = GetUserById(userId);
            var role = _roleRepository.TryGetValue(roleId);
            if ((role == null) || (user == null))
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);

            if (user.Roles.Contains(role))
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsExist);
            }

            user.Roles.Add(role);
            return Update(user);
        }

        public Tuple<bool, string> RemoveUserRoles(string userId, string roleId)
        {
            var user = GetUserById(userId);
            var role = _roleRepository.TryGetValue(roleId);
            if ((role == null) || (user == null))
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);

            if (!user.Roles.Contains(role))
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);
            }

            user.Roles.Remove(role);
            return Update(user);
        }

        public Tuple<bool, string> UpdateUserPrivileges(string userId, string privilegeId)
        {
            var user = _theRepository.GetWithFunctionPrivileges(userId);
            var privilege = _functionPrivilegeRepository.TryGetValue(privilegeId);
            if ((privilege == null) || (user == null))
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);

            if (user.FunctionPrivileges.Contains(privilege))
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsExist);
            }

            user.FunctionPrivileges.Add(privilege);
            return Update(user);
        }

        public Tuple<bool, string> RemoveUserPrivileges(string userId, string privilegeId)
        {
            var user = _theRepository.GetWithFunctionPrivileges(userId);
            var privilege = _functionPrivilegeRepository.TryGetValue(privilegeId);
            if ((privilege == null) || (user == null))
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);

            if (!user.FunctionPrivileges.Contains(privilege))
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);
            }

            user.FunctionPrivileges.Remove(privilege);
            return Update(user);
        }
    }
}