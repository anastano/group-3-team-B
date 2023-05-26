

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum TitleStatus { EXPIRED, ACTIVE };
    public class SuperGuestTitle
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public Guest1 Guest { get; set; }
        public DateTime ActivationDate { get; set; }
        public int AvailablePoints { get; set; }
        public TitleStatus Status { get; set; }

        public SuperGuestTitle() { }
        public SuperGuestTitle(Guest1 guest)
        {
            Id = -1;
            GuestId = guest.Id;
            Guest = guest;
            ActivationDate = DateTime.Now;
            AvailablePoints = 5;
            Status = TitleStatus.ACTIVE;
        }
        public SuperGuestTitle(Guest1 guest, DateTime activationDate)
        {
            Id = -1;
            GuestId = guest.Id;
            Guest = guest;
            ActivationDate = activationDate;
            AvailablePoints = 5;
            Status = TitleStatus.ACTIVE;
        }
    }
}
