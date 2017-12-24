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
    public class ChatViewModel : ViewModelBase
    {
        public User CurrentUser { get; set; }
        public Contact ChatContact { get; set; }
        public Message MessageContent { get; set; }
        public List<Message> HistoryContent { get; set; }

        public ICommand SendCommand { get; set; }
        //add game comand later

        public ChatViewModel()
        {
              
            SendCommand = new RelayCommand(() =>
            {

            })
        }
    }
}
