using AutoMapper;
using Kengic.Was.Application.WasModel.Dto.SystemSequences;
using Kengic.Was.Domain.Entity.SystemSequence;

namespace Kengic.Was.Application.WasModel.Dto.Profiles
{
    internal class SystemSequenceProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<SystemSequence, SystemSequenceDto>();

            CreateMap<SystemSequenceDto, SystemSequence>();
        }
    }
}