using System;
using System.Collections.Generic;
using System.ServiceModel;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.AlarmEvents;
using Kengic.Was.DistributedServices.Common.ExceptionHandings;

namespace Kengic.Was.Wcf.IAlarmEvent
{
    [ServiceContract]
    [StandardFaults]
    public interface IAlarmEventService
    {
        [OperationContract]
        Tuple<bool, string> CreateAlarmEventType(AlarmEventTypeDto value);

        [OperationContract]
        Tuple<bool, string> RemoveAlarmEventType(AlarmEventTypeDto value);

        [OperationContract]
        Tuple<bool, string> UpdateAlarmEventType(AlarmEventTypeDto value);

        [OperationContract]
        IEnumerable<AlarmEventRecordDto> GetAlarmRecordListWithStatus();

        [OperationContract]
        Tuple<bool, string> CreateAlarmEventRecord(AlarmEventRecordDto value);

        [OperationContract]
        Tuple<bool, string> UpdateAlarmEventRecord(AlarmEventRecordDto value);

        [OperationContract]
        Tuple<bool, string> RemoveAlarmEventRecord(AlarmEventRecordDto value);

        [OperationContract]
        string GetDataFromAlarmEventRecord(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromAlarmEventType(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        int GetDataActiveAlarmNumber();
    }
}