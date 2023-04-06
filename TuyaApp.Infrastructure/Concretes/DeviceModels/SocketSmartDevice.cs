using TuyaApp.Application.Abstractions.DeviceModels;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Infrastructure.Concretes.DeviceModels
{
    public class SocketSmartDevice : SmartDevice, ISocketSmartDevice
    {       
        public SocketSmartDevice(Device device, string function, ITuyaCloudService tuyaCloudService) : base(device, tuyaCloudService, function)
        {
        }
    }
}
