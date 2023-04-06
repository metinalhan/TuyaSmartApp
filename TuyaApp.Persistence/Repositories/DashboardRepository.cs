using TuyaApp.Application.Abstractions.Repositories;
using TuyaApp.Domain.Entities;
using TuyaApp.Persistence.Context;

namespace TuyaApp.Persistence.Repositories
{
    public class DashboardRepository : Repository<Dashboard>, IDashboardRepository
    {
        public DashboardRepository(TuyaAppDbContext context) : base(context)
        {
        }
    }
}
