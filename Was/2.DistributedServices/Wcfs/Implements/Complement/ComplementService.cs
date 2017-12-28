using System;
using Kengic.Was.Application.Services.SystemTracing;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.DistributedServices.Common;
using Kengic.Was.Domain.Entity.Complement;
using Kengic.Was.Wcf.IComplement;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace Kengic.Was.Wcf.Complement
{
    [ExceptionShielding("WcfServicePolicy")]
    public class ComplementService : IComplementService
    {
        private readonly IOperationTracingApplicationService _operationTracingApplicationService;

        public ComplementService(IOperationTracingApplicationService operationTracingApplicationService)
        {
            _operationTracingApplicationService = operationTracingApplicationService;
        }

        public Tuple<bool, string> CreateSourceWorkTask(string message)
        {
            var workTask = message.JsonToValue<ComplementSourceWorkTask>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncCreateComplementSourceWorkTask", message, 5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.CreateBy,
                StaticParameterForMessage.Create,
                GetType().Name,
                StaticParameterForMessage.SourceWorkTask, workTask.ObjectToHandle,
                workTask.LogicalSortter + "->" + workTask.RequestShuteAddr, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> TerminateSourceWorkTask(string message)
        {
            var workTask = message.JsonToValue<ComplementSourceWorkTask>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncTerminateComplementSourceWorkTask", message,
                5);
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
            var workTask = message.JsonToValue<ComplementSourceWorkTask>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncCancelComplementSourceWorkTask", message, 5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.CancelledBy,
                StaticParameterForMessage.Cancel,
                GetType().Name,
                StaticParameterForMessage.Message, workTask.ObjectToHandle,
                workTask.Id, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }
    }
}