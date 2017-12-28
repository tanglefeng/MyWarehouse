
using Kengic.Was.Operator.Common;
using Kengic.Was.Operator.WebEasy.ExecuteMethod.SourceWorkTask;
using Kengic.Was.Operator.WebEasy.Process;

namespace Kengic.Was.Operator.WebEasy
{
    public class WebEasyOperator : WasOperator
    {
     

        protected override void RegisterQueue()
        {
          

            base.RegisterQueue();
        }

        protected override void RegisterFunctionMethod()
        {
            RegisterFunctionMethod(new ReceiveSubSystemMessageForAuto());
            RegisterFunctionMethod(new FinishSourceWorkTask());
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