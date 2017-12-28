using System;
using System.Collections.Generic;
using Kengic.Was.Application.Services.AlarmEvent;
using Kengic.Was.Application.Services.SystemTracing;
using Kengic.Was.Application.WasModel.Common;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.AlarmEvents;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.DistributedServices.Common;
using Kengic.Was.Domain.Entity.AlarmEvent;
using Kengic.Was.Wcf.IAlarmEvent;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace Kengic.Was.Wcf.AlarmEvent
{
    [ExceptionShielding("WcfServicePolicy")]
    public class AlarmEventService : IAlarmEventService
    {
        private readonly IAlarmEventRecordApplicationService _alarmEventRecordApplicationService;
        private readonly IAlarmEventRecordRepository _alarmEventRecordRepository;
        private readonly IAlarmEventTypeApplicationService _alarmEventTypeApplicationService;
        private readonly IAlarmEventTypeRepository _alarmEventTypeRepository;
        private readonly IOperationTracingApplicationService _operationTracingApplicationService;

        public AlarmEventService(IAlarmEventTypeApplicationService alarmEventTypeApplicationService,
            IAlarmEventRecordApplicationService alarmEventRecordApplicationService,
            IOperationTracingApplicationService operationTracingApplicationService,
            IAlarmEventRecordRepository alarmEventRecordRepository,
            IAlarmEventTypeRepository alarmEventTypeRepository)
        {
            if (alarmEventTypeApplicationService == null)
            {
                throw new ArgumentNullException(nameof(alarmEventTypeApplicationService));
            }

            if (alarmEventRecordApplicationService == null)
            {
                throw new ArgumentNullException(nameof(alarmEventRecordApplicationService));
            }
            if (operationTracingApplicationService == null)
            {
                throw new ArgumentNullException(nameof(operationTracingApplicationService));
            }

            _alarmEventTypeApplicationService = alarmEventTypeApplicationService;
            _alarmEventRecordApplicationService = alarmEventRecordApplicationService;
            _operationTracingApplicationService = operationTracingApplicationService;
            _alarmEventRecordRepository = alarmEventRecordRepository;
            _alarmEventTypeRepository = alarmEventTypeRepository;
        }

        public Tuple<bool, string> CreateAlarmEventType(AlarmEventTypeDto value)
        {
            var alarmType = value.ProjectedAs<AlarmEventTypeDto, AlarmEventType>();
            var returnValue = _alarmEventTypeApplicationService.Create(alarmType);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create,
                GetType().Name,
                StaticParameterForMessage.AlarmEventType, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateAlarmEventType(AlarmEventTypeDto value)
        {
            var alarmType = value.ProjectedAs<AlarmEventTypeDto, AlarmEventType>();
            var returnValue = _alarmEventTypeApplicationService.Update(alarmType);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Update,
                GetType().Name,
                StaticParameterForMessage.AlarmEventType, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveAlarmEventType(AlarmEventTypeDto value)
        {
            var alarmType = value.ProjectedAs<AlarmEventTypeDto, AlarmEventType>();
            var returnValue = _alarmEventTypeApplicationService.Remove(alarmType);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Remove,
                GetType().Name,
                StaticParameterForMessage.AlarmEventType, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateAlarmEventRecord(AlarmEventRecordDto value)
        {
            var alarmRecord = value.ProjectedAs<AlarmEventRecordDto, AlarmEventRecord>();
            if (string.IsNullOrEmpty(alarmRecord.Id))
            {
                alarmRecord.Id = Guid.NewGuid().ToString("N");
            }

            var returnValue = _alarmEventRecordApplicationService.Create(alarmRecord);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create,
                GetType().Name,
                StaticParameterForMessage.AlarmEventRecord, value.Object, value.Object + ":" + value.Code,
                value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateAlarmEventRecord(AlarmEventRecordDto value)
        {
            var alarmRecord = value.ProjectedAs<AlarmEventRecordDto, AlarmEventRecord>();
            var returnValue = _alarmEventRecordApplicationService.Update(alarmRecord);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Update,
                GetType().Name,
                StaticParameterForMessage.AlarmEventRecord, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveAlarmEventRecord(AlarmEventRecordDto value)
        {
            var alarmRecord = value.ProjectedAs<AlarmEventRecordDto, AlarmEventRecord>();
            var returnValue = _alarmEventRecordApplicationService.Remove(alarmRecord);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Remove,
                GetType().Name,
                StaticParameterForMessage.AlarmEventRecord, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public IEnumerable<AlarmEventRecordDto> GetAlarmRecordListWithStatus()
        {
            var alarmRecordList = _alarmEventRecordApplicationService.GetWithStatus();
            return alarmRecordList?.ProjectedAsCollection<AlarmEventRecord, AlarmEventRecordDto>();
        }

        public string GetDataFromAlarmEventRecord(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_alarmEventRecordRepository, dynamicQueryMethods);

        public string GetDataFromAlarmEventType(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_alarmEventTypeRepository, dynamicQueryMethods);

        public int GetDataActiveAlarmNumber() => _alarmEventRecordApplicationService.GetDataActiveAlarmNumber();
    }
}