using System;
using Kengic.Was.CrossCutting.MessageCodes;

namespace Kengic.Was.Operator.SdsSimulation.Methods.SourceWorkTasks
{
    public class ReleaseSourceWorkTaskForAuto : CommonExecuteMethod
    {
        public ReleaseSourceWorkTaskForAuto()
        {
            Id = "ReleaseSdsSimulationSourceWorkTaskForAuto";
        }

        protected override Tuple<bool, string> Execute(string message)
        {
            ReleaseSourceWorkTaskForAuto();
            return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
        }
    }
}