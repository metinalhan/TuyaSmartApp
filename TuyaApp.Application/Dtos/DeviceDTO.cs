using TuyaApp.Application.Enums;

namespace TuyaApp.Application.Dtos
{
    public class DeviceDTO
    {
        public string DeviceName { get; set; }
        public string DeviceTuyaId { get; set; }
        public DeviceType DeviceType { get; set; }
        public int NumberOfSwitch { get; set; }
        public bool IsDefault { get; set; }
        public int Id { get; set; }
    }
}
