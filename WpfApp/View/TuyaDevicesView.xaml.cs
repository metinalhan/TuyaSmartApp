using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Consts;
using TuyaApp.Application.Dtos.DeviceDtos;
using TuyaApp.Application.Extensions;
using TuyaApp.Domain.Entities;
using TuyaApp.Application.Enums;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for TuyaDevicesView.xaml
    /// </summary>
    public partial class TuyaDevicesView : Window
    {
        public ObservableCollection<CreateDeviceDTO> _devices { get; set; }
        private readonly ITuyaCloudService _tuyaCloudService;
        private readonly IDeviceService _deviceService;
        private readonly TuyaAccount _tuyaAccount;

        public TuyaDevicesView(TuyaAccount tuyaAccount)
        {
            InitializeComponent();

            _tuyaAccount = tuyaAccount;
            _tuyaCloudService = App.AppHost.Services.GetRequiredService<ITuyaCloudService>();
            _deviceService = App.AppHost.Services.GetRequiredService<IDeviceService>();

            SetDeviceList();
        }
        private async void SetDeviceList()
        {           
            var registeredDevices = await _deviceService.GetDevicesByAccountAsync(_tuyaAccount.Id);

            var devices = await _tuyaCloudService.GetAllUserDevicesAsync(_tuyaAccount);
           
            _devices = new ObservableCollection<CreateDeviceDTO>();

            foreach (var device in devices)
            {
                bool flag = registeredDevices.Find(f=>f.DeviceTuyaId.Equals(device.DeviceId)) == null;
                if(!flag)
                    continue;

                var deviceType = CategoryCodes.GetCategoryByCode(device.Category);
                
                var numberOfSwitch = device.DeviceSpecs?.FindAll(f => 
                Regex.IsMatch(f.Code, @"^switch_\d+$") || 
                Regex.IsMatch(f.Code, @"^switch\d+_value$") ||
                Regex.IsMatch(f.Code, @"^switch_led$")).Count;

                if (device.DeviceSpecs == null || deviceType == DeviceType.WirelessSwitch)
                    continue;

                var newDevice = new CreateDeviceDTO
                {
                    DeviceName = device.DeviceName,
                    DeviceTuyaId = device.DeviceId,
                    NumberOfSwitch = numberOfSwitch.Equals(0) ? -1 : numberOfSwitch.ToInt32(),
                    DeviceType = deviceType == null ? -1 : deviceType.ToInt32(),
                    DeviceTypeName = deviceType == null ? "undefined" : deviceType.GetDescription(),
                    TuyaAccount = _tuyaAccount
                };

                _devices.Add(newDevice);
            }

            lvList.ItemsSource = _devices;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Seçili Cihazlar Eklenecek, Emin misin ?",
                "Cihaz Ekle Onay",
                MessageBoxButton.YesNo
            );

            if (result == MessageBoxResult.Yes)
            {
                var devices = _devices.Where(item => item.IsSelected).ToList();

                foreach (var device in devices)
                {
                    await _deviceService.AddNewDeviceAsync(device);
                }
            }

            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
