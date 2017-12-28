using System;
using System.Collections.Generic;
using System.ServiceModel;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.Vip;

namespace Kengic.Was.Wcf.IVip
{
    [ServiceContract]
    [ServiceKnownType(typeof (VipPackageMessageWorkTaskDto))]
    [ServiceKnownType(typeof (VipSourceWorkTaskDto))]
    public interface IVipService
    {
        [OperationContract]
        Tuple<bool, string> TerminateSourceWorkTask(string message);

        [OperationContract]
        Tuple<bool, string> CancelSourceWorkTask(string message);

        [OperationContract]
        Tuple<bool, string> CancelPackageMessageWorkTask(string message);

        [OperationContract]
        Tuple<bool, string> TerminatePackageMessageSubWorkTask(string message);


        [OperationContract]
        string GetDataFromSourceWorkTask(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromPackageMessageWorkTask(List<DynamicQueryDto> dynamicQueryMethods);
    }
}