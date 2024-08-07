using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Enums;
using TuyaApp.Application.Extensions;
using TuyaApp.Domain.Entities;

namespace WpfApp.View.DeviceViews
{
    /// <summary>
    /// Interaction logic for ContactSensorDashboardView.xaml
    /// </summary>
    public partial class ContactSensorDashboardView : UserControl
    {
        private readonly Device _device;
        private readonly ITuyaCloudService _tuyaCloudService;

        public ContactSensorDashboardView(Device device, ITuyaCloudService tuyaCloudService)
        {
            InitializeComponent();

            progress.Visibility = Visibility.Visible;
            _device = device;
            _tuyaCloudService = tuyaCloudService;

            lblName.Content = device.DeviceName;

            LoadLastStatus().GetAwaiter();
        }

        private async Task LoadLastStatus()
        {
            var result = await _tuyaCloudService.CheckLastStateAsync(
                _device,
                _device.DefaultFunction
            );           

            bool lastState = (bool)result[ContactSensor.Status.ToInt32()].Value;
            int batt = result[ContactSensor.Battery.ToInt32()].Value.ToInt32();

            var devicedetails = await _tuyaCloudService.GetDeviceDetails(_device);
            var lastActivity = devicedetails.UpdateTime.FromUnixTimestamp();

            lblLastTime.Content = lastActivity.ToString("dd/MM/yyyy HH:mm"); ;

            //lblSensor.Content = lastState;
            if (lastState)
                tgSwitch.IsOn = true;
            else
                tgSwitch.IsOn = false;

            tbBatt.Content = "% " + batt;

            progress.Visibility = Visibility.Collapsed;
        }
    }
}
