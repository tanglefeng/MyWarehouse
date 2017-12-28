using System;
using System.Linq;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Systems.ActivityContracts;

namespace Kengic.Was.Operator.Common.Processes
{
    public class AsyncReceiveMessageProcess : OperatorFunctionProcess
    {
        protected override bool Execute()
        {
            var receciveMessageQueue = MessageManager.GetMessageQueue(StaticParameterForOperator.ReceiveMessageQueueName);

            var message = receciveMessageQueue.Values.OrderBy(e => e.Priority).FirstOrDefault();

            if (message == null)
            {
                return false;
            }
            ProcessMessage outMessage;
            var activityContractName = message.ActivityContrace;
            try
            {
                var activityContract = ActivityContractRepository.GetActivityContract(activityContractName);

                var functionMethod = FunctionMethodManager.GetMethod(activityContract.ActivityContract);

                if (functionMethod == null)
                {
                    LogRepository.WriteErrorLog(LogName, StaticParameterForMessage.MethodIsNotExist,
                        activityContract.ActivityContract);
                    receciveMessageQueue.TryRemove(message.Id, out outMessage);
                    return false;
                }


                var returnValue = functionMethod.MethodExecute(message.Message);
                receciveMessageQueue.TryRemove(message.Id, out outMessage);

                return returnValue.Item1;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(LogName, StaticParameterForMessage.MethodExecuteExcept,
                    $"{activityContractName},{ex}");
                receciveMessageQueue.TryRemove(message.Id, out outMessage);
                return false;
            }
        }
    }
}