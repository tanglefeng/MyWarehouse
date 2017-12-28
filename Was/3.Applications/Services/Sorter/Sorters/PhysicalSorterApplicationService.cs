using System;
using System.Linq;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.Sorter.Inducts;
using Kengic.Was.Domain.Entity.Sorter.Scanners;
using Kengic.Was.Domain.Entity.Sorter.Shutes;
using Kengic.Was.Domain.Entity.Sorter.Sorters;

namespace Kengic.Was.Application.Services.Sorter.Sorters
{
    public class PhysicalSorterApplicationService : IPhysicalSorterApplicationService
    {
        private readonly IInductRepository _inductRepository;
        private readonly ILogicalSorterRepository _logicalSorterRepository;
        private readonly IScannerRepository _scannerRepository;
        private readonly IShuteRepository _shuteRepository;
        private readonly IPhysicalSorterRepository _theRepository;

        public PhysicalSorterApplicationService(
            IPhysicalSorterRepository theRepository, IInductRepository inductRepository,
            IScannerRepository scannerRepository, IShuteRepository shuteRepository,
            ILogicalSorterRepository logicalSorterRepository)
        {
            _theRepository = theRepository;
            _inductRepository = inductRepository;
            _scannerRepository = scannerRepository;
            _shuteRepository = shuteRepository;
            _logicalSorterRepository = logicalSorterRepository;
        }

        public Tuple<bool, string> Create(PhysicalSorter value) => _theRepository.Create(value);
        public Tuple<bool, string> Update(PhysicalSorter value) => _theRepository.Update(value);

        public Tuple<bool, string> Remove(PhysicalSorter value)
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

            var logicalSorterList = _logicalSorterRepository.GetValueByPhysicalSorter(value.Id);
            if ((logicalSorterList != null) && (logicalSorterList.Count > 0))
            {
                const string messageCode = StaticParameterForMessage.RelationObjectIsExist;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }

            return _theRepository.Remove(value);
        }

        public IQueryable<PhysicalSorter> GetAll() => _theRepository.GetAll();
    }
}