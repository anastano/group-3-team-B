using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum ForumStatus { CLOSED, ACTIVE};
    public class Forum
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public ForumStatus Status { get; set; }
        public List<ForumComment> Comments { get; set; }
        public Forum() { }
        public Forum(User user, Location location)
        {
            UserId = user.Id;
            User = user;
            LocationId = location.Id;
            Location = location;
            Status = ForumStatus.ACTIVE;
        }
        public bool IsUseful()
        {
            bool ownerCondition = this.Comments.FindAll(c => c.IsUseful == true && c.User.Role == UserRole.OWNER).Count >= 10;
            bool guestCondition = this.Comments.FindAll(c => c.IsUseful == true && (c.User.Role == UserRole.GUEST1 || c.User.Role == UserRole.GUEST2)).Count >= 20;
            return (ownerCondition && guestCondition);
        }
    }
}
