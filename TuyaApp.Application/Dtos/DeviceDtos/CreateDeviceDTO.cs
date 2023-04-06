using TuyaApp.Domain.Entities;

namespace TuyaApp.Application.Dtos.DeviceDtos
{
    public class CreateDeviceDTO
    {
        public string DeviceName { get; set; }
        public string DeviceTuyaId { get; set; }
        public int NumberOfSwitch { get; set; }
        public int DeviceType { get; set; }
        public TuyaAccount TuyaAccount { get; set; }
    }
}
