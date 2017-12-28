using System;
using System.Linq;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.Sorter.LogicalDestinations;
using Kengic.Was.Domain.Entity.Sorter.Routings;

namespace Kengic.Was.Application.Services.Sorter.LogicalDestinations
{
    public class LogicalDestinationApplicationService : ILogicalDestinationApplicationService
    {
        private readonly IRoutingRepository _routingRepository;
        private readonly ILogicalDestinationRepository _theRepository;

        public LogicalDestinationApplicationService(
            ILogicalDestinationRepository theRepository, IRoutingRepository routingRepository)
        {
            _theRepository = theRepository;
            _routingRepository = routingRepository;
        }

        public Tuple<bool, string> Create(LogicalDestination value)
        {
            if (value.Id == value.ParentId)
            {
                const string messageCode = StaticParameterForMessage.ParentIsNotEqualSelf;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }

            return _theRepository.Create(value);
        }

        public Tuple<bool, string> Update(LogicalDestination value)
        {
            if (value.Id == value.ParentId)
            {
                const string messageCode = StaticParameterForMessage.ParentIsNotEqualSelf;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            return _theRepository.Update(value);
        }

        public Tuple<bool, string> Remove(LogicalDestination value)
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

        public IQueryable<LogicalDestination> GetAll() => _theRepository.GetAll();
    }
}