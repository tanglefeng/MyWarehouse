using System;
using System.Collections.Generic;
using Kengic.Was.Application.Services.SystemSequences;
using Kengic.Was.Application.Services.SystemTracing;
using Kengic.Was.Application.WasModel.Common;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.SystemSequences;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.DistributedServices.Common;
using Kengic.Was.Domain.Entity.SystemSequence;
using Kengic.Was.Wcf.ISystemSequences;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace Kengic.Was.Wcf.SystemSequences
{
    [ExceptionShielding("WcfServicePolicy")]
    public class SystemSequenceService : ISystemSequenceService
    {
        private readonly IOperationTracingApplicationService _operationTracingApplicationService;
        private readonly ISystemSequenceRepository _systemSequenceRepository;
        private readonly ISystemSequencesApplicationService _systemSequencesApplicationService;

        public SystemSequenceService(ISystemSequencesApplicationService systemSequencesApplicationService,
            IOperationTracingApplicationService operationTracingApplicationService,
            ISystemSequenceRepository systemSequenceRepository)
        {
            if (systemSequencesApplicationService == null)
            {
                throw new ArgumentNullException(nameof(systemSequencesApplicationService));
            }
            if (operationTracingApplicationService == null)
            {
                throw new ArgumentNullException(nameof(operationTracingApplicationService));
            }

            _systemSequencesApplicationService = systemSequencesApplicationService;
            _operationTracingApplicationService = operationTracingApplicationService;
            _systemSequenceRepository = systemSequenceRepository;
        }

        public Tuple<bool, string> CreateSystemSequence(SystemSequenceDto value)
        {
            var values = value.ProjectedAs<SystemSequenceDto, SystemSequence>();
            var returnValue = _systemSequencesApplicationService.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create, GetType().Name,
                StaticParameterForMessage.SystemSequence, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateSystemSequence(SystemSequenceDto value)
        {
            var values = value.ProjectedAs<SystemSequenceDto, SystemSequence>();
            var returnValue = _systemSequencesApplicationService.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Update, GetType().Name,
                StaticParameterForMessage.SystemSequence, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveSystemSequence(SystemSequenceDto value)
        {
            var values = value.ProjectedAs<SystemSequenceDto, SystemSequence>();
            var returnValue = _systemSequencesApplicationService.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Remove, GetType().Name,
                StaticParameterForMessage.SystemSequence, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public string GetDataFromSystemSequence(
            List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_systemSequenceRepository, dynamicQueryMethods);
    }
}