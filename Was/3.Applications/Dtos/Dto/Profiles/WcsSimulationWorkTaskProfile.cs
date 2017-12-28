using AutoMapper;
using Kengic.Was.Application.WasModel.Dto.WcsSimulations;
using Kengic.Was.Domain.Entity.WcsSimulation;

namespace Kengic.Was.Application.WasModel.Dto.Profiles
{
    internal class WcsSimulationWorkTaskProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<WcsSimulationSourceWorkTask, WcsSimulationSourceWorkTaskDto>();

            CreateMap<WcsSimulationSourceWorkTaskDto, WcsSimulationSourceWorkTask>();
        }
    }
}