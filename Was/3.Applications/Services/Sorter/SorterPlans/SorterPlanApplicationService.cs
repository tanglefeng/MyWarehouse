using System;
using System.Linq;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.Sorter.Plans;
using Kengic.Was.Domain.Entity.Sorter.Routings;

namespace Kengic.Was.Application.Services.Sorter.SorterPlans
{
    public class SorterPlanApplicationService : ISorterPlanApplicationService
    {
        private readonly IRoutingRepository _routingRepository;
        private readonly ISorterPlanRepository _theRepository;

        public SorterPlanApplicationService(
            ISorterPlanRepository theRepository, IRoutingRepository routingRepository)
        {
            _theRepository = theRepository;
            _routingRepository = routingRepository;
        }

        public Tuple<bool, string> Create(SorterPlan value) => _theRepository.Create(value);
        public Tuple<bool, string> Update(SorterPlan value) => _theRepository.Update(value);

        public Tuple<bool, string> Remove(SorterPlan value)
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

        public IQueryable<SorterPlan> GetAll() => _theRepository.GetAll();
    }
}