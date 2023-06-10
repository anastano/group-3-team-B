using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class ForumCommentService
    {
        private readonly IForumCommentRepository _forumCommentRepository;
        private readonly IForumCommentReportRepository _reportRepostory;

        public ForumCommentService()
        {
            _forumCommentRepository = Injector.Injector.CreateInstance<IForumCommentRepository>();
            _reportRepostory = Injector.Injector.CreateInstance<IForumCommentReportRepository>();
        }

        public ForumComment GetById(int id)
        {
            return _forumCommentRepository.GetById(id);
        }

        public List<ForumComment> GetAll()
        {
            return _forumCommentRepository.GetAll();
        }
        public List<ForumComment> GetByForumId(int forumId)
        {
            return _forumCommentRepository.GetByForumId(forumId);
        } 
        public void Add(ForumComment comment)
        {
            _forumCommentRepository.Add(comment);
        }

        public bool ReportComment(Owner owner, ForumComment forumComment)
        {
            if (_reportRepostory.GetByOwnerIdAndCommentId(owner.Id, forumComment.Id) == null)
            {
                forumComment.ReportCounter++;
                _forumCommentRepository.Update(forumComment);
                _reportRepostory.Add(new ForumCommentReport(owner, forumComment));
                return true;
            }
            return false;
        }
    }
}
