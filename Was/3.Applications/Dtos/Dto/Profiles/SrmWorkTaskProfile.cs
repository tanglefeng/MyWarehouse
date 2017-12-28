using AutoMapper;
using Kengic.Was.Application.WasModel.Dto.Srms;
using Kengic.Was.Domain.Entity.StorageRetrievalMachine;

namespace Kengic.Was.Application.WasModel.Dto.Profiles
{
    internal class SrmWorkTaskProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<SrmSourceWorkTask, SrmSourceWorkTaskDto>();

            CreateMap<SrmSourceWorkTaskDto, SrmSourceWorkTask>();

            CreateMap<SrmMessageWorkTask, SrmMessageWorkTaskDto>();

            CreateMap<SrmMessageWorkTaskDto, SrmMessageWorkTask>();
        }
    }
}