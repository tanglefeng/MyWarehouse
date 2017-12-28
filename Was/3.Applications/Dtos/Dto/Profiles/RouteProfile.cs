using AutoMapper;
using Kengic.Was.Application.WasModel.Dto.Routes;
using Kengic.Was.Domain.Entity.Route;

namespace Kengic.Was.Application.WasModel.Dto.Profiles
{
    internal class RouteProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<RouteMap, RouteMapDto>();

            CreateMap<RouteMapDto, RouteMap>();

            CreateMap<RouteNode, RouteNodeDto>();

            CreateMap<RouteNodeDto, RouteNode>();

            CreateMap<RouteEdge, RouteEdgeDto>();

            CreateMap<RouteEdgeDto, RouteEdge>();
        }
    }
}