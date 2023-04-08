using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class UserRepository : IUserRepository
    {
        private UserFileHandler _fileHandler;
        private List<User> _users;

        public UserRepository()
        {
            _fileHandler = new UserFileHandler();
            _users = _fileHandler.Load();
        }

        public User GetById(int id)
        {
            return _users.Find(u => u.Id == id);
        }

        public List<User> GetAll()
        {
            return _users;
        }

        public User GetByUsernameAndPassword(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
