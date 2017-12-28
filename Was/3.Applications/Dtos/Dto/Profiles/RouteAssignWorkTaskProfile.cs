using AutoMapper;
using Kengic.Was.Application.WasModel.Dto.RouteAssigns;
using Kengic.Was.Domain.Entity.RouteAssign;

namespace Kengic.Was.Application.WasModel.Dto.Profiles
{
    internal class RouteAssignWorkTaskProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<RouteAssignExecuteWorkTask, RouteAssignExecuteWorkTaskDto>();

            CreateMap<RouteAssignExecuteWorkTaskDto, RouteAssignExecuteWorkTask>();

            CreateMap<RouteAssignSubWorkTask, RouteAssignSubWorkTaskDto>();

            CreateMap<RouteAssignSubWorkTaskDto, RouteAssignSubWorkTask>();
        }
    }
}