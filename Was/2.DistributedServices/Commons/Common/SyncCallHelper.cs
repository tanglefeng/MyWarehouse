using System;
using Kengic.Was.Operator.Common;

namespace Kengic.Was.DistributedServices.Common
{
    public class SyncCallHelper
    {
        public static Tuple<bool, string> SyncCallActivityContract(string activityName, string message, int priority)
        {
            var processMessage = new ProcessMessage
            {
                ActivityContrace = activityName,
                Message = message,
                Priority = priority
            };

            return OperatorRepository.ExecuteActivity(processMessage);
        }
    }
}