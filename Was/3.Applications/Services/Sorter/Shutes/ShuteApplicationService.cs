using System;
using System.Linq;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.Sorter.Routings;
using Kengic.Was.Domain.Entity.Sorter.Shutes;

namespace Kengic.Was.Application.Services.Sorter.Shutes
{
    public class ShuteApplicationService : IShuteApplicationService
    {
        private readonly IRoutingRepository _routingRepository;
        private readonly IShuteRepository _theRepository;

        public ShuteApplicationService(
            IShuteRepository theRepository, IRoutingRepository routingRepository)
        {
            _theRepository = theRepository;
            _routingRepository = routingRepository;
        }

        public Tuple<bool, string> Create(Shute value) => _theRepository.Create(value);
        public Tuple<bool, string> Update(Shute value) => _theRepository.Update(value);

        public Tuple<bool, string> Remove(Shute value)
        {
            var valueList = _routingRepository.GetValueByLogicalDestination(value.Id);
            if ((valueList != null) && (valueList.Count > 0))
            {
                const string messageCode = StaticParameterForMessage.RelationObjectIsExist;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }

            return _theRepository.Remove(value);
        }

        public IQueryable<Shute> GetAll() => _theRepository.GetAll();
    }
}