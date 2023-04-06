using TuyaApp.Application.Abstractions.Repositories;
using TuyaApp.Domain.Entities;
using TuyaApp.Persistence.Context;

namespace TuyaApp.Persistence.Repositories
{
    public class DeviceRepository : Repository<Device>, IDeviceRepository
    {
        public DeviceRepository(TuyaAppDbContext context) : base(context)
        {
        }
    }
}
