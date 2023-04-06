using TuyaApp.Domain.Common;

namespace TuyaApp.Domain.Entities
{
    public class Dashboard : BaseEntity
    {      
        public double PositionLeft { get; set; }
        public double PositionTop { get; set; }      
        public Device Device { get; set; }
    }
}
