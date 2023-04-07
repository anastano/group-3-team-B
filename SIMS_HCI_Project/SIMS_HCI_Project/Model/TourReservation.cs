using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Serializer;

namespace SIMS_HCI_Project.Model
{
    public enum TourReservationStatus { CANCELLED, GOING };
    public class TourReservation : ISerializable
    {
        public int Id { get; set; }
        public TourTime TourTime { get; set; }
        public int TourTimeId { get; set; }
        public Guest2 Guest2 { get; set; }
        public int Guest2Id { get; set; }
        public int PartySize { get; set; }
        public TourReservationStatus Status { get; set; }
        public int VoucherUsedId { get; set; } // TODO: Connect with Voucher class later
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

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourTimeId.ToString(), Guest2Id.ToString(), PartySize.ToString(), Status.ToString(), VoucherUsedId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourTimeId = Convert.ToInt32(values[1]);
            Guest2Id = Convert.ToInt32(values[2]);
            PartySize = Convert.ToInt32(values[3]);
            Enum.TryParse(values[4], out TourReservationStatus status);
            Status = status;
            VoucherUsedId= Convert.ToInt32(values[5]);
        }
    }
}
