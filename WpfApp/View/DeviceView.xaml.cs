using FluentValidation;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Dtos;
using TuyaApp.Application.Dtos.DeviceDtos;
using TuyaApp.Application.Enums;
using TuyaApp.Application.Extensions;
using TuyaApp.Application.Helpers.DeviceHelpers;
using TuyaApp.Domain.Entities;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for DeviceView.xaml
    /// </summary>
    public partial class DeviceView : UserControl
    {
        private readonly IDeviceService _deviceService;
        private readonly ITuyaAccountService _accountService;
        private readonly IValidator<CreateDeviceDTO> _validator;
        private BindingList<string> errors;
        private readonly ITuyaCloudService _tuyaCloudService;

        public DeviceView(
            IDeviceService deviceService,
            ITuyaAccountService accountService,
            IValidator<CreateDeviceDTO> validator,
            ITuyaCloudService tuyaCloudService)
        {
            InitializeComponent();

            _deviceService = deviceService;
            _accountService = accountService;
            _validator = validator;
            _tuyaCloudService = tuyaCloudService;

            errors = new();
            errorList.ItemsSource = errors;

            SetAccountList().GetAwaiter();
            SetDeviceTypeList();
        }

        // Set the device type list for the combo box to all values of the DeviceType enum
        private void SetDeviceTypeList()
        {
            cbDeviceType.ItemsSource = Enum.GetValues(typeof(DeviceType)).Cast<DeviceType>();
        }

        // Get all Tuya accounts asynchronously and set the first one that is marked as default as the selected item in the combo box
        private async Task SetAccountList()
        {
            var list = await _accountService.GetAllTuyaAccountsAsync();
            var dfault = list.FirstOrDefault(x => x.IsDefault == true);
            cbAccount.ItemsSource = list;
            cbAccount.SelectedItem = dfault;
        }

        // Delete the selected device, if the user confirms the deletion
        private async void DeleteDevice_Click(object sender, RoutedEventArgs e)
        {
            var device = lvList.SelectedItem as DeviceDTO;

            if (device is null)
                return;

            MessageBoxResult result = MessageBox.Show(
                "Seçili Cihaz Silinecek, Emin misin ?",
                "Cihaz Sil Onay",
                MessageBoxButton.YesNo
            );

            if (result == MessageBoxResult.Yes)
            {
                await _deviceService.DeleteDeviceAsync(device.Id);
            }

            await GetDevicesList();
        }

        // Add a new device with the specified account, device name, device type, device ID, and number of switches
        private async void Save_ButtonClick(object sender, RoutedEventArgs e)
        {
            var account = cbAccount.SelectedItem as TuyaAccount;
            var deviceType = cbDeviceType.SelectedItem;

            string numberOfSwitch = tbNumberOfSwitch.Text;

            var newDevice = new CreateDeviceDTO
            {
                DeviceName = tbDeviceName.Text,
                DeviceTuyaId = tbDeviceId.Text,
                NumberOfSwitch = numberOfSwitch.Equals("") ? -1 : numberOfSwitch.ToInt32(),
                DeviceType = deviceType == null ? -1 : deviceType.ToInt32(),
                TuyaAccount = account
            };

            var results = await _validator.ValidateAsync(newDevice);

            errors.Clear();
            foreach (var result in results.Errors)
            {
                errors.Add("*" + result.ErrorMessage);
            }

            if (results.IsValid == false)
                return;

            await _deviceService.AddNewDeviceAsync(newDevice);

            tbDeviceName.Clear();
            tbDeviceId.Clear();
            tbNumberOfSwitch.Text = "0";
            cbDeviceType.SelectedIndex = -1;

            await GetDevicesList();
        }

        // Hide the grid
        private void HidGrid_Click(object sender, RoutedEventArgs e)
        {
            gridGizle.Visibility = Visibility.Collapsed;
        }

        // Show the grid
        private void ShowGrid_Click(object sender, RoutedEventArgs e)
        {
            gridGizle.Visibility = Visibility.Visible;
        }

        // Get the list of devices for the newly selected account
        private async void cbAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await GetDevicesList();
        }

        // Get the list of devices for the currently selected account
        private async Task GetDevicesList()
        {
            var account = cbAccount.SelectedItem as TuyaAccount;

            if (account is null)
                return;

            lvList.ItemsSource = await _deviceService.GetDevicesByAccountAsync(account.Id);
        }

        // Allow only numeric input and a few specific non-numeric keys
        private void SadeceRakamKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = JustNumericInputHelper.CanInput(e);
        }

        private async void GetDevicesFromTuyaAccount_Click(object sender, RoutedEventArgs e)
        {
            var account = cbAccount.SelectedItem as TuyaAccount;

            var dialog = new TuyaDevicesView(account);

            dialog.ShowDialog();

            await GetDevicesList();
        }
    }
}
