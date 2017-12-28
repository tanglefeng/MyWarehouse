using System;
using System.Linq;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.Sorter.Shutes;

namespace Kengic.Was.Application.Services.Sorter.Shutes
{
    public class ShuteTypeApplicationService : IShuteTypeApplicationService
    {
        private readonly IShuteRepository _shuteRepository;
        private readonly IShuteTypeRepository _theRepository;

        public ShuteTypeApplicationService(
            IShuteTypeRepository theRepository, IShuteRepository shuteRepository)
        {
            _theRepository = theRepository;
            _shuteRepository = shuteRepository;
        }

        public Tuple<bool, string> Create(ShuteType value) => _theRepository.Create(value);
        public Tuple<bool, string> Update(ShuteType value) => _theRepository.Update(value);

        public Tuple<bool, string> Remove(ShuteType value)
        {
            var valueList = _shuteRepository.GetValueByType(value.Id);
            if ((valueList != null) && (valueList.Count > 0))
            {
                const string messageCode = StaticParameterForMessage.RelationObjectIsExist;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            return _theRepository.Remove(value);
        }

        public IQueryable<ShuteType> GetAll() => _theRepository.GetAll();
    }
}