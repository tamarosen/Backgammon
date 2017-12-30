using BackgammonProject.Models;
using BackgammonProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace BackgammonProject.Services
{
    class AppContext
    {
        // singleton
        public static AppContext self = null;

        public User CurrentUser { get; set; }
        public MessageWebSocket MyWebSocket { get; set; }
        public ChatViewModel ChatWinodw { get; set; }
        public MessageDispatcher Dispatcher { get; set; }

        // singleton
        private AppContext()
        {
            CurrentUser = null;
            MyWebSocket = null;
            ChatWinodw = null;
            new MessageDispatcher(this);
        }

        public static AppContext Get()
        {
            if (self == null)
            {
                self = new AppContext(); 
            }
            return self;
        }
    }
}
