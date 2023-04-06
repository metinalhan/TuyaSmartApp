using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TuyaApp.Application.Abstractions.Repositories;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Dtos.TuyaAccountDtos;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Persistence.Services
{
    public class TuyaAccountService : ITuyaAccountService
    {
        private readonly ITuyaAccountRepository _tuyaAccountRepository;

        // Constructor injection of ITuyaAccountRepository
        public TuyaAccountService(ITuyaAccountRepository tuyaAccountRepository)
        {
            _tuyaAccountRepository = tuyaAccountRepository;
        }

        // Add a new TuyaAccount to the database
        public async Task AddTuyaAccountAsync(CreateNewAccountDTO account)
        {
            var new_account = new TuyaAccount()
            {
                AccountName = account.AccountName,      
                ClientId = account.ClientId,
                Secret = account.Secret,
            };

            await _tuyaAccountRepository.AddAsync(new_account);
            await _tuyaAccountRepository.SaveAsync();
        }

        // Delete a TuyaAccount from the database by ID
        public async Task<bool> DeleteAccountAsync(int accountId)
        {
           await _tuyaAccountRepository.RemoveAsync(accountId);
            await _tuyaAccountRepository.SaveAsync();

            return true;
        }

        // Get all TuyaAccounts from the database as a list
        public async Task<List<TuyaAccount>> GetAllTuyaAccountsAsync() =>
            await _tuyaAccountRepository.GetAll().ToListAsync();

        // Get the default TuyaAccount from the database
        public async Task<TuyaAccount> GetDefaultAccountAsync() =>        
             await _tuyaAccountRepository.GetAll(x => x.IsDefault).FirstOrDefaultAsync();

        // Set a TuyaAccount as the default account
        public async Task<bool> MakeFavouriteAccountAsync(int accountId)
        {
           var list = await GetAllTuyaAccountsAsync();

            foreach (var item in list)
            {
                if (item.Id != accountId)
                    item.IsDefault = false;
                else
                    item.IsDefault = true;
            }

            await _tuyaAccountRepository.SaveAsync();

            return true;
        }
    }
}
