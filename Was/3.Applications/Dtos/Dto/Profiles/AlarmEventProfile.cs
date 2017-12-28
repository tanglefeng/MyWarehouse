using AutoMapper;
using Kengic.Was.Application.WasModel.Dto.AlarmEvents;
using Kengic.Was.Domain.Entity.AlarmEvent;

namespace Kengic.Was.Application.WasModel.Dto.Profiles
{
    internal class AlarmEventProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<AlarmEventRecord, AlarmEventRecordDto>();
            //.ForMember(cv => cv.AlarmTypeName,
            //    cp => cp.MapFrom(cv => cv.Type == null ? null : cv.Type.Name))
            //; //配置AlarmRecord和Alarm的关联对应（后边部分）
            CreateMap<AlarmEventType, AlarmEventTypeDto>();

            CreateMap<AlarmEventRecordDto, AlarmEventRecord>();
            CreateMap<AlarmEventTypeDto, AlarmEventType>();


            //CreateMap<AlarmEventType, ForeignAlarmEventType>();
        }
    }
}