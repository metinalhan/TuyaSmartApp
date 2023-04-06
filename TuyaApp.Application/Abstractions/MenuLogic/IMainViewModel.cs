using System.Collections.ObjectModel;
using TuyaApp.Application.ViewModels;

namespace TuyaApp.Application.Abstractions.MenuLogic
{
    public interface IMainViewModel
    {
        ObservableCollection<MenuViewModel> Menus { get; }        
    }
}
