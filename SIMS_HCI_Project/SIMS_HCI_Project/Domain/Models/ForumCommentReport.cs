using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class ForumCommentReport
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
        public int ForumCommentId { get; set; }
        public ForumComment ForumComment { get; set; }

        public ForumCommentReport() { }
        public ForumCommentReport(Owner owner, ForumComment forumComment)
        {
            OwnerId = owner.Id;
            Owner = owner;
            ForumCommentId = forumComment.Id;
            ForumComment = forumComment;
        }

    }
}
