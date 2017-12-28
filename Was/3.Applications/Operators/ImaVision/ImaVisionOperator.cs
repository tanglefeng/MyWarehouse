

using Kengic.Was.Operator.Common;
using Kengic.Was.Operator.ImaVision.ExecuteMethod;
using Kengic.Was.Operator.ImaVision.Process;

namespace Kengic.Was.Operator.ImaVision
{
    public class ImaVisionOperator : WasOperator
    {
       
        protected override void RegisterQueue()
        {
          
            base.RegisterQueue();
        }

        protected override void RegisterFunctionMethod()
        {
            RegisterFunctionMethod(new ReceiveSubSystemMessageForAuto());
            RegisterFunctionMethod(new RequestDestination());
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