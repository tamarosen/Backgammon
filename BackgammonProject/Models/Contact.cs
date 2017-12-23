using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonProject.Models
{
    public class Contact : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public bool IsOnline { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
