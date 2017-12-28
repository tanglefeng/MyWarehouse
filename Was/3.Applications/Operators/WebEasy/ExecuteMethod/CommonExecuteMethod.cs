using System;
using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Connector.Common;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;
using Kengic.Was.Domain.Entity.WorkTask;
using Kengic.Was.Operator.Common.Methods;
using Newtonsoft.Json.Linq;
using Kengic.Was.Operator.WebEasy.StaticParameters;
using Kengic.Was.Domain.Entity.AlarmEvent;
using System.Collections.Concurrent;

namespace Kengic.Was.Operator.WebEasy.ExecuteMethod
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
                //TestingReadySourceWorkTask();

                var executorList = GetExecutors();
                if ((executorList == null) || (executorList.Count <= 0))
                {
                    return;
                }

                //Get message from the different executor in turns
                foreach (var executor in executorList)
                {
                    var connectionId = executor.Connection;
                    //后期是不是应该改为这个具有链接标志位的
                   // messageDict = GetMessageFromSubsystem(connectionId);
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
                       // var messageArray = stringMessageDict.Split(':');
                       var messageArray = stringMessageDict.Split('|');

                        if (messageArray.Length < 3)
                        {
                            messageDict.TryRemove(messageKey, out outMessage);
                            continue;
                        }
                   
                         //switch (messageArray[1].Trim())
                            switch (messageArray[3].Trim())
                        {
                            // case "Task": 
                            case "SORTER":
                                ReadySourceWorkTask(stringMessageDict, connectionId);
                                 break;                          
                         }       
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

    
        public void ReadySourceWorkTask(string message, string connectionId)
        {
            var messageArray = message.Split('|');
           // var messageArray = message.Split(':');

            if (messageArray.Length < 6) return;

            var sorterSubWorkTask = new SorterSubWorkTask
            {
                Id = Id = SequenceCreator.GetIdBySeqNoAndKey("TK"),
                WorkTaskType = WorkTaskType.Sorting,
                NodeId = "1",
                //导入台赋0值
                // Induct = messageArray[5],
                Induct = "0",
                Scanner = "ImaVision",
                //TrackingId为第一个(包的数量值在此即为0 此时均为计算机运算符号)
                //TrackingId = messageArray[2],
                TrackingId = messageArray[1],
                OperatorName = OperatorName,
                //ObjectToHandle = messageArray[3],
                //同理，TrObjectToHandled为第二个(包的数量值在此即为0 此时均为计算机运算符号)
                ObjectToHandle = messageArray[2],
                RequestShuteNum ="01",
                //同理，RequestShuteAddr为第五个(包的数量值在此即为0 此时均为计算机运算符号)
                //RequestShuteAddr = messageArray[4],
                RequestShuteAddr = messageArray[5],
                Status = WorkTaskStatus.Create,
                TriggerMode = WorkTaskTriggerMode.Immediately,
                CreateBy = OperatorName,
                CreateTime = DateTime.Now,
                ScannerTime = DateTime.Now
            };

            SendMessage(sorterSubWorkTask, StaticParameter.NotifySorterOperatorForComplement, 5);
        }


        public void TestingReadySourceWorkTask()
        {        
            var sorterSubWorkTask = new SorterSubWorkTask
            {
                Id = "201711020000001",
                WorkTaskType = WorkTaskType.Sorting,
                NodeId = "1",
                Scanner = "ImaVision",
                OperatorName = OperatorName,
                ObjectToHandle = "888888888",
                RequestShuteNum = "01",
                RequestShuteAddr = "1",
                Status = WorkTaskStatus.Create,
                TriggerMode = WorkTaskTriggerMode.Immediately,
                CreateBy = OperatorName,
                CreateTime = DateTime.Now,
                ScannerTime = DateTime.Now
            };

            SendMessage(sorterSubWorkTask, StaticParameter.NotifySorterOperatorForComplement, 5);
        }

        protected Tuple<bool, string> FinishSourceWorkTask(string message)
        {
            var jObject = message.JsonToValue<JObject>();
            //这个转换可参考的是数据表的列与行的关系 简直帅气的一比
            var workTaskId = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Id);
            var objectToHandle = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.ObjectToHandle);
            var result = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Results);
            //新建最终地址 字段
            var finalAddress= JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.CurrentAddress);
            var messageType = "Result";
            var resultCode = "0";
            if (result.Trim().ToUpper() == "OK")
            {
                resultCode = "1";
            }

            var sendmessage =
                    $"|{messageType}|{workTaskId}|{objectToHandle}|{finalAddress}|{resultCode}|";
            //var sendmessage =
            //         $":{messageType}:{workTaskId}:{objectToHandle}:{finalAddress}:{resultCode}:";


            LogRepository.WriteInfomationLog(LogName, "TIP:"+sendmessage,null);
        
            var executorList = GetExecutors();
            if ((executorList == null) || (executorList.Count <= 0))
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.Failure);
            }

            var connector = ConnectorsRepository.GetConnectorInstance(executorList[0].Connection);
            if (connector == null)
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


           
            var messageList = new List<string> { sendmessage };

            connector.SendMessage(messageList);

            return new Tuple<bool, string>(false, StaticParameterForMessage.Ok);
        }   
    }
}