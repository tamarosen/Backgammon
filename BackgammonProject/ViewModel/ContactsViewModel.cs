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
    public class ContactsViewModel : ViewModelBase
    {
#region Services
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
#endregion

#region Commands
        public ICommand ChatCommand { get; set; }
        //add game command later
#endregion

#region Properties
        public User CurrentUser { get; set; }
        public Contact SelectedContact { get; set; } //need to add binding to selected contact somehow
        public List<Contact> Contacts { get; set; }
        public List<Contact> OnlineContacts { get; set; }
        public List<Contact> OfflineContacts { get; set; }
#endregion

#region Constructor        
        public ContactsViewModel(List<Contact> contacts, INavigationService navigationService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            Contacts = contacts;
            OnlineContacts = new List<Contact>();
            OfflineContacts = new List<Contact>();

            foreach (var contact in Contacts)
            {
                if (contact.IsOnline)
                    OnlineContacts.Add(contact);
                else
                    OfflineContacts.Add(contact);
            }

            ChatCommand = new RelayCommand(() =>
            {
                if (SelectedContact == null)
                    _dialogService.ShowMessageBox("You must select a contact to chat with.", "Error");
                else
                    _navigationService.NavigateTo("ChatPage");
                _navigationService.NavigateTo("ChatPage");
            });
#endregion
        }
    }
}
