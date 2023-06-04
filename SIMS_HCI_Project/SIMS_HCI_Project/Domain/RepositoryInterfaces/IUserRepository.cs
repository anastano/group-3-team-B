using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IUserRepository
    {
        User GetById(int id);
        User GetByUsername(string username);
        List<User> GetByUserRole(UserRole userRole);
        bool CheckIfUsernameExists(string username);

        void Add(User newUser);
        void Update(User user);
    }
}