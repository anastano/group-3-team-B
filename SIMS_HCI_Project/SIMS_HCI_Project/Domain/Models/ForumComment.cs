using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class ForumComment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ForumId { get; set; }
        public Forum Forum { get; set; }
        public string Content { get; set; }
        public int ReportCounter { get; set; }
        public bool IsUseful { get; set; }
        public ForumComment() { }
        public ForumComment(User user, Forum forum, String content)
        {
            UserId = user.Id;
            User = user;
            ForumId = forum.Id;
            Forum = forum;
            Content = content;
            ReportCounter = 0;
        }

        ///DELETE ONE OF THE CONSTRUSTORS
        public ForumComment(User user, Forum forum, String content, bool isUseful)
        {
            UserId = user.Id;
            User = user;
            ForumId = forum.Id;
            Forum = forum;
            Content = content;
            ReportCounter = 0;
            IsUseful = isUseful;
        }

    }
}
