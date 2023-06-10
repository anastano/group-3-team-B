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
    public class ForumService
    {
        private readonly IForumRepository _forumRepository;

        public ForumService()
        {
            _forumRepository = Injector.Injector.CreateInstance<IForumRepository>();
        }

        public Forum GetById(int id)
        {
            return _forumRepository.GetById(id);
        }
        public List<Forum> GetByGuestId(int userId)
        {
            return _forumRepository.GetByUserId(userId);
        }
        public List<Forum> GetForumsExcludingGuests(int userId)
        {
            return _forumRepository.GetForumsExcludingUsers(userId);
        }
        public List<Forum> GetAll()
        {
            return _forumRepository.GetAll();
        }
        public void CloseForum(int id)
        {
            _forumRepository.CloseForum(id);
        }
        public Forum Add(Forum forum)
        {
            return _forumRepository.Add(forum);
        }
        public bool IsUSeful(Forum forum)
        {
            bool ownerCommentsFlag = forum.Comments.FindAll(c => c.IsUseful == true && c.User.Role == UserRole.OWNER).Count >= 10;
            bool guestCommentsFlag = forum.Comments.FindAll(c => c.IsUseful == true && (c.User.Role == UserRole.GUEST1 || c.User.Role == UserRole.GUEST2)).Count >= 20;
            return (ownerCommentsFlag && guestCommentsFlag);
        }
    }
}
