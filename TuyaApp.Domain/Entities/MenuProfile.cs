using System.Collections.Generic;
using TuyaApp.Domain.Common;

namespace TuyaApp.Domain.Entities
{
    public class MenuProfile : BaseEntity
    {
        public string ProfilName { get; set; }
        public MenuSave? MenuSave { get; set; }
        public bool IsDefault { get; set; }
        public TuyaAccount TuyaAccount { get; set; }
    }

    public class MenuSave
    {
        public List<MenuMod> MenuLists { get; set; } = new();
    }

    public class MenuMod
    {
        public string Name { get; set; }
        public string Guid { get; set; }
        public int Priority { get; set; }
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string ButtonFunction { get; set; }

        public List<SubMenuMod> SubMenus { get; set; } = new();
    }

    public class SubMenuMod
    {
        public string SubMenuName { get; set; }
        public string Guid { get; set; }
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string ButtonFunction { get; set; }
    }
}
