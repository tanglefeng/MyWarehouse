
using Kengic.Was.Operator.Common;
using Kengic.Was.Operator.Jd.ExecuteMethod.SourceWorkTask;
using Kengic.Was.Operator.Jd.Process;

namespace Kengic.Was.Operator.Jd
{
    public class JdOperator : WasOperator
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