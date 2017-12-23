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
        public ICommand LoginCommand { get; set; }

        public User CurrentUser { get; set; }

        public LoginViewModel(ILoginService loginService, IDialogService dialogService)
        {
            _loginService = loginService;
            _dialogService = dialogService;

            CurrentUser = new User();

            LoginCommand = new RelayCommand(() =>
            {
                var contactsList = _loginService.GetAllContacts(CurrentUser);

                if (contactsList == null)
                    _dialogService.ShowMessageBox("Password incorrect", "Error");

                else
                    ;//navigate to contects page
            });
        }
    }
}
