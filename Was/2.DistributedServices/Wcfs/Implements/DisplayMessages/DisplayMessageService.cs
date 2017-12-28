using System;
using System.Collections.Generic;
using Kengic.Was.Application.Services.DisplayMessages;
using Kengic.Was.Application.Services.SystemTracing;
using Kengic.Was.Application.WasModel.Common;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.DisplayMessages;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.DistributedServices.Common;
using Kengic.Was.Domain.Entity.DisplayMessage;
using Kengic.Was.Wcf.IDisplayMessages;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace Kengic.Was.Wcf.DisplayMessages
{
    [ExceptionShielding("WcfServicePolicy")]
    public class DisplayMessageService : IDisplayMessageService
    {
        private readonly IDisplayMessageRepository _displayMessageRepository;
        private readonly IDisplayMessageApplicationService _iDisplayMessagetApplicationService;
        private readonly IOperationTracingApplicationService _operationTracingApplicationService;

        public DisplayMessageService(IDisplayMessageApplicationService iDisplayMessagetApplicationService,
            IOperationTracingApplicationService operationTracingApplicationService,
            IDisplayMessageRepository displayMessageRepository)
        {
            if (iDisplayMessagetApplicationService == null)
            {
                throw new ArgumentNullException(nameof(iDisplayMessagetApplicationService));
            }
            if (operationTracingApplicationService == null)
            {
                throw new ArgumentNullException(nameof(operationTracingApplicationService));
            }
            _iDisplayMessagetApplicationService = iDisplayMessagetApplicationService;
            _operationTracingApplicationService = operationTracingApplicationService;
            _displayMessageRepository = displayMessageRepository;
        }

        public Tuple<bool, string> Create(DisplayMessageDto value)
        {
            var values = value.ProjectedAs<DisplayMessageDto, DisplayMessage>();

            if (string.IsNullOrEmpty(value.Id))
            {
                values.Id = Guid.NewGuid().ToString("N");
            }

            var returnValue = _iDisplayMessagetApplicationService.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create, GetType().Name,
                StaticParameterForMessage.DisplayMessage, values.Source, value.Message, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> Update(DisplayMessageDto value)
        {
            var values = value.ProjectedAs<DisplayMessageDto, DisplayMessage>();
            var returnValue = _iDisplayMessagetApplicationService.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Update, GetType().Name,
                StaticParameterForMessage.DisplayMessage, value.Source, value.Message, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> Remove(DisplayMessageDto value)
        {
            var values = value.ProjectedAs<DisplayMessageDto, DisplayMessage>();
            var returnValue = _iDisplayMessagetApplicationService.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Remove, GetType().Name,
                StaticParameterForMessage.DisplayMessage, value.Source, value.Message, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public string GetDataFromDisplayMessage(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_displayMessageRepository, dynamicQueryMethods);
    }
}