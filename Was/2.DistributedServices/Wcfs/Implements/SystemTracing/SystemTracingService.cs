using System;
using System.Collections.Generic;
using Kengic.Was.Application.Services.SystemTracing;
using Kengic.Was.Application.WasModel.Common;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.SystemTracings;
using Kengic.Was.DistributedServices.Common;
using Kengic.Was.Domain.Entity.SystemTracing;
using Kengic.Was.Wcf.ISystemTracing;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace Kengic.Was.Wcf.SystemTracing
{
    [ExceptionShielding("WcfServicePolicy")]
    public class SystemTracingService : ISystemTracingService
    {
        private readonly IOperationTracingApplicationService _operationTracingApplicationService;
        private readonly IOperationTracingRepository _operationTracingRepository;

        public SystemTracingService(IOperationTracingApplicationService operationTracingApplicationService,
            IOperationTracingRepository operationTracingRepository)
        {
            if (operationTracingApplicationService == null)
            {
                throw new ArgumentNullException(nameof(operationTracingApplicationService));
            }

            _operationTracingApplicationService = operationTracingApplicationService;
            _operationTracingRepository = operationTracingRepository;
        }

        public Tuple<bool, string> CreateOperationTracing(OperationTracingDto value)
        {
            var operationTracing = value.ProjectedAs<OperationTracingDto, OperationTracing>();
            return _operationTracingApplicationService.Create(operationTracing);
        }

        public Tuple<bool, string> UpdateOperationTracing(OperationTracingDto value)
        {
            var operationTracing = value.ProjectedAs<OperationTracingDto, OperationTracing>();
            return _operationTracingApplicationService.Update(operationTracing);
        }

        public Tuple<bool, string> RemoveOperationTracing(OperationTracingDto value)
        {
            var operationTracing = value.ProjectedAs<OperationTracingDto, OperationTracing>();
            return _operationTracingApplicationService.Remove(operationTracing);
        }

        public string GetDataFromOpertionTracing(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_operationTracingRepository, dynamicQueryMethods);
    }
}