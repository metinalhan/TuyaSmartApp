using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TuyaApp.Application.Abstractions.Services
{
    //This is an interface definition for a all dasboard devices
    public interface IDashboardViewService
    {
        //Define event handlers
        event MouseButtonEventHandler PreviewDownEvent;
        event MouseButtonEventHandler PreviewUpEvent;
        event MouseEventHandler MoveMouseEvent;
        event RoutedEventHandler RemoveFromDashboardEvent;

        //Create device view method for dasboard
        StackPanel CreateDeviceView(string deviceTuyaId, UserControl view);
    }
}
