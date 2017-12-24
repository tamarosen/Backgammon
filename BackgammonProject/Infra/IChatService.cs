using BackgammonProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonProject.Infra
{
    public interface IChatService
    {
        IList<Message> GetChatHistory(Contact contact, User user);
        IList<Message> SendMessage(Message message);
    }
}
