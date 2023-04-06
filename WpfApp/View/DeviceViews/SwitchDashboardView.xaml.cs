using MahApps.Metro.Controls;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Consts;
using TuyaApp.Application.Extensions;
using TuyaApp.Domain.Entities;

namespace WpfApp.View.DeviceViews
{
    /// <summary>
    /// Interaction logic for SwitchDashboardView.xaml
    /// </summary>
    public partial class SwitchDashboardView : UserControl
    {
        private readonly Device _device;
        private readonly ITuyaCloudService _tuyaCloudService;
        public SwitchDashboardView(Device device, ITuyaCloudService tuyaCloudService)
        {
            InitializeComponent();

            progress.Visibility = Visibility.Visible;

            _device = device;
            _tuyaCloudService = tuyaCloudService;

            lblName.Content = _device.DeviceName;

            LoadSwitches().GetAwaiter();
        }

        //This method create Switch functions programatically according to given number of switch
        private async Task LoadSwitches()
        {
            int switchAmount = _device.NumberOfSwitch;

            ToggleSwitch switches;

            for (int i = 1; i <= switchAmount; i++)
            {
                switches = new ToggleSwitch();
                switches.Tag = i;
                switches.Header = "Switch " + i;
                switches.OffContent = "Kapalı";
                switches.OnContent = "Açık";
                switches.Margin = new Thickness(2);
                switches.Toggled += ToggleSwitch_Toggled;

                switchPanel.Children.Add(switches);
            }

            foreach (var item in switchPanel.Children)
            {
                var itemType = item.GetType();
                var name = itemType.UnderlyingSystemType.Name;

                if (name.Equals(nameof(ToggleSwitch)))
                {
                    var tSwitch = item as ToggleSwitch;
                    //bool lastStatus = await _tuyaCloudService.CheckLastStateAsync(_device, tSwitch.Tag.ToString()); 
                    
                    var result = await _tuyaCloudService.CheckLastStateAsync(_device, tSwitch.Tag.ToString());
                    int port = tSwitch.Tag.ToString().GetPort();
                    bool lastStatus = (bool)result[port].Value;

                    if (lastStatus)
                        tSwitch.IsOn = true;
                    else
                        tSwitch.IsOn = false;
                }

            }

            progress.Visibility = Visibility.Collapsed;

        }

        //This method for switch on and off functions of Switch device
        private async void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var toggleSwitch = sender as ToggleSwitch;
            var tag = toggleSwitch.Tag;

            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    await _tuyaCloudService.ExecuteRequestAsync(_device, SwitchSmartDeviceConst.Switch + tag.ToString(), "true");
                }
                else
                {
                    await _tuyaCloudService.ExecuteRequestAsync(_device, SwitchSmartDeviceConst.Switch + tag.ToString(), "false");
                }
            }
        }
    }
}
