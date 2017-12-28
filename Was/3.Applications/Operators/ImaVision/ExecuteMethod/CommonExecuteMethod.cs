using System;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;
using Kengic.Was.Domain.Entity.WorkTask;
using Kengic.Was.Operator.Common.Methods;
using System.Linq;
using Newtonsoft.Json.Linq;
using Kengic.Was.Connector.Common;
using System.Collections.Generic;
using Kengic.Was.Domain.Entity.AlarmEvent;
using System.Collections.Concurrent;

namespace Kengic.Was.Operator.ImaVision.ExecuteMethod
{
    public class CommonExecuteMethod : OperatorFunctionMethod
    {
        protected override void InitializeFunction()
        {
            base.InitializeFunction();
        }

        public void ReceiveSubSystemMessageForAuto()
        {
            var messageDict = new ConcurrentDictionary<string, object>();
            //Get all executor of the operator
            try
            {
                var executorList = GetExecutors();
                if ((executorList == null) || (executorList.Count <= 0))
                {
                    return;
                }

                //Get message from the different executor in turns
                foreach (var executor in executorList)
                {
                    var connectionId = executor.Connection;
                    messageDict = GetMessageFromSubsystemWithoutReceiveSendSatus(connectionId);
                    if ((messageDict == null) || (messageDict.Count <= 0))
                    {
                        continue;
                    }

                    var messageKeyList = messageDict.Keys;
                    foreach (var messageKey in messageKeyList)
                    {
                        object outMessage;
                        var stringMessageDict = (string) messageDict[messageKey];
                        if (stringMessageDict == null)
                        {
                            messageDict.TryRemove(messageKey, out outMessage);
                            continue;
                        }
                        var messageArray = stringMessageDict.Split('|');

                        if (messageArray.Length < 5)
                        {
                            messageDict.TryRemove(messageKey, out outMessage);
                            continue;
                        }
                     
                        //switch (messageArray[1].Trim())
                        //{                           
                        //    case "Induct":
                        //        CreateSourceWorkTask(stringMessageDict, connectionId);
                        //        break;                        
                        //}
                                          
                        messageDict.TryRemove(messageKey, out outMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                messageDict.Clear();
                LogRepository.WriteExceptionLog(LogName, StaticParameterForMessage.ReceiveMessageException,
                    ex.ToString());
            }
        }
    
        public Tuple<bool, string> RequestDestination(string message)
        {
            var executorList = GetExecutors();
            if ((executorList == null) || (executorList.Count <= 0))
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.Failure);
            }

            var connector = ConnectorsRepository.GetConnectorInstance(executorList[0].Connection);
            if(connector == null)
                return new Tuple<bool, string>(false, StaticParameterForMessage.Failure);

            //Create,Remove alarm automatically
            if (!connector.RecSendMsgStatus)
            {
                if (!connector.AlarmActiveStatus)
                {
                    CreateDeviceAlarm(StaticParameterForMessage.Disconnect, OperatorName, connector.Id,
                        AlarmEventGrade.Critical);
                    connector.AlarmActiveStatus = true;
                }
                return new Tuple<bool, string>(false, StaticParameterForMessage.Failure);
            }
            if (connector.AlarmActiveStatus)
            {
                connector.AlarmActiveStatus = false;
                RemoveDeviceAlarm(StaticParameterForMessage.Disconnect, OperatorName, connector.Id);
            }

            var jObject = message.JsonToValue<JObject>();

            var id = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Id);
            var objectToHandle = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.ObjectToHandle);
            var indcut = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Induct);
            var messageType = "UUID";

            //id是不是应该改为扫描到的订单号
            var sendmessage =
                      $"|{messageType}|{id}|";
            var messageList = new List<string> { sendmessage };

            connector.SendMessage(messageList);

            return new Tuple<bool, string>(false, StaticParameterForMessage.Ok);
        }
   
    }

   
}