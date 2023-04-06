using System.Collections.Generic;
using TuyaApp.Domain.Common;

namespace TuyaApp.Domain.Entities
{
    public class TuyaAccount : BaseEntity
    {
        public string AccountName { get; set; }
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public bool IsDefault { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public ICollection<MenuProfile> Menus { get; set; }
    }
}
