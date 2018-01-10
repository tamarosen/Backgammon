using BackgammonProject.Models;
using BackgammonProject.ViewModel;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace BackgammonProject.Services
{
    class TalkBackAppContext
    {
        // singleton
        public static TalkBackAppContext self = null;

        public UserModel CurrentUser { get; set; }
        public MessageWebSocket MyWebSocket { get; set; }
        public ChatViewModel ChatWinodw { get; set; }
        public MessageDispatcher Dispatcher { get; set; }
        public LoginViewModel LoginWindow { get; set; }
        public ConcurrentDictionary<string, bool> Contacts { get; set; }
        public ContactsViewModel ContactsWindow { get; set; }
        public string PeerUserName { get; internal set; }

        // singleton
        private TalkBackAppContext()
        {
            CurrentUser = null;
            MyWebSocket = null;
            ChatWinodw = null;
            LoginWindow = null;
            Contacts = new ConcurrentDictionary<string, bool>();
            new MessageDispatcher(this);
        }

        public static TalkBackAppContext Get()
        {
            if (self == null)
            {
                self = new TalkBackAppContext(); 
            }
            return self;
        }
    }
}
