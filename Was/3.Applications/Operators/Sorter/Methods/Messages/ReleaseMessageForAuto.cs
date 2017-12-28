using System;
using Kengic.Was.CrossCutting.MessageCodes;

namespace Kengic.Was.Operator.Sorter.Methods.Messages
{
    public class ReleaseMessageForAuto : CommonExecuteMethod
    {
        public ReleaseMessageForAuto()
        {
            Id = "ReleaseSorterMessageWorkTaskForAuto";
        }

        protected override Tuple<bool, string> Execute(string message)
        {
            ReleaseMessageWorkTaskForAuto();
            return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
        }
    }
}