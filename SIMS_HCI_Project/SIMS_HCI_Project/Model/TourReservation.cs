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
        public string Guest2Id { get; set; }
        public int PartySize { get; set; }
        public TourReservationStatus Status { get; set; }
 //       public int VoucherUsedId { get; set; } // TODO: Connect with Voucher class later

        public TourReservation()
        {
        }

        public TourReservation(int tourTimeId, string guest2Id, int partySize)
        {
            TourTimeId = tourTimeId;
            Guest2Id = guest2Id;
            PartySize = partySize;
            Status = TourReservationStatus.GOING;
 //           VoucherUsedId = -1;
            
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourTimeId.ToString(), Guest2Id.ToString(), PartySize.ToString(), Status.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourTimeId = Convert.ToInt32(values[1]);
            Guest2Id = values[2];
            PartySize = Convert.ToInt32(values[3]);
            Enum.TryParse(values[4], out TourReservationStatus Status);
            //VoucherUsedId= Convert.ToInt32(values[5]);
        }
    }
}
