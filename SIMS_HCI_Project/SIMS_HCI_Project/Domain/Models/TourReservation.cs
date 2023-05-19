using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Serializer;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum TourReservationStatus { CANCELLED, GOING };

    public class TourReservation
    {
        public int Id { get; set; }
        public int TourTimeId { get; set; }
        public TourTime TourTime { get; set; }
        public int Guest2Id { get; set; }
        public Guest2 Guest2 { get; set; }
        public int PartySize { get; set; }
        public TourReservationStatus Status { get; set; }
        public int VoucherUsedId { get; set; } 
        public TourVoucher TourVoucher { get; set; }

        public TourReservation()
        {
        }

        public TourReservation(int tourTimeId, int guest2Id, int partySize, int voucherId)
        {
            TourTimeId = tourTimeId;
            Guest2Id = guest2Id;
            PartySize = partySize;
            Status = TourReservationStatus.GOING;
            VoucherUsedId = voucherId;
        }

        public TourReservation(TourReservation temp)
        {
            TourTimeId = temp.TourTimeId;
            Guest2Id = temp.Guest2Id;
            PartySize = temp.PartySize;
            Status = temp.Status;
        }

        public void Cancel()
        {
            this.Status = TourReservationStatus.CANCELLED;
        }
    }
}
