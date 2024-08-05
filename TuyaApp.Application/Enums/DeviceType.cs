using System.ComponentModel;

namespace TuyaApp.Application.Enums
{
    public enum DeviceType
    {
        [Description("Light")]
        Lamp,

        [Description("Socket")]
        Socket,

        [Description("Switch")]
        Switch,

        [Description("Temperature Humidity Sensor")]
        THSensor,

        [Description("Contact Sensor")]
        ContactSensor
    }
}
