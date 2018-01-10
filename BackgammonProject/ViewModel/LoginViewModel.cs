using BackgammonProject.Services;
using BackgammonProject.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.Concurrent;
using Windows.UI.Xaml;

namespace BackgammonProject.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        // private readonly ILoginService _loginService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        public ICommand LoginCommand { get; set; }

        public UserModel CurrentUser { get; set; }

        private TalkBackAppContext ctx;

        public LoginViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            ctx = TalkBackAppContext.Get();
            CurrentUser = new UserModel();

            // _loginService = loginService;
            _dialogService = dialogService;
            _navigationService = navigationService;

            ctx.Dispatcher.EstablishConnectionAsync();
            ctx.LoginWindow = this;

            LoginCommand = new RelayCommand(() =>
            {
                // The service sends the user input to the server that checks them in the DB 
                // _loginService.LogIn(CurrentUser);
                //
                TryLogin();

                // When the server finish to check user input, it sends back contcts list and the page navigate
                // or sends error message that triggers a popup window with appropriate message.
                
                //contactList = something from the service
                //if (contactsList == null)
                //    _dialogService.ShowMessageBox("Password incorrect", "Error");
                //else
                //    _navigationService.NavigateTo("ContactsPage", contactsList);
                //_navigationService.NavigateTo("ContactsPage", contactsList);
            });
        }

        private void TryLogin()
        {
            Login login = new Login { Name = CurrentUser.Name, Password = CurrentUser.Password };
            ctx.Dispatcher.SendObjectAsync(login);
        }

        public async Task OnLoginResponseAsync(LoginResponse resp)
        {
            await _dialogService.ShowMessageBox($"Eror: {resp.ErrorMessage}", "Error");
        }

        public void OnContactsListReceived(IList<AbstractXmlSerializable> items)
        {
            ConcurrentDictionary<string, bool> contacts = ctx.Contacts;
            ctx.CurrentUser = CurrentUser;
            foreach (AbstractXmlSerializable item in items)
            {
                Contact contact = (Contact) item;
                if (contact != null)
                {
                    contacts.TryAdd(contact.Name, contact.IsOnline);
                }
            }

            _navigationService.NavigateTo(ViewModelLocator.ContactsPageKey);
            Window.Current.Activate();
        }
    }
}
