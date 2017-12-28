using System;
using System.Linq;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.Sorter.Inducts;
using Kengic.Was.Domain.Entity.Sorter.Plans;
using Kengic.Was.Domain.Entity.Sorter.Scanners;
using Kengic.Was.Domain.Entity.Sorter.Shutes;
using Kengic.Was.Domain.Entity.Sorter.Sorters;

namespace Kengic.Was.Application.Services.Sorter.Sorters
{
    public class LogicalSorterApplicationService : ILogicalSorterApplicationService
    {
        private readonly IInductRepository _inductRepository;
        private readonly IScannerRepository _scannerRepository;
        private readonly IShuteRepository _shuteRepository;
        private readonly ISorterPlanRepository _sorterPlanRepository;
        private readonly ILogicalSorterRepository _theRepository;

        public LogicalSorterApplicationService(
            ILogicalSorterRepository theRepository, IInductRepository inductRepository,
            IScannerRepository scannerRepository, IShuteRepository shuteRepository,
            ISorterPlanRepository sorterPlanRepository)
        {
            _theRepository = theRepository;
            _inductRepository = inductRepository;
            _scannerRepository = scannerRepository;
            _shuteRepository = shuteRepository;
            _sorterPlanRepository = sorterPlanRepository;
        }

        public Tuple<bool, string> Create(LogicalSorter value) => _theRepository.Create(value);
        public Tuple<bool, string> Update(LogicalSorter value) => _theRepository.Update(value);

        public Tuple<bool, string> Remove(LogicalSorter value)
        {
            var inductList = _inductRepository.GetValueByLogicalSorter(value.Id);
            if ((inductList != null) && (inductList.Count > 0))
            {
                const string messageCode = StaticParameterForMessage.RelationObjectIsExist;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }

            var scannerList = _scannerRepository.GetValueByLogicalSorter(value.Id);
            if ((scannerList != null) && (scannerList.Count > 0))
            {
                const string messageCode = StaticParameterForMessage.RelationObjectIsExist;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }

            var shuteList = _shuteRepository.GetValueByLogicalSorter(value.Id);
            if ((shuteList != null) && (shuteList.Count > 0))
            {
                const string messageCode = StaticParameterForMessage.RelationObjectIsExist;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }

            var sorterPlanList = _sorterPlanRepository.GetValueByLogicalSorter(value.Id);
            if ((sorterPlanList != null) && (sorterPlanList.Count > 0))
            {
                const string messageCode = StaticParameterForMessage.RelationObjectIsExist;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }


            return _theRepository.Remove(value);
        }

        public LogicalSorter GetValueByNodeId(string value) => _theRepository.GetValueByNodeId(value);
        public IQueryable<LogicalSorter> GetAll() => _theRepository.GetAll();
    }
}