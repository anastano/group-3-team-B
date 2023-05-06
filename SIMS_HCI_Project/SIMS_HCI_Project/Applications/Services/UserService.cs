using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService() 
        { 
            _userRepository= Injector.Injector.CreateInstance<IUserRepository>();
        }

        public User LogIn(string username, string password)
        {
            User user = _userRepository.GetByUsername(username);

            if (user == null || password != user.Password) return null; // [Update] separate to return some Sucess DTO holding User info and Error info, to indicate what went wrong

            return user;
        }

        public bool SignIn(User newUser)
        {
            if (_userRepository.CheckIfUsernameExists(newUser.Username)) return false;

            _userRepository.Add(newUser);
            return true;
        }
    }
}
