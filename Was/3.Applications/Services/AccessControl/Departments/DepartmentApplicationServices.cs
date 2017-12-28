using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.AccessControl.Departments;

namespace Kengic.Was.Application.Services.AccessControl.Departments
{
    public class DepartmentApplicationServices : IDepartmentApplicationServices
    {
        private readonly IDepartmentRepository _theRepository;

        public DepartmentApplicationServices(IDepartmentRepository theRepository)
        {
            if (theRepository == null)
            {
                throw new ArgumentNullException(nameof(theRepository));
            }
            _theRepository = theRepository;
        }

        public IEnumerable<Department> GetChildrenValue(Department department)
        {
            var departmentlist = _theRepository.GetAll();
            IList<Department> chiledrenList = new List<Department>();
            foreach (var temp in departmentlist)
            {
                if (temp.Parent == null)
                {
                    continue;
                }
                if (temp.Parent.Id == department.Id)
                {
                    chiledrenList.Add(temp);
                }
            }
            return chiledrenList;
        }

        public Tuple<bool, string> Create(Department value)
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

        public Tuple<bool, string> Update(Department value)
        {
            string messageCode;
            if (value.Id == value.ParentId)
            {
                messageCode = StaticParameterForMessage.ParentIsNotEqualSelf;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            if (GetChildrenValue(value).Contains(_theRepository.TryGetValue(value.ParentId)))
            {
                messageCode = StaticParameterForMessage.ParentIsNotChildren;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            return _theRepository.Update(value);
        }

        public Tuple<bool, string> Remove(Department value)
        {
            if (GetChildrenValue(value) != null)
            {
                if (GetChildrenValue(value).Any())
                {
                    const string messageCode = StaticParameterForMessage.ChildrenIsNotNull;
                    LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                    return new Tuple<bool, string>(false, messageCode);
                }
            }
            var departmentwithall = _theRepository.GetWithAll(value.Id);
            if ((departmentwithall != null) && (departmentwithall.Personnels != null))
            {
                if (departmentwithall.Personnels.Any())
                {
                    const string messageCode = StaticParameterForMessage.RelationObjectIsExist;
                    LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                    return new Tuple<bool, string>(false, messageCode);
                }
            }
            return _theRepository.Remove(value);
        }

        public IQueryable<Department> GetAll() => _theRepository.GetAll();
    }
}