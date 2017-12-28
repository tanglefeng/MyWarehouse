using System;
using System.Collections.Generic;
using Kengic.Was.Application.Services.SystemTracing;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.PackageWorkTasks;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.DistributedServices.Common;
using Kengic.Was.Domain.Entity.Package;
using Kengic.Was.Wcf.IPackage;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace Kengic.Was.Wcf.Package
{
    [ExceptionShielding("WcfServicePolicy")]
    public class PackageService : IPackageService
    {
        private readonly IOperationTracingApplicationService _operationTracingApplicationService;
        private readonly IPackageSourceWorkTaskRepository _packageSourceWorkTaskRepository;

        public PackageService(
            IOperationTracingApplicationService operationTracingApplicationService,
            IPackageSourceWorkTaskRepository packageSourceWorkTaskRepository)
        {
            if (operationTracingApplicationService == null)
            {
                throw new ArgumentNullException(nameof(operationTracingApplicationService));
            }

            _operationTracingApplicationService = operationTracingApplicationService;
            _packageSourceWorkTaskRepository = packageSourceWorkTaskRepository;
        }

        public Tuple<bool, string> CreateSourceWorkTask(string message)
        {
            var workTask = message.JsonToValue<PackageSourceWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncCreatePackageSourceWorkTask", message,
                5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.CreateBy,
                StaticParameterForMessage.Create,
                GetType().Name,
                StaticParameterForMessage.SourceWorkTask, workTask.ObjectToHandle,
                workTask.Executor + ":" + workTask.CustomField01, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> TerminateSourceWorkTask(string message)
        {
            var workTask = message.JsonToValue<PackageSourceWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract(
                "SyncTerminatePackageSourceWorkTaskFromUI", message, 5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.TerminatedBy,
                StaticParameterForMessage.Terminate,
                GetType().Name,
                StaticParameterForMessage.SourceWorkTask, workTask.ObjectToHandle,
                workTask.Id, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CancelSourceWorkTask(string message)
        {
            var workTask = message.JsonToValue<PackageSourceWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncCancelPackageSourceWorkTask", message,
                5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.CancelledBy,
                StaticParameterForMessage.Cancel,
                GetType().Name,
                StaticParameterForMessage.SourceWorkTask, workTask.ObjectToHandle,
                workTask.Id, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RenewSourceWorkTask(string message)
        {
            var workTask = message.JsonToValue<PackageSourceWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncRenewPackageSourceWorkTask", message,
                5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.UpdateBy,
                StaticParameterForMessage.Renew,
                GetType().Name,
                StaticParameterForMessage.SourceWorkTask, workTask.ObjectToHandle,
                workTask.Id, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public string GetDataFromPackageSourceWorkTask(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_packageSourceWorkTaskRepository, dynamicQueryMethods);
    }
}