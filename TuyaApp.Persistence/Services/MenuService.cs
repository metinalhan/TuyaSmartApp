using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TuyaApp.Application.Abstractions.Repositories;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Persistence.Services
{
    // A service that provides methods to interact with MenuProfile entities.
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;

        // Constructor injection of IMenuRepository.
        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        // Deletes a menu profile by its ID.
        public async Task<bool> DeleteMenuProfileAsync(int id)
        {
          await _menuRepository.RemoveAsync(id);
           await _menuRepository.SaveAsync();

            return true;
        }

        // Retrieves a list of all menu profiles.
        public async Task<List<MenuProfile>> GetAllMenuAsync() =>        
             await _menuRepository.GetAll().ToListAsync();

        // Retrieves a list of all menu profiles for a given account.
        public async Task<List<MenuProfile>> GetAllMenuByAccountAsync(int account_id) =>        
             await _menuRepository.GetAll(x=>x.TuyaAccount.Id == account_id).ToListAsync();

        // Retrieves the default menu profile for a given account.
        public async Task<MenuProfile> GetDefaultMenuProfileByAccount(int accountId) =>         
             await _menuRepository.GetAll(x => x.IsDefault).FirstOrDefaultAsync();

        // Retrieves a menu profile by its ID.
        public async Task<MenuProfile> GetMenuProfileByIdAsync(int id) =>         
            await _menuRepository.GetById(id).FirstOrDefaultAsync();

        // Sets a menu profile as the default for a given account.
        public async Task<bool> MakeFavouriteMenuProfileAsync(int accountId ,int profile_id)
        {                     
            var menu = await GetAllMenuByAccountAsync(accountId);

            foreach (var item in menu)
            {
                if (item.Id != profile_id)
                    item.IsDefault = false;
                else
                    item.IsDefault = true;
            }

            await _menuRepository.SaveAsync();

            return true;
        }

        // Saves a new menu profile for a given account.
        public async Task<int> SaveMenuAsync(TuyaAccount account, string name)
        {
            var menu = new MenuProfile
            {
                ProfilName = name,               
                TuyaAccount = account
            };

            await _menuRepository.AddAsync(menu);
         return await _menuRepository.SaveAsync();
        }

        // Updates the content of a menu profile by its ID.
        public async Task<bool> UpdateMenuContentAsync(MenuSave menusave,int id)
        {
          var menu = await GetMenuProfileByIdAsync(id);
            menu.MenuSave = menusave;

           await _menuRepository.SaveAsync();
            return true;
        }
    }
}
