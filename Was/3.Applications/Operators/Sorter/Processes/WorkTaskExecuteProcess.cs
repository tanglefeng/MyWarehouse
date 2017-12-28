using Kengic.Was.Operator.Common.Processes;

namespace Kengic.Was.Operator.Sorter.Processes
{
    public class WorkTaskExecuteProcess : OperatorFunctionProcess
    {
        protected override bool Execute()
        {
            ExecuteMethodInProcess("ReceiveSorterSubSystemMessageForAuto");
            ExecuteMethodInProcess("ReadySorterSubWorkTaskForAuto");
            ExecuteMethodInProcess("ReleaseSorterSubWorkTaskForAuto");
            ExecuteMethodInProcess("FinishSorterSubWorkTaskForAuto");
            ExecuteMethodInProcess("ReleaseSorterMessageWorkTaskForAuto");
            ExecuteMethodInProcess("FaultTimeoutSorterSubWorkTaskForAuto");
            return true;
        }
    }
}