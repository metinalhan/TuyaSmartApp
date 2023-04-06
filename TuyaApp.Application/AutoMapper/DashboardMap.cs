using AutoMapper;
using TuyaApp.Application.Dtos;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Application.AutoMapper
{
    public class DashboardMap:Profile
    {
        // This class is used to define a mapping between the Dashboard and DashboardSaveDTO classes using the AutoMapper library.

        public DashboardMap()
        {
            CreateMap<DashboardSaveDTO,Dashboard>().ReverseMap();
        }
    }
}
