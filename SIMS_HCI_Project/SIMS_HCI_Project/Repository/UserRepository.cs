using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repository
{
    public class UserRepository
    {
        private const string FilePath = "../../../Resources/Database/users.csv";

        private readonly Serializer<User> _serializer;

        private List<User> _users;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
        }

        public User GetByUsernameAndPassword(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

    }
}
