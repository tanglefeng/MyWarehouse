using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Xml.Linq;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.AccessControl.FunctionPrivileges;
using Kengic.Was.Domain.Entity.AccessControl.Roles;

namespace Kengic.Was.Application.Services.AccessControl.Roles
{
    public class RoleApplicationServices : IRoleApplicationServices
    {
        private readonly IFunctionPrivilegeRepository _privilegeRepository;
        private readonly IRoleRepository _theRepository;

        public RoleApplicationServices(IRoleRepository theRepository, IFunctionPrivilegeRepository privilegeRepository)
        {
            if (theRepository == null)
            {
                throw new ArgumentNullException(nameof(theRepository));
            }
            _privilegeRepository = privilegeRepository;
            _theRepository = theRepository;
        }

        public Tuple<bool, string> Create(Role value)
        {
            var isAlreadyExist = _theRepository.TryGetValue(value.Id);
            if (isAlreadyExist != null)
            {
                const string messageCode = StaticParameterForMessage.ObjectIsExist;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            return _theRepository.Create(value);
        }

        public Tuple<bool, string> Update(Role value) => _theRepository.Update(value);
        public Tuple<bool, string> Remove(Role value) => _theRepository.Remove(value);
        public IQueryable<Role> GetAll() => _theRepository.GetAll();
        public Role GetRole(string id) => _theRepository.GetValues(id);

        public Tuple<bool, string> UpdateRolePrivilege(string roleId, string privileageId)
        {
            var role = GetRole(roleId);
            var privilege = _privilegeRepository.TryGetValue(privileageId);
            if ((role == null) || (privilege == null))
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);

            if (role.FunctionPrivileges.Contains(privilege))
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsExist);
            }

            role.FunctionPrivileges.Add(privilege);
            return Update(role);
        }

        public Tuple<bool, string> RemoveRolePrivilege(string roleId, string privileageId)
        {
            var role = GetRole(roleId);
            var privilege = _privilegeRepository.TryGetValue(privileageId);
            if ((role == null) || (privilege == null))
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);

            if (!role.FunctionPrivileges.Contains(privilege))
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);
            }

            role.FunctionPrivileges.Remove(privilege);
            return Update(role);
        }
    }
}