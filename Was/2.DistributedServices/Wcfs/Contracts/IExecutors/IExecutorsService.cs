using System;
using System.Collections.Generic;
using System.ServiceModel;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.Executors;
using Kengic.Was.DistributedServices.Common.ExceptionHandings;

namespace Kengic.Was.Wcf.IExecutors
{
    [ServiceContract]
    [StandardFaults]
    public interface IExecutorsService
    {
        [OperationContract]
        Tuple<bool, string> CreateExecutor(WasExecutorDto value);

        [OperationContract]
        Tuple<bool, string> UpdateExecutor(WasExecutorDto value);

        [OperationContract]
        Tuple<bool, string> RemoveExecutor(WasExecutorDto value);

        [OperationContract]
        string GetDataFromExecutor(List<DynamicQueryDto> dynamicQueryMethods);
    }
}