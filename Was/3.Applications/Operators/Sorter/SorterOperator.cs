using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.WorkTask;
using Kengic.Was.Operator.Common;
using Kengic.Was.Operator.Sorter.Methods.ExecuteWorkTasks;
using Kengic.Was.Operator.Sorter.Methods.Messages;
using Kengic.Was.Operator.Sorter.Methods.SubWorkTasks;
using Kengic.Was.Operator.Sorter.Parameters;
using Kengic.Was.Operator.Sorter.Processes;
using Kengic.Was.Operator.Sorter.Queues;
using Microsoft.Practices.ServiceLocation;

namespace Kengic.Was.Operator.Sorter
{
    public class SorterOperator : WasOperator
    {
        private static readonly SorterSubWorkTaskQueue SubWorkTaskQueue =
            new SorterSubWorkTaskQueue(ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>());

        private static readonly SorterMessageWorkTaskQueue MesssageWorkTaskQueue =
            new SorterMessageWorkTaskQueue(ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>());

        protected SorterExecuteWorkTaskRepository ExecuteWorkTaskRepository =
            ServiceLocator.Current.GetInstance<SorterExecuteWorkTaskRepository>();

        protected override void RegisterQueue()
        {
            ExecuteWorkTaskRepository.LogName = LogName;
            RegisterQueue(StaticParameterForWorkTask.ExecuteWorkTaskQueueName, ExecuteWorkTaskRepository);
            SubWorkTaskQueue.LogName = LogName;
            RegisterQueue(StaticParameterForWorkTask.SubWorkTaskQueueName, SubWorkTaskQueue);
            MesssageWorkTaskQueue.LogName = LogName;
            RegisterQueue(StaticParameterForSorter.MessageWorkTaskQueueName, MesssageWorkTaskQueue);

            base.RegisterQueue();
        }

        protected override void RegisterFunctionMethod()
        {
            RegisterFunctionMethod(new CreateExecuteWorkTask());
            RegisterFunctionMethod(new CancelExecuteWorkTask());
            RegisterFunctionMethod(new TerminateExecuteWorkTask());
            RegisterFunctionMethod(new FinishSubWorkTask());
            RegisterFunctionMethod(new CreateSubWorkTask());
            RegisterFunctionMethod(new TerminateSubWorkTask());
            RegisterFunctionMethod(new CancelSubWorkTask());
            RegisterFunctionMethod(new ReadySubWorkTaskForAuto());
            RegisterFunctionMethod(new FinishSubWorkTaskForAuto());
            RegisterFunctionMethod(new ReleaseSubWorkTaskForAuto());
            RegisterFunctionMethod(new ReceiveSubSystemMessageForAuto());
            RegisterFunctionMethod(new CancelMessage());
            RegisterFunctionMethod(new TerminateMessage());
            RegisterFunctionMethod(new RenewMessage());
            RegisterFunctionMethod(new CreateMessage());
            RegisterFunctionMethod(new ReleaseMessageForAuto());
            RegisterFunctionMethod(new FaultTimeoutSubWorkTaskForAuto());
            RegisterFunctionMethod(new ReadySubWorkTaskForComplement());
            RegisterFunctionMethod(new FaultTimeoutExecuteWorkTaskForAuto());
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