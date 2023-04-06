using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using TuyaApp.Application.Abstractions.MenuLogic;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Consts;
using TuyaApp.Application.Dtos;
using TuyaApp.Application.ViewModels;
using TuyaApp.Domain.Entities;
using MessageBox = System.Windows.MessageBox;

namespace TuyaApp.Infrastructure.Concretes.Services
{
    public class MenuLogicService : IMenuLogicService
    {
        private readonly IMainViewModel _mainViewModel;
        public event EventHandler MenuItemClick;
        private readonly NotifyIcon _notifyIcon;
        private readonly IDeviceService _deviceService;

        public MenuLogicService(IMainViewModel mainViewModel, NotifyIcon notifyIcon, IDeviceService deviceService)
        {
            _mainViewModel = mainViewModel;
            _notifyIcon = notifyIcon;
            _deviceService = deviceService;
        }

        public void MenuAdd(string menuAdi, object menu)
        {
            // Declare a variable to hold the MenuViewModel object
            MenuViewModel groupViewModel;

            // Check if the menu parameter is not null and is of type MenuViewModel
            if (menu != null && menu is MenuViewModel)
            {
                // If it is, cast it to MenuViewModel and assign it to groupViewModel
                groupViewModel = menu as MenuViewModel;

                // Create a new SubMenuModel object with the provided subMenuName and a new Guid
                var userModel = new SubMenuModel(
                           subMenuName: menuAdi,
                           guid: Guid.NewGuid().ToString()
                           );

                // Add the userModel to the groupViewModel
                groupViewModel.AddSubMenu(userModel);

            }
            else
            {
                // If the menu parameter is not a MenuViewModel, create a new MenuModel object
                uint lastIndex = (uint)_mainViewModel.Menus.Count;
                var groupModel = new MenuModel(menuAdi, Guid.NewGuid().ToString(), lastIndex);

                // Create a new MenuViewModel with the groupModel and assign it to groupViewModel
                groupViewModel = new MenuViewModel(groupModel);

                // Add the groupViewModel to the _mainViewModel.Menus list
                _mainViewModel.Menus.Add(groupViewModel);
            }
        }

        // This method applies a MenuProfile to the application tray menu
        public void ApplyMenuProfile(MenuProfile profile = null)
        {
            // Create a new ContextMenuStrip
            var contextMenuStrip = new ContextMenuStrip();

            // If a profile is provided, build the menu items based on the profile
            if (profile != null)
                BuildMenuItems(contextMenuStrip, profile);

            // Create a new default menu item
            var defaultMenuItem = new ToolStripMenuItem();

            // Set the text color, name, and text of the default menu item
            defaultMenuItem.ForeColor = Color.Blue;
            defaultMenuItem.Name = DefaultMenu.Dashboard;
            defaultMenuItem.Text = DefaultMenu.Dashboard;

            // Add an event handler for when the default menu item is clicked
            defaultMenuItem.Click += ToolStripMenuItem_Click;

            // Add the default menu item to the contextMenuStrip
            contextMenuStrip.Items.Add(defaultMenuItem);

            // Set the contextMenuStrip as the ContextMenuStrip for the _notifyIcon
            _notifyIcon.ContextMenuStrip = contextMenuStrip;
        }

        // This method builds the menu items for the system tray menu based on the provided MenuProfile
        private ContextMenuStrip BuildMenuItems(ContextMenuStrip contextMenuStrip, MenuProfile profile)
        {
            // Get the list of MenuLists from the MenuProfile
            var list = profile.MenuSave;

            // Loop through each MenuList in the list
            foreach (var item in list.MenuLists)
            {
                // Create a new ToolStripMenuItem
                var menuItem = new ToolStripMenuItem();

                // Set the Tag and Text properties of the menuItem
                menuItem.Tag = item;
                menuItem.Text = item.Name;

                // If the MenuList has submenus, create a subitem for each one
                if (item.SubMenus.Count > 0)
                {
                    foreach (var subItem in item.SubMenus)
                    {
                        var subMenu = new ToolStripMenuItem();

                        // Set the Tag and Text properties of the subMenu
                        subMenu.Tag = subItem;
                        subMenu.Text = subItem.SubMenuName;

                        // Add an event handler for when the subMenu is clicked
                        subMenu.Click += ToolStripMenuItem_Click;

                        // Add the subMenu to the menuItem's DropDownItems collection
                        menuItem.DropDownItems.Add(subMenu);
                    }
                }
                else
                {
                    // If the MenuList has no submenus, add an event handler for when the menuItem is clicked
                    menuItem.Click += ToolStripMenuItem_Click;
                }

                // Add the menuItem to the contextMenuStrip
                contextMenuStrip.Items.Add(menuItem);
            }

            // Return the contextMenuStrip
            return contextMenuStrip;
        }

        public MenuSave MenuSave()
        {
            // Create a new MenuSave object to return
            var menuSave = new MenuSave();

            // Loop through each menu item in the MainViewModel's Menus collection
            foreach (var item in _mainViewModel.Menus)
            {
                // Create a new MenuMod object to represent the current menu item
                var menuMod = new MenuMod();

                // Set the properties of the MenuMod object based on the corresponding properties of the MainViewModel's menu item
                menuMod.Name = item.Model.Name;
                menuMod.Guid = item.Model.Guid;
                menuMod.Priority = (int)item.Model.Priority;
                menuMod.DeviceName = item.Model.DeviceName;
                menuMod.DeviceId = item.Model.DeviceId;
                menuMod.ButtonFunction = item.Model.ButtonFunction;

                // If the current menu item has any sub-menus...
                if (item.SubMenus.Count > 0)
                {
                    // Loop through each sub-menu item in the current menu item's SubMenus collection
                    foreach (var subItem in item.SubMenus)
                    {
                        // Create a new SubMenuMod object to represent the current sub-menu item
                        var subMenuMod = new SubMenuMod();

                        // Set the properties of the SubMenuMod object based on the corresponding properties of the sub-menu item
                        subMenuMod.SubMenuName = subItem.Model.SubMenuName;
                        subMenuMod.Guid = subItem.Model.Guid;
                        subMenuMod.DeviceName = subItem.Model.DeviceName;
                        subMenuMod.DeviceId = subItem.Model.DeviceId;
                        subMenuMod.ButtonFunction = subItem.Model.ButtonFunction;

                        // Add the SubMenuMod object to the current MenuMod object's SubMenus collection
                        menuMod.SubMenus.Add(subMenuMod);
                    }
                }

                // Add the MenuMod object to the MenuSave object's MenuLists collection
                menuSave.MenuLists.Add(menuMod);
            }

            // Return the MenuSave object
            return menuSave;
        }

        private void ToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            // Invoke the MenuItemClick event, passing the sender and EventArgs objects from the ToolStripMenuItem click event
            MenuItemClick?.Invoke(sender, e);
        }

        public void MenuRemove(object menu)
        {
            // Check if the menu object is a MenuViewModel object
            if (menu is MenuViewModel)
            {
                // Remove the MenuViewModel object from the _mainViewModel.Menus collection
                _mainViewModel.Menus.Remove(menu as MenuViewModel);

                // Loop through the remaining MenuViewModel objects and reassign their priority values based on their index in the collection
                for (int i = 0; i < _mainViewModel.Menus.Count; i++)
                {
                    _mainViewModel.Menus[i].Model.Priority = (uint)i;
                }
            }
            else
            {
                // If the menu object is not a MenuViewModel object, assume it is a SubMenuViewModel object
                var sec = menu as SubMenuViewModel;

                // Find the SubMenuViewModel object to remove from the main menu structure
                var subMenuToRemove = _mainViewModel.Menus.SelectMany(s => s.SubMenus)
                    .Where(w => w.Model.Guid == sec.Model.Guid)
                    .FirstOrDefault();

                // Loop through all MenuViewModel objects and remove the SubMenuViewModel object from their SubMenus collections
                foreach (var item in _mainViewModel.Menus)
                {
                    item.SubMenus.Remove(subMenuToRemove);
                }
            }
        }

        public void MenuUp(object menu)
        {
            // Cast the passed object to a MenuViewModel
            var select = menu as MenuViewModel;

            // If the selected menu item is already at the top, return without doing anything
            if (select.Model.Priority == 0)
                return;

            // Find the menu item directly above the selected menu item in the main menu structure
            var temp = _mainViewModel.Menus.Where(w => w.Model.Priority == select.Model.Priority - 1).FirstOrDefault();

            // Increment the priority of the menu item above the selected menu item
            temp.Model.Priority++;

            // Get the indices of the selected menu item and the menu item above it in the main menu structure
            int indis = _mainViewModel.Menus.IndexOf(temp);
            int indis2 = _mainViewModel.Menus.IndexOf(select);

            // Swap the positions of the selected menu item and the menu item above it in the main menu structure
            _mainViewModel.Menus[indis] = select;
            _mainViewModel.Menus[indis2] = temp;

            // Decrement the priority of the selected menu item
            select.Model.Priority--;
        }

        public void MenuDown(object menu)
        {
            // Cast the passed object to a MenuViewModel
            var select = menu as MenuViewModel;

            // If the selected menu item is already at the bottom, return without doing anything
            if (select.Model.Priority == _mainViewModel.Menus.Count - 1)
                return;

            // Find the menu item directly below the selected menu item in the main menu structure
            var temp = _mainViewModel.Menus.Where(w => w.Model.Priority == select.Model.Priority + 1).FirstOrDefault();

            // Decrement the priority of the menu item below the selected menu item
            temp.Model.Priority--;

            // Get the indices of the selected menu item and the menu item below it in the main menu structure
            int indis = _mainViewModel.Menus.IndexOf(temp);
            int indis2 = _mainViewModel.Menus.IndexOf(select);

            // Swap the positions of the selected menu item and the menu item below it in the main menu structure
            _mainViewModel.Menus[indis] = select;
            _mainViewModel.Menus[indis2] = temp;

            // Increment the priority of the selected menu item
            select.Model.Priority++;
        }

        public void ShowMenuContent(MenuProfile menu_row)
        {
            // Retrieve the MenuSave object from the MenuProfile
            var menuList = menu_row.MenuSave;

            // Loop through each MenuMod object in the MenuSave object
            foreach (var item in menuList.MenuLists)
            {
                // Create a new MenuModel object using the properties of the MenuMod object
                var menumodel = new MenuModel(item.Name, item.Guid, (uint)item.Priority);
                menumodel.DeviceName = item.DeviceName;
                menumodel.DeviceId = item.DeviceId;
                menumodel.ButtonFunction = item.ButtonFunction;

                // Create a new MenuViewModel object using the MenuModel object
                var menu = new MenuViewModel(menumodel);

                // If the MenuMod object has SubMenuMod objects, loop through each one
                if (item.SubMenus.Count > 0)
                    foreach (var subItem in item.SubMenus)
                    {
                        // Create a new SubMenuModel object using the properties of the SubMenuMod object
                        var subMenuModel = new SubMenuModel(subItem.SubMenuName, subItem.Guid);
                        subMenuModel.DeviceName = subItem.DeviceName;
                        subMenuModel.DeviceId = subItem.DeviceId;
                        subMenuModel.ButtonFunction = subItem.ButtonFunction;

                        // Add the SubMenuModel object to the MenuViewModel object as a submenu
                        menu.AddSubMenu(subMenuModel);
                    }

                // Add the MenuViewModel object to the list of menus in the MainViewModel
                _mainViewModel.Menus.Add(menu);
            }
        }

        //Assign Function to Selected Device
        public void AssignFunctionToDevice(object selected, object button_switch, DeviceDTO device)
        {
            // Check if the selected item is a MenuViewModel
            if (selected is MenuViewModel)
            {
                // Cast the selected item to a MenuViewModel
                MenuViewModel selected_menu = selected as MenuViewModel;

                // If the selected menu has submenus, show a warning message
                if (selected_menu.SubMenus.Count > 0)
                    MessageBox.Show("Fonksiyonu sadece alt menüye uygulayabilirsiniz !", "Uyarı", MessageBoxButton.OK);
                else
                {
                    // If the selected menu does not have submenus, update its model with the selected device information
                    var update_model = _mainViewModel.Menus.FirstOrDefault(x => x.Model.Guid == selected_menu.Model.Guid);
                    update_model.Model.DeviceName = device.DeviceName;
                    update_model.Model.DeviceId = device.Id;
                    update_model.Model.ButtonFunction = button_switch.ToString();
                }
            }
            else
            {
                // If the selected item is a SubMenuViewModel, update its model with the selected device information
                var sec = selected as SubMenuViewModel;
                var subMenu = _mainViewModel.Menus.SelectMany(s => s.SubMenus)
                    .Where(w => w.Model.Guid == sec.Model.Guid)
                    .FirstOrDefault();

                subMenu.Model.DeviceName = device.DeviceName;
                subMenu.Model.DeviceId = device.Id;
                subMenu.Model.ButtonFunction = button_switch.ToString();
            }
        }

        // This method asynchronously assigns the default function to the given device and returns a boolean value indicating whether the operation is successful or not.
        public async Task<bool> AssignDefaultFunctionAsync(object selected)
        {
            if (selected is MenuViewModel)
            {
                // If the selected object is a MenuViewModel, retrieve the device and assign the default function to it.
                var selected_menu = selected as MenuViewModel;

                if (selected_menu.SubMenus.Count > 0)
                    MessageBox.Show("Alt Fonksiyona uygulayabilirsiniz !", "Uyarı", MessageBoxButton.OK);
                else
                {
                    int deviceId = selected_menu.Model.DeviceId;
                  Device device = await _deviceService.GetDeviceByIdAsync(deviceId);
                    device.DefaultFunction = selected_menu.Model.ButtonFunction;

                    // Assign the default function to the device and return the result.
                    return await _deviceService.AssignDefaultFunctionToDeviceAsync(device);
                }

                return false;
            }
            else
            {
                // If the selected object is a SubMenuViewModel, retrieve the device and assign the default function to it.
                var selected_menu = selected as SubMenuViewModel;
                int deviceId = selected_menu.Model.DeviceId;
                Device device = await _deviceService.GetDeviceByIdAsync(deviceId);
                device.DefaultFunction = selected_menu.Model.ButtonFunction;

                // Assign the default function to the device and return the result.
                return await _deviceService.AssignDefaultFunctionToDeviceAsync(device);
            }
        }       
    }
}
