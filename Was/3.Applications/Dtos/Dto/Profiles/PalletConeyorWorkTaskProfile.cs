using AutoMapper;
using Kengic.Was.Application.WasModel.Dto.PalletConveyorWorkTasks;
using Kengic.Was.Domain.Entity.PalletConveyor;

namespace Kengic.Was.Application.WasModel.Dto.Profiles
{
    internal class PalletConeyorWorkTaskProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<PalletConveyorSourceWorkTask, PalletConveyorSourceWorkTaskDto>();

            CreateMap<PalletConveyorSourceWorkTaskDto, PalletConveyorSourceWorkTask>();

            CreateMap<PalletConveyorMessageWorkTask, PalletConveyorMessageWorkTaskDto>();

            CreateMap<PalletConveyorMessageWorkTaskDto, PalletConveyorMessageWorkTask>();

            CreateMap<PalletConveyorPkMsgWorkTask, PalletConveyorPkMsgWorkTaskDto>();

            CreateMap<PalletConveyorPkMsgWorkTaskDto, PalletConveyorPkMsgWorkTask>();
        }
    }
}