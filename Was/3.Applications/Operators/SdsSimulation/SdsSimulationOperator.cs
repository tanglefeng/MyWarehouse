using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.WorkTask;
using Kengic.Was.Operator.Common;
using Kengic.Was.Operator.SdsSimulation.Methods.SourceWorkTasks;
using Kengic.Was.Operator.SdsSimulation.Processes;
using Kengic.Was.Operator.SdsSimulation.Queues;
using Microsoft.Practices.ServiceLocation;

namespace Kengic.Was.Operator.SdsSimulation
{
    public class SdsSimulationOperator : WasOperator
    {
        private static readonly SdsSimulationSourceWorkTaskQueue SourceWorkTaskQueue =
            new SdsSimulationSourceWorkTaskQueue(ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>());

        protected override void RegisterQueue()
        {
            SourceWorkTaskQueue.LogName = LogName;
            RegisterQueue(StaticParameterForWorkTask.SourceWorkTaskQueueName, SourceWorkTaskQueue);

            base.RegisterQueue();
        }

        protected override void RegisterFunctionMethod()
        {
            RegisterFunctionMethod(new CreateSourceWorkTask());
            RegisterFunctionMethod(new CancelSourceWorkTask());
            RegisterFunctionMethod(new TerminateSourceWorkTask());
            RegisterFunctionMethod(new ReleaseSourceWorkTaskForAuto());
            RegisterFunctionMethod(new RenewSourceWorkTask());
            RegisterFunctionMethod(new TerminateSourceWorkTask());
            RegisterFunctionMethod(new CreateSourceWorkTaskForAuto());
            RegisterFunctionMethod(new FinishSourceWorkTask());
            RegisterFunctionMethod(new ReadySourceWorkTaskForAuto());
            RegisterFunctionMethod(new ReadySourceWorkTaskForAuto());
            base.RegisterFunctionMethod();
        }

        protected override void RegisterFuncProcess()
        {
            RegisterFunctionProcess(new WorkTaskExecuteProcess
            {
                Id = StaticParameterForOperator.WorkTaskExecuteProcessName
            });

            base.RegisterFuncProcess();
        }
    }
}