using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class TourRequest
    {
        public TourRequestStatus Status { get; set; }
        public int GuestId { get; set; }
        public Guest2 Guest { get; set; }

        public void Accept()
        {
            this.Status = TourRequestStatus.ACCEPTED;
        }

        public void Invalidate()
        {
            this.Status = TourRequestStatus.INVALID;
        }
    }
}
