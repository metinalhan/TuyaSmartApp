using MahApps.Metro.Controls;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Consts;
using TuyaApp.Application.Enums;
using TuyaApp.Application.Extensions;
using TuyaApp.Domain.Entities;

namespace WpfApp.View.DeviceViews
{
    /// <summary>
    /// Interaction logic for LampDashboardView.xaml
    /// </summary>
    public partial class LampDashboardView : UserControl
    {
        private readonly Device _device;
        private readonly string _port;
        ITuyaCloudService _tuyaCloudService;    

        public LampDashboardView(Device device, ITuyaCloudService tuyaCloudService)
        {
            InitializeComponent();

            progress.Visibility = Visibility.Visible;


            _device = device;
            _port = LampSmartDeviceConst.Default;
            _tuyaCloudService = tuyaCloudService;

           lblName.Content = device.DeviceName;

            LoadLastStatus();
        }

        //This method load last states of functions of Lamps devices
        private async Task LoadLastStatus()
        {
            var result = await _tuyaCloudService.CheckLastStateAsync(_device, _device.DefaultFunction);

           // int port = _device.DefaultFunction.GetPort();
            bool lastState = (bool)result[Lamp.Status.ToInt32()].Value;
            int bright = result[Lamp.Brightness.ToInt32()].Value.ToInt32();
            int contrast = result[Lamp.Contrast.ToInt32()].Value.ToInt32();

            slBright.Value = bright / 10;
            slColorValue.Value = contrast / 10;

            if (lastState)
                tgSwitch.IsOn = true;
            else
                tgSwitch.IsOn = false;

            progress.Visibility = Visibility.Collapsed;
        }

        //This method invoke when clicked on and off switch of Lamp device
        private async void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    await _tuyaCloudService.ExecuteRequestAsync(_device, LampSmartDeviceConst.Switch + _port, "true");
                }
                else
                {
                    await _tuyaCloudService.ExecuteRequestAsync(_device, LampSmartDeviceConst.Switch + _port, "false");
                }
            }
        }

        //This method for control brightness of Lamp device
        private async void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            int val = (int)slBright.Value * 10;

           await _tuyaCloudService.ExecuteRequestAsync(_device, LampSmartDeviceConst.Brightness, val.ToString());
        }

        //This method for control contrast of Lamp device
        private async void slColorValue_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            int val = (int)slColorValue.Value * 10;

            await _tuyaCloudService.ExecuteRequestAsync(_device, LampSmartDeviceConst.Contrast, val.ToString());
        }
       
    }
}
