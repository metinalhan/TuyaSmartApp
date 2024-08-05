using System.Collections.Generic;
using System.Threading.Tasks;
using TuyaApp.Application.Dtos;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Application.Abstractions.Services
{
    // This is an interface definition for a service that provides functionality for interacting with the Tuya cloud.
    public interface ITuyaCloudService
    {
        // Checks the last state of the specified switch on the specified device.
        // Task<bool> CheckLastStateAsync(Device device,string sw_number);
        Task<List<RestResultDto>> CheckLastStateAsync(Device device, string sw_number);
        Task<List<AllDevicesListResponseDto>> GetAllUserDevicesAsync(TuyaAccount tuyaAccount);

        // Executes a request to turn on or off the specified switch on the specified device.
        Task<bool> ExecuteRequestAsync(Device device, string function, string action);

        // Sets the icon for the switch based on its last state.
        void SetIcon(bool lastState);
    }
}
