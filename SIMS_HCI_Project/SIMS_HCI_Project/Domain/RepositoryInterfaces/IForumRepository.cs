using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface IForumRepository
    { 
        Forum GetById(int id);
        List<Forum> GetAll();
        List<Forum> GetByUserId(int userId);
        List<Forum> GetForumsExcludingUsers(int userId);
        void CloseForum(int id);
        void Add(Forum forum);
    }
}
