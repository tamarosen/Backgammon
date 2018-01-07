using BackgammonProject.Models;
using BackgammonProject.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BackgammonProject.ViewModel
{
    public class ChatViewModel : ViewModelBase
    {
        //public User CurrentUser { get; set; }
        //public Contact ChatContact { get; set; }
        //public Message MessageContent { get; set; }

        public ObservableCollection<string> HistoryContent { get; set; }
        public string MyMessage { get; set; }
        public string PeerUserName { get; set; }

        public ICommand SendCommand { get; set; }


        public ChatViewModel(string peerUserName)
        {
            TalkBackAppContext ctx = TalkBackAppContext.Get();
            ctx.ChatWinodw = this;
            
            HistoryContent = new ObservableCollection<string>();
            SendCommand = new RelayCommand( () =>
            {
                Message msg = new Message()
                {
                    From = ctx.CurrentUser.Name,
                    Content = MyMessage,
                    To = PeerUserName
                };
                SendMessage(msg);

                MyMessage = string.Empty;
                RaisePropertyChanged(nameof(MyMessage));
            });
        }
        

        public void MessageReceived(string message)
        {
            HistoryContent.Add(message);
            RaisePropertyChanged(nameof(HistoryContent));
        }

        public void SendMessage(Message msg)
        {
            TalkBackAppContext.Get().Dispatcher.SendObjectAsync(msg);
        }

        internal void Disconnected(ChatRequestResponse resp)
        {
        }
    }
}

