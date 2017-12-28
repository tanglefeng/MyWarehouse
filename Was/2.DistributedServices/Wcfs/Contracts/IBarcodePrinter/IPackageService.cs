using System;
using System.Collections.Generic;
using System.ServiceModel;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.PackageWorkTasks;
using Kengic.Was.DistributedServices.Common.ExceptionHandings;

namespace Kengic.Was.Wcf.IPackage
{
    [ServiceContract]
    [StandardFaults]
    [ServiceKnownType(typeof (PackageSourceWorkTaskDto))]
    public interface IPackageService
    {
        [OperationContract]
        Tuple<bool, string> CreateSourceWorkTask(string message);

        [OperationContract]
        Tuple<bool, string> TerminateSourceWorkTask(string message);

        [OperationContract]
        Tuple<bool, string> CancelSourceWorkTask(string message);

        [OperationContract]
        Tuple<bool, string> RenewSourceWorkTask(string message);

        [OperationContract]
        string GetDataFromPackageSourceWorkTask(List<DynamicQueryDto> dynamicQueryMethods);
    }
}