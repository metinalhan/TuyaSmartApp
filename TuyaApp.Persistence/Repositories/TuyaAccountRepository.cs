using TuyaApp.Application.Abstractions.Repositories;
using TuyaApp.Domain.Entities;
using TuyaApp.Persistence.Context;

namespace TuyaApp.Persistence.Repositories
{
    public class TuyaAccountRepository : Repository<TuyaAccount>, ITuyaAccountRepository
    {
        public TuyaAccountRepository(TuyaAppDbContext context) : base(context)
        {
        }
    }
}
