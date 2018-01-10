using BackgammonProject.Models;
using BackgammonProject.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System.Threading;
using Windows.UI.Xaml;

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
        public ICommand LogOutCommand { get; set; }
        #endregion

        #region Properties
        public string CurrentUserName { get; set; }
        public string SelectedItem { get; set; } //need to add binding to selected contact somehow
        public ObservableCollection<string> OnlineContacts { get; set; }
        public ObservableCollection<string> OfflineContacts { get; set; }
        private TalkBackAppContext ctx;
        private bool _chatRequestActive;
        private ThreadPoolTimer timer;
        
        #endregion

#region Constructor        
        public ContactsViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _chatRequestActive = false;

            OnlineContacts = new ObservableCollection<string>();
            OfflineContacts = new ObservableCollection<string>();
            ctx = TalkBackAppContext.Get();
            ctx.ContactsWindow = this;
            CurrentUserName = ctx.CurrentUser.Name;

            ReloadContacts();

            StartChat = new RelayCommand(() =>
            {
                if (SelectedItem == null)
                    _dialogService.ShowMessageBox("You must select a contact to chat with.", "Error");
                else
                {
                    ctx.PeerUserName = SelectedItem;
                    _chatRequestActive = true;
                    ChatRequest req = new ChatRequest { From = CurrentUserName, To = SelectedItem };
                    ctx.Dispatcher.SendObjectAsync(req);
                    timer = ThreadPoolTimer.CreateTimer(OnNoResponse, new TimeSpan(0, 0, 59));
                }
            });

            LogOutCommand = new RelayCommand(() =>
            {
                ctx.MyWebSocket.Close(1000, "User signedoff");
                ctx.Dispatcher.EstablishConnectionAsync();
                _navigationService.NavigateTo(ViewModelLocator.LoginPageKey);
                Window.Current.Activate();
            });
        }

        private void OnNoResponse(ThreadPoolTimer timer)
        {
            _chatRequestActive = false;
            _dialogService.ShowError("Timeout: no response from peer, retry or select someone else", "Timeout", "OK", null);
        }
#endregion

        private void ReloadContacts()
        {
            OnlineContacts.Clear();
            OfflineContacts.Clear();
            foreach (var contact in ctx.Contacts)
            {
                if (contact.Value)
                    OnlineContacts.Add(contact.Key);
                else
                    OfflineContacts.Add(contact.Key);
            }

            RaisePropertyChanged(nameof(OnlineContacts));
            RaisePropertyChanged(nameof(OfflineContacts));
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
            RaisePropertyChanged(nameof(OnlineContacts));
            RaisePropertyChanged(nameof(OfflineContacts));
        }

        public void ChatRequestReceived(ChatRequest req)
        {
            if (!_chatRequestActive)
            {
                ctx.PeerUserName = req.From;
                _navigationService.NavigateTo(ViewModelLocator.ChatPageKey);
                ChatRequestResponse resp = new ChatRequestResponse { Success = true, ErrorMessage = "", From = CurrentUserName, To = req.From };
                ctx.Dispatcher.SendObjectAsync(resp);
                Window.Current.Activate();
            }
        }

        public void ChatResponseReceived(ChatRequestResponse resp)
        {
            if (resp.Success && _chatRequestActive)
            {
                timer.Cancel();
                _chatRequestActive = false;
                _navigationService.NavigateTo(ViewModelLocator.ChatPageKey);
                Window.Current.Activate();
                return;
            }

            if (!resp.Success && _chatRequestActive)
            {
                timer.Cancel();
                _chatRequestActive = false;
                _dialogService.ShowError(resp.ErrorMessage, "Error", "OK", null);
            }
        }
    }
}
