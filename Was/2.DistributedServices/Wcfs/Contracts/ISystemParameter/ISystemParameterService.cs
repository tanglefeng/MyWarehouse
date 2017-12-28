using System;
using System.Collections.Generic;
using System.ServiceModel;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.SystemParameters;
using Kengic.Was.DistributedServices.Common.ExceptionHandings;

namespace Kengic.Was.Wcf.ISystemParameters
{
    [ServiceContract]
    [StandardFaults]
    public interface ISystemParameterService
    {
        //[OperationContract]
        //IEnumerable<SystemParameterDto> GetSystemParameter();

        //[OperationContract]
        //SystemParameterDto GetSystemParameterById(string id);

        [OperationContract]
        Tuple<bool, string> CreateSystemParameter(SystemParameterDto value);

        [OperationContract]
        Tuple<bool, string> UpdateSystemParameter(SystemParameterDto value);

        [OperationContract]
        Tuple<bool, string> RemoveSystemParameter(SystemParameterDto value);

        //[OperationContract]
        //IEnumerable<SystemParameterTemplateDto> GetSystemParameterTemplate();

        //[OperationContract]
        //SystemParameterTemplateDto GetSystemParameterTemplateById(string id);

        [OperationContract]
        Tuple<bool, string> CreateSystemParameterTemplate(SystemParameterTemplateDto value);

        [OperationContract]
        Tuple<bool, string> UpdateSystemParameterTemplate(SystemParameterTemplateDto value);

        [OperationContract]
        Tuple<bool, string> RemoveSystemParameterTemplate(SystemParameterTemplateDto value);

        [OperationContract]
        string GetDataFromSystemParameterTemplate(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromSystemParameter(List<DynamicQueryDto> dynamicQueryMethods);
    }
}