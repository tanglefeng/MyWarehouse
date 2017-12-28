using System;
using System.Collections.Generic;
using System.ServiceModel;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.SystemSequences;
using Kengic.Was.DistributedServices.Common.ExceptionHandings;

namespace Kengic.Was.Wcf.ISystemSequences
{
    [ServiceContract]
    [StandardFaults]
    public interface ISystemSequenceService
    {
        [OperationContract]
        Tuple<bool, string> CreateSystemSequence(SystemSequenceDto value);

        [OperationContract]
        Tuple<bool, string> UpdateSystemSequence(SystemSequenceDto value);

        [OperationContract]
        Tuple<bool, string> RemoveSystemSequence(SystemSequenceDto value);

        [OperationContract]
        string GetDataFromSystemSequence(List<DynamicQueryDto> dynamicQueryMethods);
    }
}