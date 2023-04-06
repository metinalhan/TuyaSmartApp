using TuyaApp.Domain.Entities;

namespace TuyaApp.Application.Dtos
{
    public class DashboardSaveDTO
    {       
        public string DeviceTuyaId { get; set; }
        public double PositionLeft { get; set; }
        public double PositionTop { get; set; }
        public Device device { get; set; }
    }
}
