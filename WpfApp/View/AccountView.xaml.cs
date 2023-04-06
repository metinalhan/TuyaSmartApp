using FluentValidation;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Dtos.TuyaAccountDtos;
using TuyaApp.Domain.Entities;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for AccountView.xaml
    /// </summary>
    public partial class AccountView : UserControl
    {
        private readonly ITuyaAccountService _accountService;
        private readonly IValidator<CreateNewAccountDTO> _validator;
        private BindingList<string> errors;

        public AccountView(ITuyaAccountService accountService, IValidator<CreateNewAccountDTO> validator)
        {
            InitializeComponent();

            _accountService = accountService;
            _validator = validator;

            errors = new();
            errorList.ItemsSource = errors;

            GetAccountList().GetAwaiter();
        }

        // Call the GetAllTuyaAccountsAsync method of the _accountService object to get a list of Tuya accounts asynchronously
        private async Task GetAccountList()
        {
            var accountList = await _accountService.GetAllTuyaAccountsAsync();

            // Set the ItemsSource property of the lvList object to the accountList retrieved above
            lvList.ItemsSource = accountList;
        }

        private async void Save_ButtonClick(object sender, RoutedEventArgs e)
        {
            // Get the values entered in the tbAccountName, tbClientId, and tbSecret TextBoxes          

            var newAccount = new CreateNewAccountDTO
            {
                AccountName = tbAccountName.Text,
                ClientId = tbClientId.Text,
                Secret = tbSecret.Text
            };

            //Validate all entry
            var results = await _validator.ValidateAsync(newAccount);

            errors.Clear();
            foreach (var result in results.Errors)
            {
                errors.Add("*"+result.ErrorMessage);
            }

            //Check validation if passes
            if (results.IsValid == false)
                return;
           
            // Call the AddTuyaAccountAsync method of the _accountService object to add a new Tuya account asynchronously
            await _accountService.AddTuyaAccountAsync(newAccount);

            tbAccountName.Clear();
            tbClientId.Clear();
            tbSecret.Clear();
            // Refresh the account list
            await GetAccountList();
        }

        private void NewAccount_Click(object sender, RoutedEventArgs e)
        {
            // Show the gridGizle object, which is presumably a UI element for adding a new account
            gridGizle.Visibility = Visibility.Visible;
        }

        private void Close_ButtonClick(object sender, RoutedEventArgs e)
        {
            // Hide the gridGizle object, presumably after the user is done adding a new account
            gridGizle.Visibility = Visibility.Collapsed;
        }

        private async void MakeFavouriteAccount_Click(object sender, RoutedEventArgs e)
        {
            // Get the currently selected Tuya account from the lvList object
            var account = lvList.SelectedItem as TuyaAccount;

            // If no account is selected, return without doing anything
            if (account is null) return;

            // Call the MakeFavouriteAccountAsync method of the _accountService object to mark the selected account as a favorite asynchronously
            await _accountService.MakeFavouriteAccountAsync(account.Id);

            // Refresh the account list
           await GetAccountList();
        }

        private async void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            // Get the currently selected Tuya account from the lvList object
            var account = lvList.SelectedItem as TuyaAccount;

            // If no account is selected, return without doing anything
            if (account is null) return;

            // Show a message box to confirm that the user wants to delete the selected account
            MessageBoxResult result = MessageBox.Show("Seçili Hesap Silinecek, Emin misin ?", "Hesap Sil Onay", MessageBoxButton.YesNo);

            // If the user confirms that they want to delete the account, call the DeleteAccountAsync method of the _accountService object to delete the account asynchronously
            if (result == MessageBoxResult.Yes)
            {
                await _accountService.DeleteAccountAsync(account.Id);
            }

            // Refresh the account list
           await GetAccountList();
        }
    }
}
