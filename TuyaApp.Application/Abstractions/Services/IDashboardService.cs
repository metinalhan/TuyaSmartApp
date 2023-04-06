using System.Collections.Generic;
using System.Threading.Tasks;
using TuyaApp.Application.Dtos;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Application.Abstractions.Services
{
    public interface IDashboardService
    {
        //Save all settings dashboard devices
        Task<bool> SaveDashboardAsync(List<DashboardSaveDTO> list);

        //Get all dashboard devices with settings
        Task<List<Dashboard>> GetAllDashboardDevicesAsync();
    }
}
