using Kengic.Was.Operator.Common.Processes;

namespace Kengic.Was.Operator.ImaVision.Process
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