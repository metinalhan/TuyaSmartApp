using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TuyaApp.Application.Abstractions.DashboardView;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Dtos;
using TuyaApp.Domain.Entities;

namespace WpfApp.View
{

    public partial class DashboardView : UserControl
    {
        List<DashboardSaveDTO> listofControls;
        IDashboardService _dashboardService;
        IDeviceService _deviceService;
        ITuyaAccountService _tuyaAccountService;
        IDashboardViewFactory _dashboardViewFactory;
        IDashboardViewService _dashboardViewService;


        private object movingObject;
        private double firstXPos, firstYPos;

        public DashboardView(IDashboardService dashboardService, IDeviceService deviceService, ITuyaAccountService tuyaAccountService,  IDashboardViewFactory dashboardViewFactory, IDashboardViewService dashboardViewService)
        {
            InitializeComponent();

            _dashboardService = dashboardService;
            _deviceService = deviceService;
            _tuyaAccountService = tuyaAccountService;
            _dashboardViewFactory = dashboardViewFactory;
            _dashboardViewService = dashboardViewService;

            _dashboardViewService.MoveMouseEvent += MoveMouse;
            _dashboardViewService.PreviewDownEvent += PreviewDown;
            _dashboardViewService.PreviewUpEvent += PreviewUp;
            _dashboardViewService.RemoveFromDashboardEvent += RemoveFromDashboard_Click;

            listofControls = new();

            SetDeviceTypeList().GetAwaiter();
            LoadAllDevices().GetAwaiter(); ;
        }

        //Set all devices into the combobox that define default account
        private async Task SetDeviceTypeList()
        {
            var account = await _tuyaAccountService.GetDefaultAccountAsync();

            if(account is not null)
            cbDevice.ItemsSource = await _deviceService.GetDevicesByAccountAsync(account.Id);
        }

        //Load all dashboard devices and send it AddtoDash for create views dynamically
        private async Task LoadAllDevices()
        {
            var list = await _dashboardService.GetAllDashboardDevicesAsync();

            foreach (var item in list)
            {
                var result = _dashboardViewFactory.CreateSmartDeviceView(item.Device);

               await AddtoDash(item.Device, result, item.PositionLeft, item.PositionTop);
            }
        }       

        //This method using dashboard service to create dashboard view for devices and added in array
        private async Task AddtoDash(Device device, UserControl view, double left = 20, double top = 20)
        {
            var stackPanel = _dashboardViewService.CreateDeviceView(device.DeviceTuyaId, view);

            canvas.Children.Add(stackPanel);

            Canvas.SetLeft(stackPanel, left);
            Canvas.SetTop(stackPanel, top);

            SavetoArray(stackPanel.Tag.ToString(), device, left, top);
        }

        //This method holding informations about dashboard devices
        private void SavetoArray(string deviceTuyaId, Device device, double left, double top)
        {
            var save = new DashboardSaveDTO
            {
                DeviceTuyaId = deviceTuyaId,
                device = device,
                PositionLeft = left,
                PositionTop = top
            };

            listofControls.Add(save);
        }

        //This method remove device from dashboard that choisen by user
        private void RemoveFromDashboard_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Cihaz Dashboard'tan Kaldırılacak, Onaylıyor musunuz?", "Onay !", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var device = (sender as Button).Tag;

                var children = canvas.FindChildren<StackPanel>().Where(w => w.Tag != null);
                var child = children.FirstOrDefault(x => x.Tag.Equals(device));

                canvas.Children.Remove(child);

                var item = listofControls.FirstOrDefault(x => x.DeviceTuyaId.Equals(device));

                listofControls.Remove(item);
            }
        }             

        //This method save all dashboard devices with position settings
        private async void Kaydet_Button_Click(object sender, RoutedEventArgs e)
        {
            var result = await _dashboardService.SaveDashboardAsync(listofControls);

            if (result)
            {
                MessageBox.Show("Dashboard Kaydedildi", "Başarılı", MessageBoxButton.OK);
            }
        }

        //This method show and hidden specific Groupbox
        private void ShowHiddenAddDeviceToDash(object sender, RoutedEventArgs e)
        {

            if (addDevice.Visibility == Visibility.Visible)
            {
                addDevice.Visibility = Visibility.Collapsed;
                btnShowHid.Content = "Cihaz Ekle";
            }
            else
            {
                addDevice.Visibility = Visibility.Visible;
                btnShowHid.Content = "Gizle";
            }
        }

        //This method Add Device that choisen to Dashboard 
        private async void AddtoDashboard_Click(object sender, RoutedEventArgs e)
        {
            var selected = cbDevice.SelectedItem as DeviceDTO;
            if (selected == null) return;


            var duplicate_check = listofControls.Any(x => x.DeviceTuyaId.Equals(selected.DeviceTuyaId));

            if (duplicate_check)
            {
                MessageBox.Show("Bu Cihaz Dashboardda Mevcut", "Uyarı !");
                return;
            }

            var device = await _deviceService.GetDeviceByIdAsync(selected.Id);

            var result = _dashboardViewFactory.CreateSmartDeviceView(device);

           await AddtoDash(device, result);
        }

        //This method for getting positon of device that clicked orange border area
        private void PreviewDown(object sender, MouseButtonEventArgs e)
        {
            var thumb = e.Source as Border;
            if (thumb == null)
                return;

            firstXPos = e.GetPosition(thumb).X;
            firstYPos = e.GetPosition(thumb).Y;

            movingObject = sender;
        }

        //This method for saving last information about position of device after release mouse button
        private void PreviewUp(object sender, MouseButtonEventArgs e)
        {
            var thumb = e.Source as Border;
            if (thumb == null)
                return;

            var spanel = ((StackPanel)movingObject);

            var item = listofControls.FirstOrDefault(x => x.DeviceTuyaId.Equals(spanel.Tag.ToString()));

            double left = Canvas.GetLeft(spanel);
            double top = Canvas.GetTop(spanel);

            item.PositionLeft = left;
            item.PositionTop = top;

            movingObject = null;
        }

        //This method for change the device position on Dashboard
        private void MoveMouse(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender == movingObject)
            {
                var thumb = e.Source as Border;
                if (thumb == null)
                    return;

                var mousePos = Mouse.GetPosition(thumb);

                if (mousePos.Y > 25)
                    return;

                double newLeft = e.GetPosition(canvas).X - firstXPos - canvas.Margin.Left;

                ((StackPanel)movingObject).SetValue(Canvas.LeftProperty, newLeft);

                double newTop = e.GetPosition(canvas).Y - firstYPos - canvas.Margin.Top;

                ((StackPanel)movingObject).SetValue(Canvas.TopProperty, newTop);
            }
        }
    }
}
