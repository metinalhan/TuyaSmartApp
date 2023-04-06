using System.Collections.ObjectModel;

namespace TuyaApp.Application.ViewModels
{
    public class MenuViewModel : ViewModelBase<MenuModel>
    {
        public MenuViewModel(MenuModel model)
            : base(model)
        {
        }

        public ObservableCollection<SubMenuViewModel> SubMenus { get; }
            = new ObservableCollection<SubMenuViewModel>();

        public void AddSubMenu(SubMenuModel menu)
        {
            var viewModel = new SubMenuViewModel(menu);
            SubMenus.Add(viewModel);
        }

    }
}
