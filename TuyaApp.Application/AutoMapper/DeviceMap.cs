using AutoMapper;
using TuyaApp.Application.Dtos;
using TuyaApp.Application.Extensions;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Application.AutoMapper
{
    public class DeviceMap : Profile
    {
        // This class is used to define a mapping between the Device and DeviceDTO classes using the AutoMapper library.
        public DeviceMap()
        {
            CreateMap<Device, DeviceDTO>()
                .ForMember(dest => dest.Id, action => action.MapFrom(dest => dest.Id))
                .ForMember(dest => dest.DeviceTuyaId, action => action.MapFrom(dest => dest.DeviceTuyaId))
                .ForMember(dest => dest.DeviceName, action => action.MapFrom(dest => dest.DeviceName))
                .ForMember(dest => dest.NumberOfSwitch, action => action.MapFrom(dest => dest.NumberOfSwitch))
                .ForMember(dest => dest.IsDefault, action => action.MapFrom(dest => dest.IsDefault))
                .ForMember(dest => dest.DeviceType, action => action.MapFrom(dest => dest.DeviceType.ConvertToEnum()))
                .ReverseMap();

        }
    }

}
