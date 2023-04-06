using System.Threading.Tasks;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Application.Abstractions.DeviceModels
{
    public interface ISmartDevice
    {
        // Declaration of the TurnOn function, which turns the device on.
        Task TurnOn();

        // Declaration of the TurnOff function, which turns the device off.
        Task TurnOff();

        // Declaration of the Switch function, which toggles the device's state between on and off.
        Task Switch();
    }
}
