using Kengic.Was.Operator.Common.Processes;

namespace Kengic.Was.Operator.SdsSimulation.Processes
{
    public class WorkTaskExecuteProcess : OperatorFunctionProcess
    {
        protected override bool Execute()
        {
            ExecuteMethodInProcess("ReleaseSdsSimulationSourceWorkTaskForAuto");
            ExecuteMethodInProcess("CreateSdsSimulationSourceWorkTaskForAuto");
            ExecuteMethodInProcess("ReadySdsSimulationSourceWorkTaskForAuto");
            return true;
        }
    }
}