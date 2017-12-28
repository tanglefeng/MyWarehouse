using System;

namespace Kengic.Was.Operator.ImaVision.ExecuteMethod
{
    public class RequestDestination : CommonExecuteMethod
    {
        public RequestDestination()
        {
            Id = "RequestDestination";
        }

        protected override Tuple<bool, string> Execute(string message) => RequestDestination(message);
    }
}