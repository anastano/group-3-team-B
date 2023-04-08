using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User GetById(int id);
        User GetByUsernameAndPassword(string username, string password);
    }
}