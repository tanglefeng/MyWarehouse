using System;
using System.Collections.Generic;
using Kengic.Was.Application.Services.Sorter.Inducts;
using Kengic.Was.Application.Services.Sorter.LogicalDestinations;
using Kengic.Was.Application.Services.Sorter.Routings;
using Kengic.Was.Application.Services.Sorter.Scanners;
using Kengic.Was.Application.Services.Sorter.Shutes;
using Kengic.Was.Application.Services.Sorter.SorterParameters;
using Kengic.Was.Application.Services.Sorter.SorterPlans;
using Kengic.Was.Application.Services.Sorter.Sorters;
using Kengic.Was.Application.Services.SystemTracing;
using Kengic.Was.Application.WasModel.Common;
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
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.DistributedServices.Common;
using Kengic.Was.Domain.Entity.Sorter.Inducts;
using Kengic.Was.Domain.Entity.Sorter.LogicalDestinations;
using Kengic.Was.Domain.Entity.Sorter.Parameters;
using Kengic.Was.Domain.Entity.Sorter.Plans;
using Kengic.Was.Domain.Entity.Sorter.Routings;
using Kengic.Was.Domain.Entity.Sorter.Scanners;
using Kengic.Was.Domain.Entity.Sorter.Shutes;
using Kengic.Was.Domain.Entity.Sorter.Sorters;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;
using Kengic.Was.Wcf.ISorter;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace Kengic.Was.Wcf.Sorter
{
    [ExceptionShielding("WcfServicePolicy")]
    public class SorterService : ISorterService
    {
        private readonly IInductApplicationService _iInductApplicationService;
        private readonly ILogicalDestinationApplicationService _iLogicalDestinationApplicationService;
        private readonly ILogicalSorterApplicationService _iLogicalSorterApplicationService;
        private readonly IInductRepository _inductRepository;
        private readonly IPhysicalSorterApplicationService _iPhysicalSorterApplicationService;
        private readonly IRoutingApplicationService _iRoutingApplicationService;
        private readonly IScannerApplicationService _iScannerApplicationService;
        private readonly IShuteApplicationService _iShuteApplicationService;
        private readonly IShuteTypeApplicationService _iShuteTypeApplicationService;
        private readonly ISorterParameterApplicationService _iSorterParameterApplicationService;
        private readonly ISorterPlanApplicationService _iSorterPlanApplicationService;
        private readonly ILogicalDestinationRepository _logicalDestinationRepository;
        private readonly ILogicalSorterRepository _logicalSorterRepository;
        private readonly IOperationTracingApplicationService _operationTracingApplicationService;
        private readonly IPhysicalSorterRepository _physicalSorterRepository;
        private readonly IRoutingRepository _routingRepository;
        private readonly IScannerRepository _scannerRepository;
        private readonly IShuteRepository _shuteRepository;
        private readonly IShuteTypeRepository _shuteTypeRepository;
        private readonly ISorterExecuteWorkTaskRepository _sorterExecuteWorkTaskRepository;
        private readonly ISorterMessageWorkTaskRepository _sorterMessageWorkTaskRepository;
        private readonly ISorterParameterRepository _sorterParameterRepository;
        private readonly ISorterPlanRepository _sorterPlanRepository;
        private readonly ISorterSubWorkTaskRepository _sorterSubWorkTaskRepository;

        public SorterService(IInductApplicationService inductApplicationService,
            ILogicalDestinationApplicationService iLogicalDestinationApplicationService1,
            IRoutingApplicationService iRoutingApplicationService,
            IScannerApplicationService iScannerApplicationService,
            IShuteApplicationService iShuteApplicationService,
            IShuteTypeApplicationService iShuteTypeApplicationService,
            ILogicalSorterApplicationService iLogicalSorterApplicationService,
            IPhysicalSorterApplicationService iPhysicalSorterApplicationService,
            ISorterPlanApplicationService iSorterPlanApplicationService,
            ISorterParameterApplicationService iSorterParameterApplicationService,
            IOperationTracingApplicationService operationTracingApplicationService,
            IInductRepository inductRepository,
            ILogicalDestinationRepository logicalDestinationRepository,
            IRoutingRepository routingRepository,
            IScannerRepository scannerRepository,
            IShuteRepository shuteRepository,
            IShuteTypeRepository shuteTypeRepository,
            ILogicalSorterRepository logicalSorterRepository,
            IPhysicalSorterRepository physicalSorterRepository,
            ISorterParameterRepository sorterParameterRepository,
            ISorterPlanRepository sorterPlanRepository,
            ISorterExecuteWorkTaskRepository sorterExecuteWorkTaskRepository,
            ISorterMessageWorkTaskRepository sorterMessageWorkTaskRepository,
            ISorterSubWorkTaskRepository sorterSubWorkTaskRepository)
        {
            if (inductApplicationService == null)
            {
                throw new ArgumentNullException(nameof(inductApplicationService));
            }
            if (iLogicalDestinationApplicationService1 == null)
            {
                throw new ArgumentNullException(nameof(iLogicalDestinationApplicationService1));
            }
            if (iRoutingApplicationService == null)
            {
                throw new ArgumentNullException(nameof(iRoutingApplicationService));
            }
            if (iScannerApplicationService == null)
            {
                throw new ArgumentNullException(nameof(iScannerApplicationService));
            }
            if (iShuteApplicationService == null)
            {
                throw new ArgumentNullException(nameof(iShuteApplicationService));
            }
            if (iShuteTypeApplicationService == null)
            {
                throw new ArgumentNullException(nameof(iShuteTypeApplicationService));
            }
            if (iLogicalSorterApplicationService == null)
            {
                throw new ArgumentNullException(nameof(iLogicalSorterApplicationService));
            }
            if (iPhysicalSorterApplicationService == null)
            {
                throw new ArgumentNullException(nameof(iPhysicalSorterApplicationService));
            }
            if (iSorterPlanApplicationService == null)
            {
                throw new ArgumentNullException(nameof(iSorterPlanApplicationService));
            }
            if (iSorterParameterApplicationService == null)
            {
                throw new ArgumentNullException(nameof(iSorterParameterApplicationService));
            }
            if (operationTracingApplicationService == null)
            {
                throw new ArgumentNullException(nameof(operationTracingApplicationService));
            }
            _iInductApplicationService = inductApplicationService;
            _iLogicalDestinationApplicationService = iLogicalDestinationApplicationService1;
            _iRoutingApplicationService = iRoutingApplicationService;
            _iScannerApplicationService = iScannerApplicationService;
            _iShuteApplicationService = iShuteApplicationService;
            _iShuteTypeApplicationService = iShuteTypeApplicationService;
            _iLogicalSorterApplicationService = iLogicalSorterApplicationService;
            _iPhysicalSorterApplicationService = iPhysicalSorterApplicationService;
            _iSorterPlanApplicationService = iSorterPlanApplicationService;
            _iSorterParameterApplicationService = iSorterParameterApplicationService;
            _operationTracingApplicationService = operationTracingApplicationService;
            _inductRepository = inductRepository;
            _logicalDestinationRepository = logicalDestinationRepository;
            _routingRepository = routingRepository;
            _scannerRepository = scannerRepository;
            _shuteRepository = shuteRepository;
            _shuteTypeRepository = shuteTypeRepository;
            _logicalSorterRepository = logicalSorterRepository;
            _physicalSorterRepository = physicalSorterRepository;
            _sorterParameterRepository = sorterParameterRepository;
            _sorterPlanRepository = sorterPlanRepository;
            _sorterExecuteWorkTaskRepository = sorterExecuteWorkTaskRepository;
            _sorterMessageWorkTaskRepository = sorterMessageWorkTaskRepository;
            _sorterSubWorkTaskRepository = sorterSubWorkTaskRepository;
        }

        public string GetDataFromInduct(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_inductRepository, dynamicQueryMethods);

        public string GetDataFromLogicalDestination(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_logicalDestinationRepository, dynamicQueryMethods);

        public string GetDataFromLogicalSorter(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_logicalSorterRepository, dynamicQueryMethods);

        public string GetDataFromPhysicalSorter(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_physicalSorterRepository, dynamicQueryMethods);

        public string GetDataFromRouting(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_routingRepository, dynamicQueryMethods);

        public string GetDataFromScanner(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_scannerRepository, dynamicQueryMethods);

        public string GetDataFromShute(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_shuteRepository, dynamicQueryMethods);

        public string GetDataFromShuteType(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_shuteTypeRepository, dynamicQueryMethods);

        public string GetDataFromSorterExecuteWorkTask(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_sorterExecuteWorkTaskRepository, dynamicQueryMethods);

        public string GetDataFromSorterMessageWorkTask(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_sorterMessageWorkTaskRepository, dynamicQueryMethods);

        public string GetDataFromSorterParameter(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_sorterParameterRepository, dynamicQueryMethods);

        public string GetDataFromSorterPlan(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_sorterPlanRepository, dynamicQueryMethods);

        public string GetDataFromSorterSubWorkTask(List<DynamicQueryDto> dynamicQueryMethods)
            => DynamicQueryDataHelper.GetData(_sorterSubWorkTaskRepository, dynamicQueryMethods);

        public Tuple<bool, string> CreateInduct(InductDto value)
        {
            var values = value.ProjectedAs<InductDto, Induct>();
            var returnValue = _iInductApplicationService.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create, GetType().Name,
                StaticParameterForMessage.Induct, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateInduct(InductDto value)
        {
            var values = value.ProjectedAs<InductDto, Induct>();
            var returnValue = _iInductApplicationService.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Update, GetType().Name,
                StaticParameterForMessage.Induct, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveInduct(InductDto value)
        {
            var values = value.ProjectedAs<InductDto, Induct>();
            var returnValue = _iInductApplicationService.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Remove, GetType().Name,
                StaticParameterForMessage.Induct, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateLogicalDestination(LogicalDestinationDto value)
        {
            var values = value.ProjectedAs<LogicalDestinationDto, LogicalDestination>();
            var returnValue = _iLogicalDestinationApplicationService.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create, GetType().Name,
                StaticParameterForMessage.LogicalDestination, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateLogicalDestination(LogicalDestinationDto value)
        {
            var values = value.ProjectedAs<LogicalDestinationDto, LogicalDestination>();
            var returnValue = _iLogicalDestinationApplicationService.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Update, GetType().Name,
                StaticParameterForMessage.LogicalDestination, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveLogicalDestination(LogicalDestinationDto value)
        {
            var values = value.ProjectedAs<LogicalDestinationDto, LogicalDestination>();
            var returnValue = _iLogicalDestinationApplicationService.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Remove, GetType().Name,
                StaticParameterForMessage.LogicalDestination, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateLogicalSorter(LogicalSorterDto value)
        {
            var values = value.ProjectedAs<LogicalSorterDto, LogicalSorter>();
            var returnValue = _iLogicalSorterApplicationService.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create, GetType().Name,
                StaticParameterForMessage.LogicalSorter, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateLogicalSorter(LogicalSorterDto value)
        {
            var values = value.ProjectedAs<LogicalSorterDto, LogicalSorter>();
            var returnValue = _iLogicalSorterApplicationService.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Update, GetType().Name,
                StaticParameterForMessage.LogicalSorter, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveLogicalSorter(LogicalSorterDto value)
        {
            var values = value.ProjectedAs<LogicalSorterDto, LogicalSorter>();
            var returnValue = _iLogicalSorterApplicationService.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Remove, GetType().Name,
                StaticParameterForMessage.LogicalSorter, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreatePhysicalSorter(PhysicalSorterDto value)
        {
            var values = value.ProjectedAs<PhysicalSorterDto, PhysicalSorter>();
            var returnValue = _iPhysicalSorterApplicationService.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create, GetType().Name,
                StaticParameterForMessage.PhysicalSorter, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdatePhysicalSorter(PhysicalSorterDto value)
        {
            var values = value.ProjectedAs<PhysicalSorterDto, PhysicalSorter>();
            var returnValue = _iPhysicalSorterApplicationService.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Update, GetType().Name,
                StaticParameterForMessage.PhysicalSorter, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemovePhysicalSorter(PhysicalSorterDto value)
        {
            var values = value.ProjectedAs<PhysicalSorterDto, PhysicalSorter>();
            var returnValue = _iPhysicalSorterApplicationService.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Remove, GetType().Name,
                StaticParameterForMessage.PhysicalSorter, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateRouting(RoutingDto value)
        {
            var values = value.ProjectedAs<RoutingDto, Routing>();
            var returnValue = _iRoutingApplicationService.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create, GetType().Name,
                StaticParameterForMessage.Routing, value.Id,
                value.SorterPlan + ":" + value.LogicalDestination + ":" + value.PhycialShute, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateRouting(RoutingDto value)
        {
            var values = value.ProjectedAs<RoutingDto, Routing>();
            var returnValue = _iRoutingApplicationService.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Update, GetType().Name,
                StaticParameterForMessage.Routing, value.Id,
                value.SorterPlan + ":" + value.LogicalDestination + ":" + value.PhycialShute, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveRouting(RoutingDto value)
        {
            var values = value.ProjectedAs<RoutingDto, Routing>();
            var returnValue = _iRoutingApplicationService.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Remove, GetType().Name,
                StaticParameterForMessage.Routing, value.Id,
                value.SorterPlan + ":" + value.LogicalDestination + ":" + value.PhycialShute, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateScanner(ScannerDto value)
        {
            var values = value.ProjectedAs<ScannerDto, Scanner>();
            var returnValue = _iScannerApplicationService.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create, GetType().Name,
                StaticParameterForMessage.Scanner, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateScanner(ScannerDto value)
        {
            var values = value.ProjectedAs<ScannerDto, Scanner>();
            var returnValue = _iScannerApplicationService.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Update, GetType().Name,
                StaticParameterForMessage.Scanner, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveScanner(ScannerDto value)
        {
            var values = value.ProjectedAs<ScannerDto, Scanner>();
            var returnValue = _iScannerApplicationService.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Remove, GetType().Name,
                StaticParameterForMessage.Scanner, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateShute(ShuteDto value)
        {
            var values = value.ProjectedAs<ShuteDto, Shute>();
            var returnValue = _iShuteApplicationService.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create, GetType().Name,
                StaticParameterForMessage.Shute, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateShute(ShuteDto value)
        {
            var values = value.ProjectedAs<ShuteDto, Shute>();
            var returnValue = _iShuteApplicationService.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Update, GetType().Name,
                StaticParameterForMessage.Shute, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveShute(ShuteDto value)
        {
            var values = value.ProjectedAs<ShuteDto, Shute>();
            var returnValue = _iShuteApplicationService.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Remove, GetType().Name,
                StaticParameterForMessage.Shute, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateShuteType(ShuteTypeDto value)
        {
            var values = value.ProjectedAs<ShuteTypeDto, ShuteType>();
            var returnValue = _iShuteTypeApplicationService.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create, GetType().Name,
                StaticParameterForMessage.ShuteType, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateShuteType(ShuteTypeDto value)
        {
            var values = value.ProjectedAs<ShuteTypeDto, ShuteType>();
            var returnValue = _iShuteTypeApplicationService.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Update, GetType().Name,
                StaticParameterForMessage.ShuteType, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveShuteType(ShuteTypeDto value)
        {
            var values = value.ProjectedAs<ShuteTypeDto, ShuteType>();
            var returnValue = _iShuteTypeApplicationService.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Remove, GetType().Name,
                StaticParameterForMessage.ShuteType, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateSorterPlan(SorterPlanDto value)
        {
            var values = value.ProjectedAs<SorterPlanDto, SorterPlan>();
            var returnValue = _iSorterPlanApplicationService.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create, GetType().Name,
                StaticParameterForMessage.SorterPlan, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateSorterPlan(SorterPlanDto value)
        {
            var values = value.ProjectedAs<SorterPlanDto, SorterPlan>();
            var returnValue = _iSorterPlanApplicationService.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Update, GetType().Name,
                StaticParameterForMessage.SorterPlan, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveSorterPlan(SorterPlanDto value)
        {
            var values = value.ProjectedAs<SorterPlanDto, SorterPlan>();
            var returnValue = _iSorterPlanApplicationService.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Remove, GetType().Name,
                StaticParameterForMessage.SorterPlan, value.Id, value.Id, value.ToShortString());
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateExecuteWorkTask(string message)
        {
            var workTask = message.JsonToValue<SorterExecuteWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncCreateSorterExecuteWorkTask", message, 5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.CreateBy,
                StaticParameterForMessage.Create,
                GetType().Name,
                StaticParameterForMessage.ExecuteWorkTask, workTask.ObjectToHandle,
                workTask.LogisticsBarcode + "->" + workTask.LogicalDestination, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> TerminateExecuteWorkTask(string message)
        {
            var workTask = message.JsonToValue<SorterExecuteWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncTerminateSorterExecuteWorkTask", message, 5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.TerminatedBy,
                StaticParameterForMessage.Terminate,
                GetType().Name,
                StaticParameterForMessage.ExecuteWorkTask, workTask.ObjectToHandle,
                workTask.Id, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CancelExecuteWorkTask(string message)
        {
            var workTask = message.JsonToValue<SorterExecuteWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncCancelSorterExecuteWorkTask", message, 5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.CancelledBy,
                StaticParameterForMessage.Cancel,
                GetType().Name,
                StaticParameterForMessage.ExecuteWorkTask, workTask.ObjectToHandle,
                workTask.Id, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateSubWorkTask(string message)
        {
            var workTask = message.JsonToValue<SorterSubWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncCreateSorterSubWorkTask", message, 5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.CreateBy,
                StaticParameterForMessage.Create,
                GetType().Name,
                StaticParameterForMessage.SubWorkTask, workTask.ObjectToHandle,
                workTask.LogicalSortter + "->" + workTask.RequestShuteAddr, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> TerminateSubWorkTask(string message)
        {
            var workTask = message.JsonToValue<SorterSubWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncTerminateSorterSubWorkTask", message, 5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.TerminatedBy,
                StaticParameterForMessage.Terminate,
                GetType().Name,
                StaticParameterForMessage.SubWorkTask, workTask.ObjectToHandle,
                workTask.Id, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CancelSubWorkTask(string message)
        {
            var workTask = message.JsonToValue<SorterSubWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncCancelSorterSubWorkTask", message, 5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.CancelledBy,
                StaticParameterForMessage.Cancel,
                GetType().Name,
                StaticParameterForMessage.Message, workTask.ObjectToHandle,
                workTask.Id, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateMessage(string message)
        {
            var workTask = message.JsonToValue<SorterMessageWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncCreateSorterMessageWorkTask", message, 5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.CreateBy,
                StaticParameterForMessage.Message,
                GetType().Name,
                StaticParameterForMessage.SubWorkTask, workTask.ObjectToHandle,
                workTask.Type + "->" + workTask.CurrentShuteAddr, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> TerminateMessage(string message)
        {
            var workTask = message.JsonToValue<SorterMessageWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncTerminateSorterMessageWorkTask", message, 5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.TerminatedBy,
                StaticParameterForMessage.Terminate,
                GetType().Name,
                StaticParameterForMessage.Message, workTask.ObjectToHandle,
                workTask.Id, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CancelMessage(string message)
        {
            var workTask = message.JsonToValue<SorterMessageWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncCancelSorterMessageWorkTask", message, 5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.CancelledBy,
                StaticParameterForMessage.Cancel,
                GetType().Name,
                StaticParameterForMessage.Message, workTask.ObjectToHandle,
                workTask.Id, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RenewMessage(string message)
        {
            var workTask = message.JsonToValue<SorterMessageWorkTaskDto>();
            var returnValue = SyncCallHelper.SyncCallActivityContract("SyncRenewSorterMessageWorkTask", message, 5);
            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, workTask.UpdateBy,
                StaticParameterForMessage.Renew,
                GetType().Name,
                StaticParameterForMessage.SubWorkTask, workTask.ObjectToHandle,
                workTask.Id, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> CreateSorterParameter(SorterParameterDto value)
        {
            var values = value.ProjectedAs<SorterParameterDto, SorterParameter>();
            var returnValue = _iSorterParameterApplicationService.Create(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.CreateBy,
                StaticParameterForMessage.Create, GetType().Name,
                StaticParameterForMessage.SorterParameter, value.Id, value.Value, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> UpdateSorterParameter(SorterParameterDto value)
        {
            var values = value.ProjectedAs<SorterParameterDto, SorterParameter>();
            var returnValue = _iSorterParameterApplicationService.Update(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Update, GetType().Name,
                StaticParameterForMessage.SorterParameter, value.Id, value.Value, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }

        public Tuple<bool, string> RemoveSorterParameter(SorterParameterDto value)
        {
            var values = value.ProjectedAs<SorterParameterDto, SorterParameter>();
            var returnValue = _iSorterParameterApplicationService.Remove(values);

            var operationTracing = OperationTracingHelper.GetOperationTracing(returnValue.Item1, value.UpdateBy,
                StaticParameterForMessage.Remove, GetType().Name,
                StaticParameterForMessage.SorterParameter, value.Id, value.Value, "");
            _operationTracingApplicationService.Create(operationTracing);

            return returnValue;
        }
    }
}