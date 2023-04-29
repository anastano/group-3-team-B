using Microsoft.Win32;
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
            Load();
        }

        private void Load()
        {
            _users = _fileHandler.Load();
        }

        private void Save()
        {
            _fileHandler.Save(_users);
        }

        public User GetById(int id)
        {
            return _users.Find(u => u.Id == id);
        }

        public User GetByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public bool CheckIfUsernameExists(string username)
        {
            return _users.Any(u => u.Username.Equals(username));
        }

        private int GenerateId()
        {
            return _users.Count == 0 ? 1 : _users[_users.Count-1].Id + 1;
        }

        public void Add(User newUser)
        {
            newUser.Id = GenerateId();
            _users.Add(newUser);

            Save();
        }
    }
}
