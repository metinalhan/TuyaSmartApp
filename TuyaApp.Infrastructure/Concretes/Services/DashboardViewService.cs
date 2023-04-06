using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TuyaApp.Application.Abstractions.Services;

namespace TuyaApp.Infrastructure.Concretes.Services
{
    public class DashboardViewService :UserControl, IDashboardViewService
    {
        public event MouseButtonEventHandler PreviewDownEvent;
        public event MouseButtonEventHandler PreviewUpEvent;
        public event MouseEventHandler MoveMouseEvent;
        public event RoutedEventHandler RemoveFromDashboardEvent;

        //This method create device view programatically for dashboard
        public StackPanel CreateDeviceView(string deviceTuyaId, UserControl view)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Tag = deviceTuyaId;
            stackPanel.PreviewMouseLeftButtonDown += PreviewDown;
            stackPanel.PreviewMouseLeftButtonUp += PreviewUp;
            stackPanel.MouseMove += MoveMouse;

            Grid grid = new Grid();
            var columnDefinition = new ColumnDefinition();
            columnDefinition.Width = GridLength.Auto;
            grid.ColumnDefinitions.Add(columnDefinition);
            var columnDefinition2 = new ColumnDefinition();
            columnDefinition2.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(columnDefinition2);

            Button btn5 = new Button();
            btn5.Foreground = Brushes.White;
            btn5.BorderThickness =new Thickness(0);
            btn5.SnapsToDevicePixels = true;
            btn5.Tag = deviceTuyaId;
            btn5.Width = 25;
            btn5.Height = 25;
            btn5.Click += RemoveFromDashboard;

            Image exitImage = new Image();
            exitImage.Stretch = Stretch.Fill;
            exitImage.Source = new BitmapImage(new Uri(@"\icons\delete.png", UriKind.Relative));

            btn5.Content = exitImage;
            Grid.SetColumn(btn5, 0);

            Border border = new Border();
            border.BorderBrush = Brushes.Orange;
            border.BorderThickness = new Thickness(0, 25, 0, 0);
            border.Background = Brushes.Transparent;
            Grid.SetColumn(border, 1);

            grid.Children.Add(btn5);
            grid.Children.Add(border);

            stackPanel.Children.Add(grid);

            stackPanel.Children.Add(view);

            return stackPanel;
        }

        //These all methods are routes event to main dashboard view class
        private void RemoveFromDashboard(object sender, RoutedEventArgs e)
        {
            RemoveFromDashboardEvent.Invoke(sender, e);
        }

        private void MoveMouse(object sender, MouseEventArgs e)
        {
           MoveMouseEvent.Invoke(sender, e);
        }

        private void PreviewUp(object sender, MouseButtonEventArgs e)
        {
            PreviewUpEvent?.Invoke(sender, e);
        }

        private void PreviewDown(object sender, MouseButtonEventArgs e)
        {
            PreviewDownEvent?.Invoke(sender, e);
        }
    }
}
