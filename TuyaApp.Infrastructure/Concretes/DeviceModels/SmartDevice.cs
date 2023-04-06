using System;
using System.Threading.Tasks;
using TuyaApp.Application.Abstractions.DeviceModels;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Consts;
using TuyaApp.Application.Extensions;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Infrastructure.Concretes.DeviceModels
{
    // This is a class that represents a generic smart device and implements the ISmartDevice interface.
    public class SmartDevice:ISmartDevice
    {
        // These are private fields of the SmartDevice class.
        private readonly Device _device;
        protected readonly string _port;
        private readonly ITuyaCloudService _tuyaCloudService;

        // This is the constructor of the SmartDevice class that takes a Device, ITuyaCloudService, and an optional function name as parameters.
        public SmartDevice(Device device, ITuyaCloudService tuyaCloudService, string port=null)
        {
            _device = device;
            _port = port;
            _tuyaCloudService = tuyaCloudService;
        }

        // This is a virtual method that turns the device on or off.
        public async virtual Task Switch()
        {
            var result = await _tuyaCloudService.CheckLastStateAsync(_device, _port);

            int port = _port.GetPort();
            bool lastState = (bool)result[port].Value;

            await _tuyaCloudService.ExecuteRequestAsync(_device, SwitchSmartDeviceConst.Switch +_port, (!lastState).ToString());
        }

        // This is a virtual method that turns the device off.
        public async virtual Task TurnOff()
        {
            await _tuyaCloudService.ExecuteRequestAsync(_device, SwitchSmartDeviceConst.Switch+_port, "false");
        }

        // This is a virtual method that turns the device on.
        public async virtual Task TurnOn()
        {
            await _tuyaCloudService.ExecuteRequestAsync(_device, SwitchSmartDeviceConst.Switch+_port, "true");
        }
    }
}
