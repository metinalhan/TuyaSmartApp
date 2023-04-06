using System.Collections.Generic;
using System.Threading.Tasks;
using TuyaApp.Application.Dtos.TuyaAccountDtos;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Application.Abstractions.Services
{
    // This is an interface definition for a service that provides Tuya account management and configuration functionality.
    public interface ITuyaAccountService
    {
        // Adds a new Tuya account with the specified account name, client ID, and secret.
        Task AddTuyaAccountAsync(CreateNewAccountDTO account);

        // Returns a list of all Tuya accounts.
        Task<List<TuyaAccount>> GetAllTuyaAccountsAsync();

        // Returns the default Tuya account.
        Task<TuyaAccount> GetDefaultAccountAsync();

        // Deletes the Tuya account with the specified ID.
        Task<bool> DeleteAccountAsync(int accountId);

        // Marks the specified Tuya account as a favourite.
        Task<bool> MakeFavouriteAccountAsync(int accountId);
    }
}
