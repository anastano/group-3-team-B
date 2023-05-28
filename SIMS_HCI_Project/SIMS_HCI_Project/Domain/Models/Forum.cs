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
        public bool IsUseful { get; set; }
        public List<ForumComment> Comments { get; set; }
        public Forum()
        {
            /*
            Location = new Location();
            MaxGuests = 1;
            MinimumReservationDays = 1;
            CancellationDeadlineInDays = 1;
            Images = new List<string>();
            Reservations = new List<AccommodationReservation>();
            IsRenovated = false;
            */
        }
    }
}
