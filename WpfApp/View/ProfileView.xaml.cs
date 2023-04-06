using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Domain.Entities;
using TuyaApp.Persistence.Services;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : UserControl
    {
        private readonly IMenuLogicService _logicService;
        private readonly IMenuService _menuService;
        private readonly ITuyaAccountService _accountService;

        public ProfileView(IMenuLogicService logicService, IMenuService menuService, ITuyaAccountService accountService)
        {
            InitializeComponent();
            _logicService = logicService;
            _menuService = menuService;
            _accountService = accountService;

            SetAccountList().GetAwaiter();
        }

        private async Task SetAccountList()
        {
            // Call the GetAllTuyaAccountsAsync method of the account service to retrieve all Tuya accounts.
            var list = await _accountService.GetAllTuyaAccountsAsync();
            var dfault = list.FirstOrDefault(x => x.IsDefault == true);
            cbAccount.ItemsSource = list;
            cbAccount.SelectedItem = dfault;
        }

        // This method is called when the Save Profile button is clicked
        private async void SaveProfile_ButtonClick(object sender, RoutedEventArgs e)
        {
            var account = cbAccount.SelectedItem as TuyaAccount;

            if (account is null)
            {
                MessageBox.Show("Profil Atanacak Hesap Seçilmedi !", "Yeni Profil", MessageBoxButton.OK);
                return;
            }

            string profileName = tbProfileName.Text;

            if(profileName.Length ==0 )
            {
                MessageBox.Show("Geçerli Bir Profil Adı Girin !", "Yeni Profil", MessageBoxButton.OK);
                return;
            }

            await _menuService.SaveMenuAsync(account, profileName);

            tbProfileName.Clear();

           await GetMenuProfileList();
        }

        // This method is called when the selection in the list of profiles is changed
        private async Task GetMenuProfileList()
        {
            var account = cbAccount.SelectedItem as TuyaAccount;

            if (account is null)
                return;

            var list = await _menuService.GetAllMenuByAccountAsync(account.Id);

            lvList.ItemsSource = list;
        }

        private async void cbAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected Tuya account from the cbAccount ComboBox.
            var selected = cbAccount.SelectedItem as TuyaAccount;

            if (selected is null)
                return;

            await GetMenuProfileList();
        }

        // This method is called when the Delete Profile button is clicked
        private async void DeleteProfile_Click(object sender, RoutedEventArgs e)
        {
            var profile = lvList.SelectedItem as MenuProfile;

            if (profile is null) return;

            MessageBoxResult result = MessageBox.Show("Seçili Profili Silinecek, Emin misin ?", "Profil Sil Onay", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                await _menuService.DeleteMenuProfileAsync(profile.Id);

            }

           await GetMenuProfileList();
        }

        // This method is called when the "Make Favourite Profile" button is clicked.
        // It makes the selected profile a favourite and updates the menu profile list.
        private async void MakeFavouriteProfile_Click(object sender, RoutedEventArgs e)
        {
            var profile = lvList.SelectedItem as MenuProfile;

            if (profile is null) return;

            await _menuService.MakeFavouriteMenuProfileAsync(profile.TuyaAccount.Id, profile.Id);

            if (profile.MenuSave is not null)
                _logicService.ApplyMenuProfile(profile);

           await GetMenuProfileList();
        }


        // This method is called when the "Close" button is clicked.
        // It hides the grid that contains the form.
        private void Close_ButtonClick(object sender, RoutedEventArgs e)
        {
            gridGizle.Visibility = Visibility.Collapsed;
        }

        // This method is called when the "New Profile" button is clicked.
        // It shows the grid that contains the form.
        private void NewProfile_Click(object sender, RoutedEventArgs e)
        {
            gridGizle.Visibility = Visibility.Visible;
        }
    }
}
