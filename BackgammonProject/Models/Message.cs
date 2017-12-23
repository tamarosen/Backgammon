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
        public int MyProperty { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
