using TuyaApp.Application.Abstractions.Repositories;
using TuyaApp.Domain.Entities;
using TuyaApp.Persistence.Context;

namespace TuyaApp.Persistence.Repositories
{
    public class MenuRepository : Repository<MenuProfile>, IMenuRepository
    {
        public MenuRepository(TuyaAppDbContext context) : base(context)
        {
        }
    }
}
