using AutoMapper;
using Kengic.Was.Application.WasModel.Dto.Executors;
using Kengic.Was.Domain.Entity.Executor;

namespace Kengic.Was.Application.WasModel.Dto.Profiles
{
    internal class ExecutorProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<WasExecutor, WasExecutorDto>();

            CreateMap<WasExecutorDto, WasExecutor>();
        }
    }
}