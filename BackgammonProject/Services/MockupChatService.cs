using BackgammonProject.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackgammonProject.Models;

namespace BackgammonProject.Services
{
    public class MockupChatService : IChatService
    {
        private IList<Message> chatHistory;
        public MockupChatService()
        {
            chatHistory = new List<Message>()
            {
               
            }
        }
        public IList<Message> GetChatHistory(Contact contact, User user)
        {
            throw new NotImplementedException();
        }

        public IList<Message> SendMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
