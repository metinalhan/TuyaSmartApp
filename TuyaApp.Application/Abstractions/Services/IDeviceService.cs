using System.Collections.Generic;
using System.Threading.Tasks;
using TuyaApp.Application.Dtos;
using TuyaApp.Application.Dtos.DeviceDtos;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Application.Abstractions.Services
{
    // This is an interface definition for a service that provides functionality related to managing devices.
    // The interface includes methods for adding new devices, retrieving devices by account, deleting devices, and more.

    public interface IDeviceService
    {
        // Adds a new device with the given parameters to the repository, using the specified Tuya account.
        Task AddNewDeviceAsync(CreateDeviceDTO device);

        // Retrieves a list of devices associated with the specified account.
        Task<List<DeviceDTO>> GetDevicesByAccountAsync(int account_id);

        // Deletes the device with the specified id from the repository.
        Task<bool> DeleteDeviceAsync(int deviceId);

        // Retrieves the device with the specified id from the repository.
        Task<Device> GetDeviceByIdAsync(int deviceId);

        // Retrieves a list of all devices from the repository.
        Task<List<Device>> GetAllDevicesAsync();

        // Marks the device with the specified id as a favourite.
        Task<bool> MakeFavouriteDeviceAsync(int deviceId);

        // Assigns default functionality to the specified device (e.g. toggle switch).
        Task<bool> AssignDefaultFunctionToDeviceAsync(Device device);

        // Retrieves the device with default functionality from the repository.
        Task<Device> GetDefaultDeviceAsync();
    }
}
