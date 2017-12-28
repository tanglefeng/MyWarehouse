using System;
using System.Linq;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.AccessControl.Personnels;

namespace Kengic.Was.Application.Services.AccessControl.Personnels
{
    public class PersonnelApplicationServices : IPersonnelApplicationServices
    {
        private readonly IPersonnelRepository _theRepository;

        public PersonnelApplicationServices(IPersonnelRepository theRepository)
        {
            if (theRepository == null)
            {
                throw new ArgumentNullException(nameof(theRepository));
            }
            _theRepository = theRepository;
        }

        public Tuple<bool, string> Create(Personnel value)
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

        public Tuple<bool, string> Update(Personnel value) => _theRepository.Update(value);

        public Tuple<bool, string> Remove(Personnel value)
        {
            var personnelwithall = _theRepository.GetWithAll(value.Id);
            if (personnelwithall.User != null)
            {
                const string messageCode = StaticParameterForMessage.RelationObjectIsExist;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            return _theRepository.Remove(value);
        }

        public IQueryable<Personnel> GetAll() => _theRepository.GetAll();
    }
}