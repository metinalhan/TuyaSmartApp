using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TuyaApp.Application.Abstractions.MenuLogic;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Consts;
using TuyaApp.Application.Dtos;
using TuyaApp.Application.Enums;
using TuyaApp.Application.ViewModels;
using TuyaApp.Domain.Entities;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        private readonly IMainViewModel _mainViewModel;
        private readonly IMenuLogicService _logicService;
        private readonly IDeviceService _deviceService;
        private readonly ITuyaAccountService _accountService;
        private readonly IMenuService _menuService;

        public MenuView(IMainViewModel mainViewModel, IMenuLogicService logicService, IDeviceService deviceService, ITuyaAccountService accountService, IMenuService menuService)
        {
            InitializeComponent();

            _mainViewModel = mainViewModel;
            _logicService = logicService;
            _deviceService = deviceService;
            _accountService = accountService;
            _menuService = menuService;

            _mainViewModel.Menus.Clear();
            DataContext = _mainViewModel;

             SetAccountList().GetAwaiter();
        }

        private async Task SetAccountList()
        {
            // Call the GetAllTuyaAccountsAsync method of the account service to retrieve all Tuya accounts.
            var list = await _accountService.GetAllTuyaAccountsAsync();
            var dfault = list.FirstOrDefault(x => x.IsDefault == true);
            cbAccount.ItemsSource = list;
            cbAccount.SelectedItem = dfault;
        }

        private async void cbAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected Tuya account from the cbAccount ComboBox.
            var selected = cbAccount.SelectedItem as TuyaAccount;

            if (selected is null)
                return;

           await GetMenuProfilList();

            // Call the GetDevicesByAccountAsync method of the device service to retrieve all devices associated with the selected Tuya account.
            // Set the ItemsSource property of the cbDevice ComboBox to the retrieved devices.
            cbDevice.ItemsSource = await _deviceService.GetDevicesByAccountAsync(selected.Id);
        }

        // This method is called when the selection in the list of profiles is changed
        private async Task GetMenuProfilList()
        {
            var account = cbAccount.SelectedItem as TuyaAccount;

            if (account is null)
                return;

            var list = await _menuService.GetAllMenuByAccountAsync(account.Id);
            var dfault = list.FirstOrDefault(x => x.IsDefault == true);
            cbProfile.ItemsSource = list;
            cbProfile.SelectedItem = dfault;
        }

        private void AddMenu_Click(object sender, RoutedEventArgs e)
        {
            string menuAdi = tbMenuAdi.Text;
            var secim = tvList.SelectedValue;

            // Get the name of the menu to add from the tbMenuAdi TextBox.

            _logicService.MenuAdd(menuAdi, secim);

            // Clear the selection in the tvList TreeView.
            ClearSelection();
        }

        private void DeleteMenu_Click(object sender, RoutedEventArgs e)
        {
            var secim = tvList.SelectedValue;

            if (secim != null)
            {
                // Call the MenuRemove method of the logic service to remove the selected menu.
                _logicService.MenuRemove(secim);

                ClearSelection();
            }
        }

        private void btUp_Click(object sender, RoutedEventArgs e)
        {
            var secim = tvList.SelectedValue;
            if (secim != null && secim is MenuViewModel)
            {
                // Call the MenuUp method of the logic service to move the selected menu up in the list.
                _logicService.MenuUp(secim);
            }
        }

        private void btDown_Click(object sender, RoutedEventArgs e)
        {
            var secim = tvList.SelectedValue;
            if (secim != null && secim is MenuViewModel)
            {
                // Call the MenuDown method of the logic service to move the selected menu down in the list.
                _logicService.MenuDown(secim);
            }
        }

        private void ClearSelection()
        {
            // Get the container for the selected item in the tvList TreeView.
            var s = tvList.ItemContainerGenerator.ContainerFromItem(tvList.SelectedItem) as TreeViewItem;

            if (s != null)
                s.IsSelected = false;
        }
       

        // This method is called when the "Assign Function to Device" button is clicked.
        // It assigns a function to the selected device and button switch.
        private void AssignFunctionToDevice_Click(object sender, RoutedEventArgs e)
        {
            var selected = tvList.SelectedValue;

            if (selected is null) return;

            var device = cbDevice.SelectedItem as DeviceDTO;

            if (device is null) return;

            var button_switch = cbDeviceSwitch.SelectedItem;

            if(button_switch is null)
            {
                if (device.DeviceType == DeviceType.Lamp)
                    button_switch = LampSmartDeviceConst.Default;
                else
                {
                    MessageBox.Show("Switch seçimi yapılmadı !", "Uyarı", MessageBoxButton.OK);
                    return;
                }
            }


            _logicService.AssignFunctionToDevice(selected, button_switch ,device);
        }

        // This method is called when the selected device in the combo box is changed.
        // It updates the list of button switches based on the selected device.
        private void DeviceSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = cbDevice.SelectedItem as DeviceDTO;

            if (selected is null)
                return;

            cbDeviceSwitch.ItemsSource = null;
            List<int> switch_list = new();

            for (int i = 1; i <= selected.NumberOfSwitch; i++)
            {
                switch_list.Add(i);
            }

            cbDeviceSwitch.ItemsSource = switch_list;
        }

        // This method is called when the Menu Content Save button is clicked
        private async void MenuContentSave_Click(object sender, RoutedEventArgs e)
        {

            var profile = cbProfile.SelectedItem as MenuProfile;

            if (profile is null) return;

            var menu = _logicService.MenuSave();

            await _menuService.UpdateMenuContentAsync(menu, profile.Id);
            _logicService.ApplyMenuProfile(profile);

            MessageBox.Show("Menü Kaydedildi !", "Menü Kaydet", MessageBoxButton.OK);
        }

        // This is an event handler for the "AssignDefaultFunction_Click" event
        private async void AssignDefaultFunction_Click(object sender, RoutedEventArgs e)
        {
            var selected = tvList.SelectedValue;

            if (selected is null) return;

          var result = await _logicService.AssignDefaultFunctionAsync(selected);

            if(result)
                MessageBox.Show("İşlem Başarıyla Uygulandı", "Başarılı", MessageBoxButton.OK);
        }

        // This method is called when a profile is selected from the combobox
        private void cbProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var profile = cbProfile.SelectedItem as MenuProfile;

            if (profile is null) return;

            _mainViewModel.Menus.Clear();

            if (profile.MenuSave is null)
                return;

            _logicService.ShowMenuContent(profile);
        }
    }
}
