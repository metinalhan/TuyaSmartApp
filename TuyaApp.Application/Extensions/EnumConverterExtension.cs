using System;
using TuyaApp.Application.Enums;

namespace TuyaApp.Application.Extensions
{
    public static class EnumConverterExtension
    {
        // Converts an integer value to the corresponding DeviceType enum string representation
        public static string ConvertToEnum(this int value)
        {
            return Enum.GetName(typeof(DeviceType), value);
        }
    }
}
