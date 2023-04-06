using System;
using System.Threading.Tasks;
using TuyaApp.Application.Dtos;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Application.Abstractions.Services
{
    // This is an interface definition for a service that provides logic related to menu management and functionality.
    public interface IMenuLogicService
    {
        // Event that is raised when a menu item is clicked.
        event EventHandler MenuItemClick;

        // Adds a new menu item with the specified name and object.
        void MenuAdd(string menuAdi, object menu);

        // Removes the specified menu item from the menu.
        void MenuRemove(object menu);

        // Moves the specified menu item up in the menu.
        void MenuUp(object menu);

        // Moves the specified menu item down in the menu.
        void MenuDown(object menu);

        // Shows the content of the specified menu profile.
        void ShowMenuContent(MenuProfile menu);

        // Saves the current menu configuration.
        MenuSave MenuSave();

        // Applies the specified menu profile to the menu.
        void ApplyMenuProfile(MenuProfile profile = null);

        // Assigns a function to the specified device based on the selected menu item and button or switch.
        void AssignFunctionToDevice(object selected, object button_switch, DeviceDTO device);

        // Assigns the default function to the specified device.
        Task<bool> AssignDefaultFunctionAsync(object selected);
    }
}
