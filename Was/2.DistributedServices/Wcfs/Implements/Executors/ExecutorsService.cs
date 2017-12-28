using System;
using System.Collections.Generic;
using Kengic.Was.Application.Services.Executors;
using Kengic.Was.Application.Services.SystemTracing;
using Kengic.Was.Application.WasModel.Common;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.Executors;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.DistributedServices.Common;
using Kengic.Was.Domain.Entity.Executor;
using Kengic.Was.Wcf.IExecutors;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace Kengic.Was.Wcf.Executors
{
    [ExceptionShielding("WcfServicePolicy")]
    public class ExecutorsService : IExecutorsService
    {
        private readonly IWasExecutorRepository _executorRepository;
        private readonly IExecutorsApplicationService _executorsApplicationService;
        private readonly IOperationTracingApplicationService _operationTracingApplicationService;

        public ExecutorsService(IExecutorsApplicationService executorsApplicationService,
            IOperationTracingApplicationService operationTracingApplicationService,
            IWasExecutorRepository executorRepository)
        {
            if (executorsApplicationService == null)
            {
                throw new ArgumentNullException(nameof(executorsApplicationService));
            }
            if (operationTracingApplicationService == null)
            {
                throw new ArgumentNullException(nameof(operationTracingApplicationService));
            }
            _executorsApplicationService = executorsApplicationService;
            _operationTracingApplicationService = operationTracingApplicationService;
            _executorRepository = executorRepository;
        }

        public Tuple<bool, string> CreateExecutor(WasExecutorDto value)
        {
            var values = value.ProjectedAs<WasExecutorDto, WasExecutor>();
            var returnValue = _executorsApplicationService.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create, GetType().Name,
                StaticParameterForMessage.Executor, value.Id, value.Connection + ":" + value.ExecuteOperator,
                value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateExecutor(WasExecutorDto value)
        {
            var values = value.ProjectedAs<WasExecutorDto, WasExecutor>();
            var returnValue = _executorsApplicationService.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Update, GetType().Name,
                StaticParameterForMessage.Executor, value.Id, value.Connection + ":" + value.ExecuteOperator,
                value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveExecutor(WasExecutorDto value)
        {
            var values = value.ProjectedAs<WasExecutorDto, WasExecutor>();
            var returnValue = _executorsApplicationService.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Remove, GetType().Name,
                StaticParameterForMessage.Executor, value.Id, value.Connection + ":" + value.ExecuteOperator,
                value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public string GetDataFromExecutor(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_executorRepository, dynamicQueryMethods);
    }
}