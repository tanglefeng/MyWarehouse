using System;
using Kengic.Was.CrossCutting.MessageCodes;

namespace Kengic.Was.Operator.SdsSimulation.Methods.SourceWorkTasks
{
    public class CreateSourceWorkTaskForAuto : CommonExecuteMethod
    {
        public CreateSourceWorkTaskForAuto()
        {
            Id = "CreateSdsSimulationSourceWorkTaskForAuto";
        }

        protected override Tuple<bool, string> Execute(string message)
        {
            CreateSourceWorkTaskForAuto();
            return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
        }
    }
}