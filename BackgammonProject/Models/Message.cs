using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonProject.Models
{
    public class Message : INotifyPropertyChanged
    {
        public string MessageContent { get; set; }
        public DateTime TimeSent { get; set; }
        public Contact Sender { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
