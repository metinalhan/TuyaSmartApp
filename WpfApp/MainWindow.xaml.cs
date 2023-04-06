using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using TuyaApp.Application.Abstractions.DeviceModels;
using TuyaApp.Application.Abstractions.MenuLogic;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Consts;
using TuyaApp.Application.Enums;
using TuyaApp.Application.Extensions;
using TuyaApp.Domain.Entities;
using WpfApp.View;
using Application = System.Windows.Application;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IMainViewModel _mainViewModel;
        private readonly IMenuLogicService _logicService;
        private readonly IDeviceService _deviceService;
        private readonly ITuyaAccountService _tuyaAccountService;
        private readonly IMenuService _menuService;
        private readonly NotifyIcon _notifyIcon;
        private readonly ITuyaCloudService _tuyaCloudService;
        private readonly ISmartDeviceFactory _smartDeviceFactory;

        private Func<MenuView> MenuFactory { get; }
        private Func<AccountView> AccountFactory { get; }
        private Func<DashboardView> DashboardFactory { get; }
        private Func<DeviceView> DeviceFactory { get; }
        private Func<ProfileView> ProfileFactory { get; }


        public MainWindow(IMainViewModel mainViewModel, IMenuLogicService logicService, IDeviceService deviceService,
            NotifyIcon notifyIcon, Func<MenuView> menuFactory, Func<AccountView> accountFactory,
            Func<DashboardView> dashboardFactory, Func<DeviceView> deviceFactory,
             ITuyaAccountService tuyaAccountService, IMenuService menuService, ITuyaCloudService tuyaCloudService, ISmartDeviceFactory smartDeviceFactory, Func<ProfileView> profileFactory)
        {
            InitializeComponent();

            MenuFactory = menuFactory;
            AccountFactory = accountFactory;
            DashboardFactory = dashboardFactory;
            ProfileFactory = profileFactory;
            DeviceFactory = deviceFactory;

            _mainViewModel = mainViewModel;
            _logicService = logicService;
            _deviceService = deviceService;
            _notifyIcon = notifyIcon;
            _tuyaAccountService = tuyaAccountService;
            _menuService = menuService;
            _tuyaCloudService = tuyaCloudService;
            _smartDeviceFactory = smartDeviceFactory;

            DataContext = _mainViewModel;

            SetDefaultMenu().GetAwaiter();
            SetIcon().GetAwaiter();

            _logicService.MenuItemClick += ToolStripMenuItem_Click;
        }

        //This method for Set Icon of Default Smart Device
        private async Task SetIcon()
        {
            _notifyIcon.Icon = new System.Drawing.Icon(@".\Resources\Bulb.ico");
            _notifyIcon.Visible = true;
            _notifyIcon.DoubleClick += IconDouble_Click;

            Device device = await _deviceService.GetDefaultDeviceAsync();
            if (device == null)
                return;

            var result = await _tuyaCloudService.CheckLastStateAsync(device, device.DefaultFunction);
            int port = device.DefaultFunction.GetPort();
            bool lastStatus = (bool)result[port].Value;
            _tuyaCloudService.SetIcon(!lastStatus);
        }

        //This method run function for choisen device when double click taskbar icon
        private async void IconDouble_Click(object? sender, EventArgs e)
        {
            Device device = await _deviceService.GetDefaultDeviceAsync();

            if (device == null) return;

            DeviceType deviceType = (DeviceType)device.DeviceType;

            var smartDevice = _smartDeviceFactory.CreateSmartDevice(deviceType, device, device.DefaultFunction);

           await smartDevice.Switch();
        }

        //This method for load menu for taskbar icon
        private async Task SetDefaultMenu()
        {
            _logicService.ApplyMenuProfile();

            var account = await _tuyaAccountService.GetDefaultAccountAsync();

            if (account == null) return;

            var profile = await _menuService.GetDefaultMenuProfileByAccount(account.Id);

            if (profile == null) return;

            if (profile.MenuSave is not null)
                _logicService.ApplyMenuProfile(profile);

        }

        //This method for runnes function that choisen by user
        private async void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            if (menuItem.Name.Equals(DefaultMenu.Dashboard))
            {
                this.Show();
                this.WindowState = WindowState.Normal;
                return;
            }


            int deviceId;
            string func;
            DeviceType deviceType;

            if (menuItem.Tag is SubMenuMod)
            {
                var selected = menuItem.Tag as SubMenuMod;
                deviceId = selected.DeviceId;
                func = selected.ButtonFunction;
            }
            else
            {
                var selected = menuItem.Tag as MenuMod;
                deviceId = selected.DeviceId;
                func = selected.ButtonFunction;
            }

            var device = await _deviceService.GetDeviceByIdAsync(deviceId);
            deviceType = (DeviceType)device.DeviceType;

            var smartDevice = _smartDeviceFactory.CreateSmartDevice(deviceType, device, func);

           await smartDevice.Switch();
        }

        //If the window minimized hide from bottom taskbar
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }

        //When clicked Mainwindow minimized instead of exit the application
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        //This method switches windows state between normal and maximized
        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        //This method for minimized window to taskbar
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        //This method switch menu to Menu View
        private void menuPage_Click(object sender, RoutedEventArgs e)
        {
            MenuView menuPage = this.MenuFactory.Invoke();
            PagesNavigation.Navigate(menuPage);
        }

        //This method switch menu to Account View
        private void accountPage_Click(object sender, RoutedEventArgs e)
        {
            AccountView accountPage = this.AccountFactory.Invoke();
            PagesNavigation.Navigate(accountPage);
        }

        //This method switch menu to Dashboard View
        private void dashboard_Click(object sender, RoutedEventArgs e)
        {
            DashboardView main = this.DashboardFactory.Invoke();
            PagesNavigation.Navigate(main);
        }

        //This method switch menu to Device View
        private void devicesPage_Click(object sender, RoutedEventArgs e)
        {
            DeviceView device = this.DeviceFactory.Invoke();
            PagesNavigation.Navigate(device);
        }

        //This method switch menu to Profile View
        private void profilesPage_Click(object sender, RoutedEventArgs e)
        {
            ProfileView profilePage = this.ProfileFactory.Invoke();
            PagesNavigation.Navigate(profilePage);
        }

        //This method exit the application when clicked
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //This method for change position of window screen
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        //This method for change window state when loading main view
        private void home_Loaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;

            DashboardView main = this.DashboardFactory.Invoke();
            PagesNavigation.Navigate(main);

            this.Hide();
        }

    }
}
