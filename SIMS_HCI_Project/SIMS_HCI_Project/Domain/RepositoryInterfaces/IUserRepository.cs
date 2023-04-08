using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User LogIn(string username, string password);
    }
}