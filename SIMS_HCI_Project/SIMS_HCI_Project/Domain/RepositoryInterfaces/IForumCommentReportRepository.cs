using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IForumCommentReportRepository
    {
        void Add(ForumCommentReport report);
        List<ForumCommentReport> GetAll();
        ForumCommentReport GetById(int id);
        ForumCommentReport GetByOwnerIdAndCommentId(int ownerId, int commentId);
    }
}