using BackgammonProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonProject.Infra
{
    public interface ILoginService
    {
        void LogIn(User user); //CheckUserInput in service side
        //IList<Contact> GetAllContacts(); in server side
    }
}
