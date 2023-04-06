using System.Threading.Tasks;
using TuyaApp.Application.Abstractions.DeviceModels;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Consts;
using TuyaApp.Application.Extensions;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Infrastructure.Concretes.DeviceModels
{
    // This is a class that represents a Lamp smart device and inherits from SmartDevice and ILampSmartDevice interfaces.
    public class LampSmartDevice : SmartDevice, ILampSmartDevice
    {
        // These are private fields of the LampSmartDevice class.
        private readonly Device _device;     
        private readonly string _port;
        private readonly ITuyaCloudService _tuyaCloudService;

        // This is the constructor of the LampSmartDevice class that takes a Device and ITuyaCloudService as parameters and calls the base constructor.
        public LampSmartDevice(Device device, ITuyaCloudService tuyaCloudService):base(device, tuyaCloudService)
        {
            _device = device;
            _port = LampSmartDeviceConst.Default;
            _tuyaCloudService = tuyaCloudService;           
        }

        // This is an override method that turns the device on or off and sets the icon accordingly.
        public override async Task Switch()
        {
            var result = await _tuyaCloudService.CheckLastStateAsync(_device, _port);

            int port = _port.GetPort();
            bool lastState = (bool)result[port].Value;

            await _tuyaCloudService.ExecuteRequestAsync(_device, LampSmartDeviceConst.Switch+_port, (!lastState).ToString());
            _tuyaCloudService.SetIcon(lastState);
        }

        // This is an override method that turns the device off.
        public async override Task TurnOff()
        {           
            await _tuyaCloudService.ExecuteRequestAsync(_device, LampSmartDeviceConst.Switch+_port, "false");
        }

        // This is an override method that turns the device on.
        public async override Task TurnOn()
        {           
            await _tuyaCloudService.ExecuteRequestAsync(_device, LampSmartDeviceConst.Switch+_port, "true");
        }
    }
}