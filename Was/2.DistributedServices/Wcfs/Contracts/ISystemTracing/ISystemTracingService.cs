using System;
using System.Collections.Generic;
using System.ServiceModel;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.SystemTracings;
using Kengic.Was.DistributedServices.Common.ExceptionHandings;

namespace Kengic.Was.Wcf.ISystemTracing
{
    [ServiceContract]
    [StandardFaults]
    public interface ISystemTracingService
    {
        [OperationContract]
        Tuple<bool, string> CreateOperationTracing(OperationTracingDto value);

        [OperationContract]
        Tuple<bool, string> RemoveOperationTracing(OperationTracingDto value);

        [OperationContract]
        Tuple<bool, string> UpdateOperationTracing(OperationTracingDto value);

        [OperationContract]
        string GetDataFromOpertionTracing(List<DynamicQueryDto> dynamicQueryMethods);
    }
}