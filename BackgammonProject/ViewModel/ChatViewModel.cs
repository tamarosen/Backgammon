using BackgammonProject.Models;
using BackgammonProject.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BackgammonProject.ViewModel
{
    public class ChatViewModel : ViewModelBase
    {
        #region Properties

        //public User CurrentUser { get; set; }
        //public Contact ChatContact { get; set; }
        //public Message MessageContent { get; set; }

        public ObservableCollection<string> HistoryContent { get; set; }
        public string MyMessage { get; set; }
        #endregion

        #region Commands

        public ICommand SendCommand { get; set; }

        #endregion

        #region Constructor
        public ChatViewModel()
        {
            AppContext ctx = AppContext.Get();
            ctx.ChatWinodw = this;
            
            // temporary code, to replace login window:
            ctx.CurrentUser = new User{ Name = "anonymous", Password = "noPass" };
            ctx.Dispatcher.EstablishConnection();
            // end of temporary code

            HistoryContent = new ObservableCollection<string>();
            SendCommand = new RelayCommand( () =>
            {
                Message msg = new Message()
                {
                    From = ctx.CurrentUser.Name,
                    Content = MyMessage,
                    To = "Roni"
                };
                SendMessage(msg);

                MyMessage = string.Empty;
                RaisePropertyChanged(nameof(MyMessage));
            });
        }

        #endregion

        #region Methods

        public void MessageReceived(string message)
        {
            HistoryContent.Add(message);
            RaisePropertyChanged(nameof(HistoryContent));
        }

        public void SendMessage(Message msg)
        {
            AppContext.Get().Dispatcher.SendObjectAsync(msg);
        }

        #endregion
    }
}

