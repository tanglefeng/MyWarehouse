using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.AccessControl.Companys;

namespace Kengic.Was.Application.Services.AccessControl.Companys
{
    public class CompanyApplicationServices : ICompanyApplicationServices
    {
        private readonly ICompanyRepository _theRepository;

        public CompanyApplicationServices(ICompanyRepository theRepository)
        {
            if (theRepository == null)
            {
                throw new ArgumentNullException(nameof(theRepository));
            }
            _theRepository = theRepository;
        }

        public IEnumerable<Company> GetChildrenValue(Company company)
        {
            var companylist = _theRepository.GetAll();
            IList<Company> chiledrenList = new List<Company>();
            foreach (var temp in companylist)
            {
                if (temp.Parent == null)
                {
                    continue;
                }
                if (temp.Parent.Id == company.Id)
                {
                    chiledrenList.Add(temp);
                }
            }
            return chiledrenList;
        }

        public Tuple<bool, string> Create(Company value)
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

        public Tuple<bool, string> Update(Company value)
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

        public Tuple<bool, string> Remove(Company value)
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
            var companywithall = _theRepository.GetWithAll(value.Id);
            if ((companywithall != null) && (companywithall.Departments != null))
            {
                if (companywithall.Departments.Any())
                {
                    const string messageCode = StaticParameterForMessage.RelationObjectIsExist;
                    LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                    return new Tuple<bool, string>(false, messageCode);
                }
            }
            return _theRepository.Remove(value);
        }

        public IQueryable<Company> GetAll() => _theRepository.GetAll();
    }
}