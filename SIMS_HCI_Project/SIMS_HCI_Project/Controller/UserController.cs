using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Controller
{
    public class UserController
    {
        private UserFileHandler _fileHandler;

        private List<User> _users;

        public UserController()
        {
            _fileHandler = new UserFileHandler();
            _users = _fileHandler.Load();
        }

        public User LogIn(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
