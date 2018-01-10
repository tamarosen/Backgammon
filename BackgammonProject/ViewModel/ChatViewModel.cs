using BackgammonProject.Models;
using BackgammonProject.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace BackgammonProject.ViewModel
{
    public class ChatViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ObservableCollection<string> HistoryContent { get; set; }
        public string MyMessage { get; set; }
        public string PeerUserName { get; set; }

        public ICommand SendCommand { get; set; }
        public ICommand BackCommand { get; set; }
        private TalkBackAppContext ctx;

        public ChatViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            ctx = TalkBackAppContext.Get();
            ctx.ChatWinodw = this;
            PeerUserName = ctx.PeerUserName;

            HistoryContent = new ObservableCollection<string>();
            SendCommand = new RelayCommand(() =>
           {
               MessageModel msg = new MessageModel()
               {
                   From = ctx.CurrentUser.Name,
                   Content = MyMessage,
                   To = PeerUserName
               };
               SendMessage(msg);

               MyMessage = string.Empty;
               RaisePropertyChanged(nameof(MyMessage));
           });

            BackCommand = new RelayCommand(() =>
           {
               _navigationService.NavigateTo(ViewModelLocator.ContactsPageKey);
               Window.Current.Activate();
           });
        }


        public void MessageReceived(string message)
        {
            HistoryContent.Add(message);
            RaisePropertyChanged(nameof(HistoryContent));
        }

        public void SendMessage(MessageModel msg)
        {
            ctx.Dispatcher.SendObjectAsync(msg);
        }

        internal void Disconnected(ChatRequestResponse resp)
        {
        }
    }
}

