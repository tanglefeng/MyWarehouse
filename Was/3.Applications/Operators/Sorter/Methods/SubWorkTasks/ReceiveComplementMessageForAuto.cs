using System;
using Kengic.Was.CrossCutting.MessageCodes;

namespace Kengic.Was.Operator.Sorter.Methods.SubWorkTasks
{
    public class ReceiveComplementMessageForAuto : CommonExecuteMethod
    {
        public ReceiveComplementMessageForAuto()
        {
            Id = "ReceiveSorterComplementMessageForAuto";
        }

        protected override Tuple<bool, string> Execute(string message)
        {
            ReceiveComplementMessageForAuto();
            return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
        }
    }
}