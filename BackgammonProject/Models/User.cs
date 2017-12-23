using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonProject.Models
{
    public class User : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsOnline { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
