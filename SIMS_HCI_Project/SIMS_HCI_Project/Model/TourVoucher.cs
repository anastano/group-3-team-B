using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    public class TourVoucher : ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public Guest2 Guest { get; set; }
        //public string GuideId { get; set; } // specification unclear. This is optional? Guide Resign part unclear, mentiones Guide specific vouchers that aren't mentioned anywhere else
        //public Guide Guide { get; set; }
        public DateTime AquiredDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public TourVoucher() { }

        public TourVoucher(int guestId, DateTime aquiredDate, DateTime expirationDate)
        {
            GuestId = guestId;
            AquiredDate = aquiredDate;
            ExpirationDate = expirationDate;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(), AquiredDate.ToString("M/d/yyyy h:mm:ss tt"), ExpirationDate.ToString("M/d/yyyy h:mm:ss tt") };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            AquiredDate = DateTime.ParseExact(values[2], "M/d/yyyy h:mm:ss tt", null);
            ExpirationDate = DateTime.ParseExact(values[3], "M/d/yyyy h:mm:ss tt", null);

        }
    }
}
