using System;
using System.Collections.Generic;
using System.Linq;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Systems.ActivityContracts;

namespace Kengic.Was.Operator.Common.Processes
{
    public class AsyncSendMessageProcess : OperatorFunctionProcess
    {
        protected override bool Execute()
        {
            var sendMessageQueue = MessageManager.GetMessageQueue(StaticParameterForOperator.SendMessageQueueName);

            if ((sendMessageQueue == null) || (sendMessageQueue.Count <= 0))
            {
                return false;
            }


            try
            {
                var processMessageList = sendMessageQueue.Values.OrderBy(e => e.Priority).ToList();
                var removeList = new List<ProcessMessage>();
                foreach (var processMessage in processMessageList)
                {
                    var activityContract =
                        ActivityContractRepository.GetActivityContract(processMessage.ActivityContrace);

                    if (activityContract == null)
                    {
                        LogRepository.WriteErrorLog(LogName, StaticParameterForMessage.ActivityContractIsNotExist,
                            processMessage.ActivityContrace);
                        continue;
                    }

                    var operatorInstance =
                        OperatorRepository.GetOperatorInstance(activityContract.OperatorName);

                    if (operatorInstance == null)
                    {
                        LogRepository.WriteErrorLog(LogName, StaticParameterForMessage.OperatorIsNotExist,
                            activityContract.OperatorName);
                        continue;
                    }
                    if (!operatorInstance.IsReciveMessage)
                    {
                        LogRepository.WriteErrorLog(LogName, StaticParameterForMessage.NoReceiveMessage,
                            activityContract.OperatorName);
                        continue;
                    }

                    var returnValue = OperatorRepository.ExecuteActivity(processMessage);
                    if (!returnValue.Item1)
                    {
                        continue;
                    }

                    removeList.Add(processMessage);

                    LogRepository.WriteErrorLog(LogName, StaticParameterForMessage.SendMessage,
                        $"{processMessage.ActivityContrace},{processMessage.Message}");
                }

                foreach (var removeMessage in removeList)
                {
                    ProcessMessage outMessage;
                    sendMessageQueue.TryRemove(removeMessage.Id, out outMessage);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(LogName, StaticParameterForMessage.SendMessageException, ex.ToString());
                sendMessageQueue.Clear();
                return false;
            }
        }
    }
}