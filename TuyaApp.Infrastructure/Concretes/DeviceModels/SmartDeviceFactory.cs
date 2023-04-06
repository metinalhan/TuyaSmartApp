using TuyaApp.Application.Abstractions.DeviceModels;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Enums;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Infrastructure.Concretes.DeviceModels
{
    // This class represents a factory that creates smart devices and implements the ISmartDeviceFactory interface.
    public class SmartDeviceFactory : ISmartDeviceFactory
    {
        private readonly ITuyaCloudService _tuyaCloudService;

        // This is the constructor of the SmartDeviceFactory class that takes an ITuyaCloudService parameter.
        public SmartDeviceFactory(ITuyaCloudService tuyaCloudService)
        {
            _tuyaCloudService = tuyaCloudService;
        }

        // This method creates a smart device based on the given device type and returns an ISmartDevice object.
        public ISmartDevice CreateSmartDevice(DeviceType deviceType, Device device ,string function)
        {
            return deviceType switch
            {
                DeviceType.Switch => new SwitchSmartDevice(device, function, _tuyaCloudService),
                DeviceType.Socket => new SocketSmartDevice(device, function, _tuyaCloudService),
                DeviceType.Lamp => new LampSmartDevice(device, _tuyaCloudService)
            };
        }
    }
}
