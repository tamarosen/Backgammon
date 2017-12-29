using BackgammonProject.Models;
using BackgammonProject.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;

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
            AppContext ctx = AppContext.get();
            ctx.ChatWinodw = this;
            
            // temporary code, to replace login window:
            ctx.CurrentUser = "anonymous";
            ctx.Dispatcher.EstablishConnection();
            // end of temporary code

            HistoryContent = new ObservableCollection<string>();
            SendCommand = new RelayCommand( () =>
            {
                Message msg = new Message();
                msg.From = ctx.CurrentUser;
                msg.Content = MyMessage;
                msg.To = "Roni";

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
            AppContext.get().Dispatcher.SendObjectAsync(msg);
        }

        #endregion
    }
}

