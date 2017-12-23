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
        IList<Contact> GetAllContacts(User user);
    }
}
