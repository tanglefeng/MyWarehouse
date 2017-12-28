using System;
using System.Collections.Generic;
using System.ServiceModel;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Application.WasModel.Dto.Sorters.Inducts;
using Kengic.Was.Application.WasModel.Dto.Sorters.LogicalDestinations;
using Kengic.Was.Application.WasModel.Dto.Sorters.Parameters;
using Kengic.Was.Application.WasModel.Dto.Sorters.Plans;
using Kengic.Was.Application.WasModel.Dto.Sorters.Routings;
using Kengic.Was.Application.WasModel.Dto.Sorters.Scanners;
using Kengic.Was.Application.WasModel.Dto.Sorters.Shutes;
using Kengic.Was.Application.WasModel.Dto.Sorters.Sorters;
using Kengic.Was.Application.WasModel.Dto.Sorters.WorkTasks;
using Kengic.Was.DistributedServices.Common.ExceptionHandings;

namespace Kengic.Was.Wcf.ISorter
{
    [ServiceContract]
    [StandardFaults]
    [ServiceKnownType(typeof (SorterExecuteWorkTaskDto))]
    [ServiceKnownType(typeof (SorterMessageWorkTaskDto))]
    [ServiceKnownType(typeof (SorterSubWorkTaskDto))]
    public interface ISorterService
    {
        [OperationContract]
        string GetDataFromInduct(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromLogicalDestination(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromLogicalSorter(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromPhysicalSorter(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromRouting(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromScanner(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromShute(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromShuteType(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromSorterExecuteWorkTask(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromSorterMessageWorkTask(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromSorterParameter(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromSorterPlan(List<DynamicQueryDto> dynamicQueryMethods);

        [OperationContract]
        string GetDataFromSorterSubWorkTask(List<DynamicQueryDto> dynamicQueryMethods);

        //[OperationContract]
        //IEnumerable<InductDto> GetInduct();

        //[OperationContract]
        //InductDto GetInductById(string id);

        [OperationContract]
        Tuple<bool, string> CreateInduct(InductDto value);

        [OperationContract]
        Tuple<bool, string> UpdateInduct(InductDto value);

        [OperationContract]
        Tuple<bool, string> RemoveInduct(InductDto value);

        //[OperationContract]
        //IEnumerable<LogicalDestinationDto> GetLogicalDestination();

        //[OperationContract]
        //LogicalDestinationDto GetLogicalDestinationById(string id);

        [OperationContract]
        Tuple<bool, string> CreateLogicalDestination(LogicalDestinationDto value);

        [OperationContract]
        Tuple<bool, string> UpdateLogicalDestination(LogicalDestinationDto value);

        [OperationContract]
        Tuple<bool, string> RemoveLogicalDestination(LogicalDestinationDto value);

        //[OperationContract]
        //IEnumerable<LogicalSorterDto> GetLogicalSorter();

        //[OperationContract]
        //LogicalSorterDto GetLogicalSorterById(string id);

        [OperationContract]
        Tuple<bool, string> CreateLogicalSorter(LogicalSorterDto value);

        [OperationContract]
        Tuple<bool, string> UpdateLogicalSorter(LogicalSorterDto value);

        [OperationContract]
        Tuple<bool, string> RemoveLogicalSorter(LogicalSorterDto value);

        //[OperationContract]
        //IEnumerable<PhysicalSorterDto> GetPhysicalSorter();

        //[OperationContract]
        //PhysicalSorterDto GetPhysicalSorterById(string id);

        [OperationContract]
        Tuple<bool, string> CreatePhysicalSorter(PhysicalSorterDto value);

        [OperationContract]
        Tuple<bool, string> UpdatePhysicalSorter(PhysicalSorterDto value);

        [OperationContract]
        Tuple<bool, string> RemovePhysicalSorter(PhysicalSorterDto value);

        //[OperationContract]
        //IEnumerable<RoutingDto> GetRouting();

        //[OperationContract]
        //RoutingDto GetRoutingById(string id);

        [OperationContract]
        Tuple<bool, string> CreateRouting(RoutingDto value);

        [OperationContract]
        Tuple<bool, string> UpdateRouting(RoutingDto value);

        [OperationContract]
        Tuple<bool, string> RemoveRouting(RoutingDto value);

        //[OperationContract]
        //IEnumerable<ScannerDto> GetScanner();

        //[OperationContract]
        //ScannerDto GetScannerById(string id);

        [OperationContract]
        Tuple<bool, string> CreateScanner(ScannerDto value);

        [OperationContract]
        Tuple<bool, string> UpdateScanner(ScannerDto value);

        [OperationContract]
        Tuple<bool, string> RemoveScanner(ScannerDto value);

        //[OperationContract]
        //IEnumerable<ShuteDto> GetShute();

        //[OperationContract]
        //ShuteDto GetShuteById(string id);

        [OperationContract]
        Tuple<bool, string> CreateShute(ShuteDto value);

        [OperationContract]
        Tuple<bool, string> UpdateShute(ShuteDto value);

        [OperationContract]
        Tuple<bool, string> RemoveShute(ShuteDto value);

        //[OperationContract]
        //IEnumerable<ShuteTypeDto> GetShuteType();

        //[OperationContract]
        //ShuteTypeDto GetShuteTypeById(string id);

        [OperationContract]
        Tuple<bool, string> CreateShuteType(ShuteTypeDto value);

        [OperationContract]
        Tuple<bool, string> UpdateShuteType(ShuteTypeDto value);

        [OperationContract]
        Tuple<bool, string> RemoveShuteType(ShuteTypeDto value);

        //[OperationContract]
        //IEnumerable<SorterPlanDto> GetSorterPlan();

        //[OperationContract]
        //SorterPlanDto GetSorterPlanById(string id);

        [OperationContract]
        Tuple<bool, string> CreateSorterPlan(SorterPlanDto value);

        [OperationContract]
        Tuple<bool, string> UpdateSorterPlan(SorterPlanDto value);

        [OperationContract]
        Tuple<bool, string> RemoveSorterPlan(SorterPlanDto value);

        //[OperationContract]
        //IEnumerable<SorterExecuteWorkTaskDto> GetExecuteWorkTasks();

        //[OperationContract]
        //SorterExecuteWorkTaskDto GetExecuteWorkTaskById(string id);

        [OperationContract]
        Tuple<bool, string> CreateExecuteWorkTask(string message);

        [OperationContract]
        Tuple<bool, string> TerminateExecuteWorkTask(string message);

        [OperationContract]
        Tuple<bool, string> CancelExecuteWorkTask(string message);

        //[OperationContract]
        //IEnumerable<SorterSubWorkTaskDto> GetSubWorkTasks();

        //[OperationContract]
        //SorterSubWorkTaskDto GetSubWorkTaskById(string id);

        [OperationContract]
        Tuple<bool, string> CreateSubWorkTask(string message);

        [OperationContract]
        Tuple<bool, string> TerminateSubWorkTask(string message);

        [OperationContract]
        Tuple<bool, string> CancelSubWorkTask(string message);


        [OperationContract]
        Tuple<bool, string> CreateMessage(string message);

        [OperationContract]
        Tuple<bool, string> TerminateMessage(string message);

        [OperationContract]
        Tuple<bool, string> CancelMessage(string message);

        [OperationContract]
        Tuple<bool, string> RenewMessage(string message);

        [OperationContract]
        Tuple<bool, string> CreateSorterParameter(SorterParameterDto value);

        [OperationContract]
        Tuple<bool, string> UpdateSorterParameter(SorterParameterDto value);

        [OperationContract]
        Tuple<bool, string> RemoveSorterParameter(SorterParameterDto value);
    }
}