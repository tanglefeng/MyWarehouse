using System;
using Kengic.Was.CrossCutting.MessageCodes;

namespace Kengic.Was.Operator.SdsSimulation.Methods.SourceWorkTasks
{
    public class ReadySourceWorkTaskForAuto : CommonExecuteMethod
    {
        public ReadySourceWorkTaskForAuto()
        {
            Id = "ReadySdsSimulationSourceWorkTaskForAuto";
        }

        protected override Tuple<bool, string> Execute(string message)
        {
            ReadySourceWorkTaskForAuto();
            return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
        }
    }
}