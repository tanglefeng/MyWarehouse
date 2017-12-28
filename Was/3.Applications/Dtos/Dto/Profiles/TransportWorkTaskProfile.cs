using AutoMapper;
using Kengic.Was.Application.WasModel.Dto.Transports;
using Kengic.Was.Domain.Entity.Transport;

namespace Kengic.Was.Application.WasModel.Dto.Profiles
{
    internal class TransportWorkTaskProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<TransportSourceWorkTask, TransportSourceWorkTaskDto>();

            CreateMap<TransportSourceWorkTaskDto, TransportSourceWorkTask>();

            CreateMap<TransportExecuteWorkTask, TransportExecuteWorkTaskDto>();

            CreateMap<TransportExecuteWorkTaskDto, TransportExecuteWorkTask>();

            CreateMap<TransportSubWorkTask, TransportSubWorkTaskDto>();

            CreateMap<TransportSubWorkTaskDto, TransportSubWorkTask>();
        }
    }
}