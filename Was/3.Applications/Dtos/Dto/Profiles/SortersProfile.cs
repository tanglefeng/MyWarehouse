using AutoMapper;
using Kengic.Was.Application.WasModel.Dto.Sorters.Inducts;
using Kengic.Was.Application.WasModel.Dto.Sorters.LogicalDestinations;
using Kengic.Was.Application.WasModel.Dto.Sorters.Parameters;
using Kengic.Was.Application.WasModel.Dto.Sorters.Plans;
using Kengic.Was.Application.WasModel.Dto.Sorters.Routings;
using Kengic.Was.Application.WasModel.Dto.Sorters.Scanners;
using Kengic.Was.Application.WasModel.Dto.Sorters.Shutes;
using Kengic.Was.Application.WasModel.Dto.Sorters.Sorters;
using Kengic.Was.Application.WasModel.Dto.Sorters.WorkTasks;
using Kengic.Was.Domain.Entity.Sorter.Inducts;
using Kengic.Was.Domain.Entity.Sorter.LogicalDestinations;
using Kengic.Was.Domain.Entity.Sorter.Parameters;
using Kengic.Was.Domain.Entity.Sorter.Plans;
using Kengic.Was.Domain.Entity.Sorter.Routings;
using Kengic.Was.Domain.Entity.Sorter.Scanners;
using Kengic.Was.Domain.Entity.Sorter.Shutes;
using Kengic.Was.Domain.Entity.Sorter.Sorters;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;

namespace Kengic.Was.Application.WasModel.Dto.Profiles
{
    internal class SortersProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Induct, InductDto>();
            CreateMap<InductDto, Induct>();

            CreateMap<LogicalDestination, LogicalDestinationDto>();
            CreateMap<LogicalDestinationDto, LogicalDestination>();
            CreateMap<RoutingDto, Routing>();
            CreateMap<Routing, RoutingDto>();
            CreateMap<ScannerDto, Scanner>();
            CreateMap<Scanner, ScannerDto>();
            CreateMap<ShuteDto, Shute>();
            CreateMap<Shute, ShuteDto>();


            CreateMap<ShuteTypeDto, ShuteType>();
            CreateMap<ShuteType, ShuteTypeDto>();

            CreateMap<SorterPlanDto, SorterPlan>();
            CreateMap<SorterPlan, SorterPlanDto>();

            CreateMap<LogicalSorterDto, LogicalSorter>();
            CreateMap<LogicalSorter, LogicalSorterDto>();

            CreateMap<PhysicalSorterDto, PhysicalSorter>();
            CreateMap<PhysicalSorter, PhysicalSorterDto>();

            CreateMap<SorterExecuteWorkTaskDto, SorterExecuteWorkTask>();
            CreateMap<SorterExecuteWorkTask, SorterExecuteWorkTaskDto>();

            CreateMap<SorterSubWorkTaskDto, SorterSubWorkTask>();
            CreateMap<SorterSubWorkTask, SorterSubWorkTaskDto>();

            CreateMap<SorterParameterDto, SorterParameter>();
            CreateMap<SorterParameter, SorterParameterDto>();

            CreateMap<SorterMessageWorkTaskDto, SorterMessageWorkTask>();
            CreateMap<SorterMessageWorkTask, SorterMessageWorkTaskDto>();
        }
    }
}