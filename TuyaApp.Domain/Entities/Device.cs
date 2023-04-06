using TuyaApp.Domain.Common;

namespace TuyaApp.Domain.Entities
{
    public class Device:BaseEntity
    {
        public string DeviceName { get; set; }
        public int DeviceType { get; set; }
        public string DeviceTuyaId { get; set; }
        public int NumberOfSwitch { get; set; }
        public string DefaultFunction { get; set; }
        public bool IsDefault { get; set; }
        public TuyaAccount TuyaAccount { get; set; }
        public Dashboard Dashboard { get; set; }
    }
}
