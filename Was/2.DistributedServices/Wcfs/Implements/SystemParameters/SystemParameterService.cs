using System;
using System.Collections.Generic;
using Kengic.Was.Application.Services.SystemParameters;
using Kengic.Was.Application.Services.SystemTracing;
using Kengic.Was.Application.WasModel.Common;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.SystemParameters;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.DistributedServices.Common;
using Kengic.Was.Domain.Entity.SystemParameter;
using Kengic.Was.Wcf.ISystemParameters;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace Kengic.Was.Wcf.SystemParameters
{
    [ExceptionShielding("WcfServicePolicy")]
    public class SystemParameterService : ISystemParameterService
    {
        private readonly IOperationTracingApplicationService _operationTracingApplicationService;
        private readonly ISystemParameterRepository _systemParameterRepository;
        private readonly ISystemParameterTemplateRepository _systemParameterTemplateRepository;
        private readonly ISystemParamtersApplicationService _systemParamtersApplicationService;
        private readonly ISystemParamtersTemplateApplicationService _systemParamtersTemplateApplicationService;

        public SystemParameterService(ISystemParamtersApplicationService systemParamtersApplicationService,
            ISystemParamtersTemplateApplicationService systemParamtersTemplateApplicationService,
            IOperationTracingApplicationService operationTracingApplicationService,
            ISystemParameterRepository systemParameterRepository,
            ISystemParameterTemplateRepository systemParameterTemplateRepository)
        {
            if (systemParamtersApplicationService == null)
            {
                throw new ArgumentNullException(nameof(systemParamtersApplicationService));
            }

            if (systemParamtersTemplateApplicationService == null)
            {
                throw new ArgumentNullException(nameof(systemParamtersTemplateApplicationService));
            }
            if (operationTracingApplicationService == null)
            {
                throw new ArgumentNullException(nameof(operationTracingApplicationService));
            }
            _systemParamtersApplicationService = systemParamtersApplicationService;
            _systemParamtersTemplateApplicationService = systemParamtersTemplateApplicationService;
            _operationTracingApplicationService = operationTracingApplicationService;
            _systemParameterRepository = systemParameterRepository;
            _systemParameterTemplateRepository = systemParameterTemplateRepository;
        }

        public Tuple<bool, string> CreateSystemParameterTemplate(SystemParameterTemplateDto value)
        {
            var values = value.ProjectedAs<SystemParameterTemplateDto, SystemParameterTemplate>();
            var returnValue = _systemParamtersTemplateApplicationService.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create, GetType().Name, StaticParameterForMessage.SystemParameterTemplate,
                value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateSystemParameterTemplate(SystemParameterTemplateDto value)
        {
            var values = value.ProjectedAs<SystemParameterTemplateDto, SystemParameterTemplate>();

            var returnValue = _systemParamtersTemplateApplicationService.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Update, GetType().Name, StaticParameterForMessage.SystemParameterTemplate,
                value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveSystemParameterTemplate(SystemParameterTemplateDto value)
        {
            var values = value.ProjectedAs<SystemParameterTemplateDto, SystemParameterTemplate>();
            var returnValue = _systemParamtersTemplateApplicationService.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Remove, GetType().Name, StaticParameterForMessage.SystemParameterTemplate,
                value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateSystemParameter(SystemParameterDto value)
        {
            var values = value.ProjectedAs<SystemParameterDto, SystemParameter>();
            var returnValue = _systemParamtersApplicationService.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create, GetType().Name,
                StaticParameterForMessage.SystemParameter, value.Id, value.Value, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateSystemParameter(SystemParameterDto value)
        {
            var values = value.ProjectedAs<SystemParameterDto, SystemParameter>();
            var returnValue = _systemParamtersApplicationService.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Update, GetType().Name,
                StaticParameterForMessage.SystemParameter, value.Id, value.Value, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveSystemParameter(SystemParameterDto value)
        {
            var values = value.ProjectedAs<SystemParameterDto, SystemParameter>();
            var returnValue = _systemParamtersApplicationService.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Remove, GetType().Name,
                StaticParameterForMessage.SystemParameter, value.Id, value.Value, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public string GetDataFromSystemParameterTemplate(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_systemParameterTemplateRepository, dynamicQueryMethods);

        public string GetDataFromSystemParameter(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_systemParameterRepository, dynamicQueryMethods);
    }
}