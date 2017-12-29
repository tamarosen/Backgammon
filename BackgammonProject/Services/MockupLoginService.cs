using BackgammonProject.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackgammonProject.Models;

namespace BackgammonProject.Services
{
    public class MockupLoginService : ILoginService
    {
        private IList<User> users;
        private MockupLoginService()
        {
            //Get all users from DB
            users = new List<User>()
            {
                new User(){ Name="tamar", Password="123456"},
                new User(){ Name="ramat", Password="654321"}
            };
        }

        private IList<Contact> ConvertUsersToContacts()
        {
            //Convert to contact model so the password won't be exposed
            List<Contact> contacts = new List<Contact>();
            foreach (var user in users)
            {
                Contact contact = new Contact() { Name = user.Name, IsOnline = true };
                contacts.Add(contact);
            }
            return contacts;
        }

        //Called when the user signs in in the login page.
        public IList<Contact> GetAllContacts(User user)
        {
            User _user = users.FirstOrDefault(u => u.Name == user.Name);
            if (_user == null)
            {
                CreateNewUser(user);
                return ConvertUsersToContacts();
            }
            if (_user.Password != user.Password)
            {
                return null;
            }
            return ConvertUsersToContacts();
        }

        private void CreateNewUser(User user)
        {
            users.Add(new User { Name = user.Name, Password = user.Password });
        }

        public IList<Contact> GetAllContacts()
        {
            throw new NotImplementedException();
        }
    }
}
