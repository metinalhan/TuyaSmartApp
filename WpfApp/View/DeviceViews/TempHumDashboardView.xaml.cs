using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Enums;
using TuyaApp.Application.Extensions;
using TuyaApp.Domain.Entities;

namespace WpfApp.View.DeviceViews
{
    public partial class TempHumDashboardView : UserControl
    {
        private readonly Device _device;
        private readonly ITuyaCloudService _tuyaCloudService;

        public TempHumDashboardView(Device device, ITuyaCloudService tuyaCloudService)
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

            float temp = result[THSensor.Temperature.ToInt32()].Value.ToInt32();
            int hum = result[THSensor.Humidity.ToInt32()].Value.ToInt32();
            int batt = result[THSensor.Battery.ToInt32()].Value.ToInt32();

            tbTemp.Content = (temp / 10) + " ℃";
            tbHum.Content = "% " + hum;
            tbBatt.Content = "% " + batt;

            progress.Visibility = Visibility.Collapsed;
        }
    }
}
