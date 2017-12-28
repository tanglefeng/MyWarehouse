using System;
using System.Collections.Generic;
using System.ServiceModel;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.DisplayMessages;
using Kengic.Was.DistributedServices.Common.ExceptionHandings;

namespace Kengic.Was.Wcf.IDisplayMessages
{
    [ServiceContract]
    [StandardFaults]
    public interface IDisplayMessageService
    {
        [OperationContract]
        Tuple<bool, string> Create(DisplayMessageDto value);

        [OperationContract]
        Tuple<bool, string> Update(DisplayMessageDto value);

        [OperationContract]
        Tuple<bool, string> Remove(DisplayMessageDto value);

        [OperationContract]
        string GetDataFromDisplayMessage(List<DynamicQueryDto> dynamicQueryMethods);
    }
}