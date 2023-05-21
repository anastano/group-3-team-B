using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IUserRepository
    {
        User GetById(int id);
        User GetByUsername(string username);
        bool CheckIfUsernameExists(string username);
        void Add(User newUser);
        List<User> GetByUserRole(UserRole userRole);
    }
}