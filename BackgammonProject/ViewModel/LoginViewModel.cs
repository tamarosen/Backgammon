using BackgammonProject.Infra;
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

namespace BackgammonProject.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ILoginService _loginService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        public ICommand LoginCommand { get; set; }

        public User CurrentUser { get; set; }

        public LoginViewModel(ILoginService loginService, IDialogService dialogService, INavigationService navigationService)
        {
            _loginService = loginService;
            _dialogService = dialogService;
            _navigationService = navigationService;

            CurrentUser = new User();

            LoginCommand = new RelayCommand(() =>
            {
                var contactsList = _loginService.GetAllContacts(CurrentUser);

                if (contactsList == null)
                    _dialogService.ShowMessageBox("Password incorrect", "Error");
                else
                    _navigationService.NavigateTo("ContactsPage", contactsList);
                _navigationService.NavigateTo("ContactsPage", contactsList);
            });
        }
    }
}
