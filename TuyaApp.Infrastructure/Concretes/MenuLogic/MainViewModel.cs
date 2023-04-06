using System.Collections.ObjectModel;
using TuyaApp.Application.Abstractions.MenuLogic;
using TuyaApp.Application.ViewModels;

namespace TuyaApp.Infrastructure.Concretes.MenuLogic
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public ObservableCollection<MenuViewModel> Menus { get; }
           = new ObservableCollection<MenuViewModel>();
    }
}
