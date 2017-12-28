using AutoMapper;
using Kengic.Was.Application.WasModel.Dto.SystemParameters;
using Kengic.Was.Domain.Entity.SystemParameter;

namespace Kengic.Was.Application.WasModel.Dto.Profiles
{
    internal class SystemParameterProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<SystemParameter, SystemParameterDto>();

            CreateMap<SystemParameterDto, SystemParameter>();


            CreateMap<SystemParameterTemplate, SystemParameterTemplateDto>();

            CreateMap<SystemParameterTemplateDto, SystemParameterTemplate>();
        }
    }
}