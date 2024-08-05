using System.Windows.Controls;
using TuyaApp.Application.Abstractions.DashboardView;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Enums;
using TuyaApp.Domain.Entities;
using WpfApp.View.DeviceViews;

namespace WpfApp.ViewModel
{
    public class DashboardViewFactory:UserControl, IDashboardViewFactory
    {
        private readonly ITuyaCloudService _tuyaCloudService;

        // This is the constructor of the DashboardViewFactory class that takes an ITuyaCloudService parameter.
        public DashboardViewFactory(ITuyaCloudService tuyaCloudService)
        {
            _tuyaCloudService = tuyaCloudService;
        }

        public UserControl CreateSmartDeviceView(Device device)
        {
            var deviceType = (DeviceType)device.DeviceType;

            return deviceType switch
            {
                DeviceType.Switch => new SwitchDashboardView(device, _tuyaCloudService),
                DeviceType.Socket => new SocketDashboardView(device,  _tuyaCloudService),
                DeviceType.Lamp => new LampDashboardView(device, _tuyaCloudService),
                DeviceType.THSensor => new TempHumDashboardView(device, _tuyaCloudService),
                DeviceType.ContactSensor => new ContactSensorDashboardView(device, _tuyaCloudService),
            };
        }
    }
}
