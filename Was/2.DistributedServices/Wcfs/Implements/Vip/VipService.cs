using System;
using System.Collections.Generic;
using Kengic.Was.Application.Services.SystemTracing;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.Vip;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.DistributedServices.Common;
using Kengic.Was.Domain.Entity.Vip;
using Kengic.Was.Wcf.IVip;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace Kengic.Was.Wcf.Vip
{
    [ExceptionShielding("WcfServicePolicy")]
    public class VipService : IVipService
    {
        private readonly IOperationTracingApplicationService _operationTracingApplicationService;
        private readonly IVipPackageMessageWorkTaskRepository _vipPackageMessageWorkTaskRepository;
        private readonly IVipSourceWorkTaskRepository _vipSourceWorkTaskRepository;

        public VipService(
            IOperationTracingApplicationService operationTracingApplicationService,
            IVipSourceWorkTaskRepository vipSourceWorkTaskRepository,
            IVipPackageMessageWorkTaskRepository vipPackageMessageWorkTaskRepository)
        {
            if (operationTracingApplicationService == null)
            {
                throw new ArgumentNullException(nameof(operationTracingApplicationService));
            }

            _operationTracingApplicationService = operationTracingApplicationService;
            _vipSourceWorkTaskRepository = vipSourceWorkTaskRepository;
            _vipPackageMessageWorkTaskRepository = vipPackageMessageWorkTaskRepository;
        }


        public Tuple<bool, string> TerminateSourceWorkTask(string message)
        {
            var workTask = message.JsonToValue<VipSourceWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncTerminateVipSourceWorkTask", message, 5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.TerminatedBy,
                StaticParameterForMessage.Terminate,
                GetType().Name,
                StaticParameterForMessage.SourceWorkTask, workTask.CageCode,
                workTask.WmsMsgid + "->" + workTask.CageCode, workTask.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CancelSourceWorkTask(string message)
        {
            var workTask = message.JsonToValue<VipSourceWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncCancelVipSourceWorkTask", message, 5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.CancelledBy,
                StaticParameterForMessage.Cancel,
                GetType().Name,
                StaticParameterForMessage.SourceWorkTask, workTask.CageCode,
                workTask.WmsMsgid + "->" + workTask.CageCode, workTask.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CancelPackageMessageWorkTask(string message)
        {
            var workTask = message.JsonToValue<VipPackageMessageWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncCancelVipPackageMessageWorkTask", message, 5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.CancelledBy,
                StaticParameterForMessage.Cancel,
                GetType().Name,
                StaticParameterForMessage.Message, workTask.CageCode,
                workTask.CageCode + "->" + workTask.ShuteCode, workTask.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> TerminatePackageMessageSubWorkTask(string message)
        {
            var workTask = message.JsonToValue<VipPackageMessageWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncTerminateVipPackageMessageWorkTask", message,
                5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.TerminatedBy,
                StaticParameterForMessage.Terminate,
                GetType().Name,
                StaticParameterForMessage.Message, workTask.CageCode,
                workTask.CageCode + "->" + workTask.ShuteCode, workTask.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }


        public string GetDataFromPackageMessageWorkTask(
            List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_vipPackageMessageWorkTaskRepository, dynamicQueryMethods);

        public string GetDataFromSourceWorkTask(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_vipSourceWorkTaskRepository, dynamicQueryMethods);
    }
}