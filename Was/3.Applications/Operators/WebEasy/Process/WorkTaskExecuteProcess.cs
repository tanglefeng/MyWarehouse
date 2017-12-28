using Kengic.Was.Operator.Common.Processes;

namespace Kengic.Was.Operator.WebEasy.Process
{
    public class WorkTaskExecuteProcess : OperatorFunctionProcess
    {
        protected override bool Execute()
        {
            ExecuteMethodInProcess("ReceiveSubSystemMessageForAuto");
            return true;
        }
    }
}