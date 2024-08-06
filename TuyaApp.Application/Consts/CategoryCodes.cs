using System.Collections.Generic;
using System.Linq;
using TuyaApp.Application.Enums;

namespace TuyaApp.Application.Consts
{
    public static class CategoryCodes
    {
        private class Category
        {
            public string Code { get; }
            public string Description { get; }
            public DeviceType DeviceType { get; }

            public Category(string code, string description, DeviceType deviceType)
            {
                Code = code;
                Description = description;
                DeviceType = deviceType;
            }
        }

        private static readonly List<Category> AllCategories = new List<Category>
        {
            new Category("dj", "Light", DeviceType.Lamp),
            new Category("cz", "Socket", DeviceType.Socket),
            new Category("kg", "Switch", DeviceType.Switch),
            new Category("wsdcg", "Temperature Humidity Sensor", DeviceType.THSensor),
            new Category("mcs", "Contact Sensor", DeviceType.ContactSensor),
            new Category("pc", "Power Strip", DeviceType.Socket),
            new Category("tdq", "Breaker", DeviceType.Switch),
            new Category("wxkg", "Wireless Switch", DeviceType.WirelessSwitch),
        };

        public static DeviceType? GetCategoryByCode(string code)
        {
            return AllCategories.FirstOrDefault(c => c.Code == code)?.DeviceType;
        }
    }
}
