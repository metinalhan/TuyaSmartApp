using System.Collections.Generic;
using System.Threading.Tasks;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Application.Abstractions.Services
{
    // This is an interface definition for a service that provides menu management and configuration functionality.
    public interface IMenuService
    {
        // Returns a list of all menu profiles associated with the specified account.
        Task<List<MenuProfile>> GetAllMenuByAccountAsync(int accountId);

        // Returns the menu profile with the specified ID.
        Task<MenuProfile> GetMenuProfileByIdAsync(int profile_id);

        // Updates the menu content of the specified menu profile with the provided data.
        Task<bool> UpdateMenuContentAsync(MenuSave menu,int id);

        // Saves a new menu with the specified name and associated with the specified Tuya account.
        Task<int> SaveMenuAsync(TuyaAccount account, string name);

        // Deletes the specified menu profile.
        Task<bool> DeleteMenuProfileAsync(int id);

        // Marks the specified menu profile as a favourite for the specified account.
        Task<bool> MakeFavouriteMenuProfileAsync(int accountId, int profile_id);

        // Returns the default menu profile associated with the specified account.
        Task<MenuProfile> GetDefaultMenuProfileByAccount(int accountId);
    }
}
