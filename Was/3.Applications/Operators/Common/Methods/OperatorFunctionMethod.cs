using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Connector.Common;
using Kengic.Was.CrossCutting.ConfigurationSection.Operators;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.AlarmEvent;
using Kengic.Was.Domain.Entity.DisplayMessage;
using Kengic.Was.Domain.Entity.Executor;
using Kengic.Was.Domain.Entity.SystemTracing;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;

namespace Kengic.Was.Operator.Common.Methods
{
    public class OperatorFunctionMethod
    {
        private const string DeviceAlarm = "DeviceAlarm";
        private const string SystemAlarm = "SystemAlarm";
        private readonly IDisplayMessageRepository _displayMessageRepository;
        private readonly IWasExecutorRepository _executorsRepository;
        protected readonly IOperationTracingRepository OperationTracingRepository;

        protected OperatorFunctionMethod()
        {
            OperationTracingRepository = ServiceLocator.Current.GetInstance<IOperationTracingRepository>();
            _executorsRepository = ServiceLocator.Current.GetInstance<IWasExecutorRepository>();
            _displayMessageRepository = ServiceLocator.Current.GetInstance<IDisplayMessageRepository>();
        }

        public string Name { get; set; }
        public string Id { get; set; }
        public string LogName { get; set; }
        public string OperatorName { get; set; }
        public OperatorMessageManager MessageManager { get; set; }
        public OperatorQueueManager QueueManager { get; set; }
        public OperatorParameterManager ParameterManager { get; set; }

        public Tuple<bool, string> MethodExecute(string message)
        {
            InitializeFunction();
            return Execute(message);
        }

        protected virtual void InitializeFunction()
        {
        }

        protected virtual Tuple<bool, string> Execute(string message) => new Tuple<bool, string>(false, "");

        private Tuple<bool, string> SendMessage(string notifyMessageId, string message, int priority,
            Func<ActivityElement, bool> filterFunction)
        {
            var operatorActivityList =
                ParameterManager.GetParameter<List<ActivityElement>>(StaticParameterForOperator.ActivityQueueName);

            var operatorActivity = operatorActivityList.FirstOrDefault(filterFunction);
            string messageCode;
            if (operatorActivity == null)
            {
                messageCode = StaticParameterForMessage.GetOperatorNotifyActivityFailure;
                LogRepository.WriteErrorLog(LogName, messageCode, notifyMessageId);
                return new Tuple<bool, string>(false, messageCode);
            }

            var sendMessageQueue = MessageManager.GetMessageQueue(StaticParameterForOperator.SendMessageQueueName);
            if (sendMessageQueue == null)
            {
                messageCode = StaticParameterForMessage.GetSendQueueFailure;
                LogRepository.WriteErrorLog(LogName, messageCode, StaticParameterForOperator.SendMessageQueueName);
                return new Tuple<bool, string>(false, messageCode);
            }

            //send message queue is max
            var sendQueueMaxNumber = ParameterManager.GetParameter<int>(StaticParameterForOperator.SendQueueMaxNumber);
            if (sendMessageQueue.Values.Count >= sendQueueMaxNumber)
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.MaxMessage);
            }

            var processMessage = new ProcessMessage
            {
                Id = Guid.NewGuid().ToString("N"),
                ActivityContrace = operatorActivity.ActivityContractName,
                Message = message,
                Priority = priority
            };

            sendMessageQueue.TryAdd(processMessage.Id, processMessage);
            messageCode = StaticParameterForMessage.Ok;
            return new Tuple<bool, string>(true, messageCode);
        }

        protected Tuple<bool, string> SendMessage(object obj, string notifyMessageId, int priority,
            Func<ActivityElement, bool> filterFunction)
        {
            var message = JsonConvert.SerializeObject(obj);
            return SendMessage(notifyMessageId, message, 5, filterFunction);
        }

        protected Tuple<bool, string> SendMessage(object obj, string notifyMessageId, int priority)
        {
            var message = JsonConvert.SerializeObject(obj);
            return SendMessage(notifyMessageId, message, 5, e => e.Code == notifyMessageId);
        }

        protected Tuple<bool, string> SendMessage(object obj, string notifyMessageId, string executeOperator,
            int priority)
        {
            var message = JsonConvert.SerializeObject(obj);
            return SendMessage(notifyMessageId, message, 5,
                e => (e.Code == notifyMessageId) && (e.ExecuteOperator == executeOperator));
        }

        protected List<WasExecutor> GetExecutors() => _executorsRepository.GetValueByOperator(OperatorName);

        protected Tuple<bool, string> UpdateExecutorCurrentLocation(string executor, string currentLocation)
        {
            var executorObj = _executorsRepository.TryGetValue(executor);
            if (executorObj != null)
            {
                executorObj.CurrentAddress = currentLocation;
                return _executorsRepository.Update(executorObj);
            }

            return new Tuple<bool, string>(false, StaticParameterForMessage.None);
        }

        protected ConcurrentDictionary<string, object> GetMessageFromSubsystem(string connection)
        {
            var connector = ConnectorsRepository.GetConnectorInstance(connection);
            if ((connector == null) || (connector.RecSendMsgStatus == false))
            {
                return null;
            }

            return connector.ReceiveDictionary.Count <= 0 ? null : connector.ReceiveDictionary;
        }

        protected ConcurrentDictionary<string, object> GetMessageFromSubsystemWithoutReceiveSendSatus(string connection)
        {
            var connector = ConnectorsRepository.GetConnectorInstance(connection);
            if (connector == null)
            {
                return null;
            }
            if (connector.ReceiveDictionary == null)
            {
                return null;
            }
            return connector.ReceiveDictionary.Count <= 0 ? null : connector.ReceiveDictionary;
        }

        protected void CreateDeviceAlarm(string alarm, string source, string alarmObject,
            AlarmEventGrade alarmEventGrade)
        {
            CreateAlarm(DeviceAlarm, alarm, source, alarmObject, alarmEventGrade);
            LogRepository.WriteWarningLog(LogName, alarm, source + "-" + alarmObject);
        }

        protected void CreateSystemAlarm(string alarm, string source, string alarmObject,
            AlarmEventGrade alarmEventGrade)
        {
            CreateAlarm(SystemAlarm, alarm, source, alarmObject, alarmEventGrade);
            LogRepository.WriteWarningLog(LogName, alarm, source + "-" + alarmObject);
        }

        private void CreateAlarm(string alarmEventType, string alarm, string source, string alarmObject,
            AlarmEventGrade alarmEventGrade)
        {
            var alarmEventRecordRepository = ServiceLocator.Current.GetInstance<IAlarmEventRecordRepository>();
            var valueList = alarmEventRecordRepository.GetAll()
                .Where(
                    e =>
                        (e.Code == alarm) && (e.Source == source) && (e.Object == alarmObject) &&
                        (e.Type == alarmEventType) && (e.Status < AlarmEventStatus.Removed)).ToList();
            if (valueList.Any())
            {
                return;
            }

            var alarmRecord = new AlarmEventRecord
            {
                Id = Guid.NewGuid().ToString("N"),
                Code = alarm,
                Source = source,
                Type = alarmEventType,
                Grade = alarmEventGrade,
                Object = alarmObject,
                CreateBy = OperatorName,
                CreateTime = DateTime.Now,
                Status = AlarmEventStatus.Create
            };

            alarmEventRecordRepository.Create(alarmRecord);
        }

        //public void CreateOperationTracing(bool status, string user, string source, string operation, string obj,
        //    string objectValue, string context)
        //{
        //    var userRepository = UnityContainer.Resolve<IUserRepository>();
        //    var userList = userRepository.GetAll().Where(e => e.Id == user);
        //    if (userList.Any())
        //    {
        //        var operationTracing = new OperationTracing
        //        {
        //            Id = SequenceCreator.GetIdBySequenceNo(),
        //            User = user,
        //            Source = source,
        //            Operation = MessageRepository.GetMessage(operation),
        //            Object = obj,
        //            ObjectValue = objectValue,
        //            CreateTime = DateTime.Now,
        //            Result =
        //                status
        //                    ? MessageRepository.GetMessage(StaticParameterForMessage.Success)
        //                    : MessageRepository.GetMessage(StaticParameterForMessage.Failure),
        //            Context = context
        //        };
        //        OperationTracingRepository.Create(operationTracing);
        //    }
        //}

        protected void RemoveDeviceAlarm(string alarm, string source, string alarmObject)
        {
            var alarmEventRecordRepository = ServiceLocator.Current.GetInstance<IAlarmEventRecordRepository>();
            var valueList = alarmEventRecordRepository.GetAll()
                .Where(
                    e =>
                        (e.Code == alarm) && (e.Source == source) && (e.Object == alarmObject) &&
                        (e.Status <= AlarmEventStatus.Removed)).ToList();
            if (valueList.Any())
            {
                foreach (var alarmEventRecord in valueList)
                {
                    alarmEventRecord.Status = AlarmEventStatus.Removed;
                    alarmEventRecord.UpdateBy = source;
                    alarmEventRecordRepository.Update(alarmEventRecord);
                }
            }
        }

        protected void CreateDisplayMessage(string source, string sourceAddress, string destinationAddress,
            string message, string messageType)
        {
            var valueList = _displayMessageRepository.GetAll()
                .Where(
                    e =>
                        (e.Source == source) && (e.SourceAddress == sourceAddress) &&
                        (e.DestinationAddress == destinationAddress) &&
                        (e.Message == message) && (e.Status < DisplayMessageStatus.Completed));
            if (valueList.Any())
            {
                return;
            }

            var alarmRecord = new DisplayMessage
            {
                Id = Guid.NewGuid().ToString("N"),
                Source = source,
                SourceAddress = sourceAddress,
                DestinationAddress = destinationAddress,
                MessageType = messageType,
                Message = message,
                CreateBy = source,
                CreateTime = DateTime.Now,
                Status = DisplayMessageStatus.Create
            };

            _displayMessageRepository.Create(alarmRecord);
        }
    }
}