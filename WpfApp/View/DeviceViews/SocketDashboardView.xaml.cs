﻿using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
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
    /// Interaction logic for SocketDashboardView.xaml
    /// </summary>
    public partial class SocketDashboardView : UserControl
    {
        private readonly Device _device;
        private readonly ITuyaCloudService _tuyaCloudService;

        public SocketDashboardView(Device device, ITuyaCloudService tuyaCloudService)
        {
            InitializeComponent();

            _device = device;

            _tuyaCloudService = tuyaCloudService;

            lblName.Content = _device.DeviceName;

            LoadSockets().GetAwaiter();
        }

        //This method create Socket functions programatically according to given number of switch
        private async Task LoadSockets()
        {
            int switchAmount = _device.NumberOfSwitch;

            ToggleSwitch switches;

            for (int i = 1; i <= switchAmount; i++)
            {
                switches = new ToggleSwitch();
                switches.Tag = i;
                switches.Header = "Socket " + i;
                switches.OffContent = "Kapalı";
                switches.OnContent = "Açık";
                switches.Margin = new Thickness(2);
                switches.Toggled += ToggleSwitch_Toggled;

                switchPanel.Children.Add(switches);
            }

            var result = await _tuyaCloudService.CheckLastStateAsync(_device, "");

            foreach (var item in switchPanel.Children)
            {
                var itemType = item.GetType();
                var name = itemType.UnderlyingSystemType.Name;

                if (name.Equals(nameof(ToggleSwitch)))
                {
                    var tSwitch = item as ToggleSwitch;
                    //var result = await _tuyaCloudService.CheckLastStateAsync(_device, tSwitch.Tag.ToString());

                    int port = tSwitch.Tag.ToString().GetPort();
                    bool lastStatus = (bool)result[port].Value;

                    if (lastStatus)
                        tSwitch.IsOn = true;
                    else
                        tSwitch.IsOn = false;
                }
            }

            var energy = result.Find(f => f.Code.Contains("current"));

            if (energy != null)
            {
                float current = energy.Value.ToFloat() / 1000;
                float power = result.Find(f => f.Code.Contains("power")).Value.ToFloat() / 10;
                float voltage = result.Find(f => f.Code.Contains("voltage")).Value.ToFloat() / 10;

                lblCurrent.Content = current + " A";
                lblVoltage.Content = voltage + " V";
                lblPower.Content = power + " W";
            }
            else
            {
                grpCur.Visibility = Visibility.Collapsed;
                grpPow.Visibility = Visibility.Collapsed;
                grpVolt.Visibility = Visibility.Collapsed;
            }


            progress.Visibility = Visibility.Collapsed;
        }

        //This method for switch on and off functions of Socket device
        private async void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var toggleSwitch = sender as ToggleSwitch;
            var tag = toggleSwitch.Tag;

            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    await _tuyaCloudService.ExecuteRequestAsync(_device, SocketSmartDeviceConst.Switch + tag.ToString(), "true");
                }
                else
                {
                    await _tuyaCloudService.ExecuteRequestAsync(_device, SocketSmartDeviceConst.Switch + tag.ToString(), "false");
                }
            }
        }
    }
}
