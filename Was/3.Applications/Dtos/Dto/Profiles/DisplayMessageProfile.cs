using AutoMapper;
using Kengic.Was.Application.WasModel.Dto.DisplayMessages;
using Kengic.Was.Domain.Entity.DisplayMessage;

namespace Kengic.Was.Application.WasModel.Dto.Profiles
{
    internal class DisplayMessageProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<DisplayMessage, DisplayMessageDto>();

            CreateMap<DisplayMessageDto, DisplayMessage>();
        }
    }
}