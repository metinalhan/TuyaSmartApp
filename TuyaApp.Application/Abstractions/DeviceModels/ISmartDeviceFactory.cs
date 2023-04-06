using TuyaApp.Application.Enums;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Application.Abstractions.DeviceModels
{
    public interface ISmartDeviceFactory
    {
        ISmartDevice CreateSmartDevice(DeviceType deviceType, Device device ,string function);
    }
}
