using System.Windows.Controls;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Application.Abstractions.DashboardView
{
    public interface IDashboardViewFactory
    {
        UserControl CreateSmartDeviceView(Device device);
    }
}
