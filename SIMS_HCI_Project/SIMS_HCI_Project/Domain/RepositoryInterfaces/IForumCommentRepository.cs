using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface IForumCommentRepository
    {
        ForumComment GetById(int id);
        List<ForumComment> GetAll();
        List<ForumComment> GetByForumId(int forumId);
    }
}
