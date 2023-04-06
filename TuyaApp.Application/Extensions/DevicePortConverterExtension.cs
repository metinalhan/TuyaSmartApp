using System;

namespace TuyaApp.Application.Extensions
{
    public static class DevicePortConverterExtension
    {
        //This extension method for convert smart devices port
        public static int GetPort(this string devicePort)
        {
            if (devicePort.Equals("led"))
                return 0;
            else
                return Convert.ToInt16(devicePort) - 1;
        }
    }
}
