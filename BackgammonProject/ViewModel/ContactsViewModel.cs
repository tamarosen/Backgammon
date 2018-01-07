using BackgammonProject.Models;
using BackgammonProject.Services;
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
        public ICommand StartChat { get; set; }
#endregion

#region Properties
        public string CurrentUserName { get; set; }
        public string SelectedItem { get; set; } //need to add binding to selected contact somehow
        public List<string> OnlineContacts { get; set; }
        public List<string> OfflineContacts { get; set; }
#endregion

#region Constructor        
        public ContactsViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            OnlineContacts = new List<string>();
            OfflineContacts = new List<string>();
            TalkBackAppContext ctx = TalkBackAppContext.Get();
            ctx.ContactsWindow = this;

            foreach (var contact in ctx.Contacts)
            {
                if (contact.Value)
                    OnlineContacts.Add(contact.Key);
                else
                    OfflineContacts.Add(contact.Key);
            }

            StartChat = new RelayCommand(() =>
            {
                if (SelectedItem == null)
                    _dialogService.ShowMessageBox("You must select a contact to chat with.", "Error");
                else
                    _navigationService.NavigateTo("ChatPage", SelectedItem);
            });
#endregion
        }

        public void OnContactUpdate(Contact contact)
        {
            if (contact.IsOnline)
            {
                OfflineContacts.Remove(contact.Name);
                OnlineContacts.Add(contact.Name);
            }
            else
            {
                OnlineContacts.Remove(contact.Name);
                OfflineContacts.Add(contact.Name);

            }
        }
    }
}
