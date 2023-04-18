using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum VoucherStatus { EXPIRED, USED, VALID};
    public class TourVoucher : ISerializable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int GuestId { get; set; }
        public Guest2 Guest { get; set; }
        public DateTime AquiredDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public VoucherStatus Status { get; set; }

        public TourVoucher() { }

        public TourVoucher(int guestId, string title, DateTime aquiredDate, DateTime expirationDate)
        {
            GuestId = guestId;
            Title = title;
            AquiredDate = aquiredDate;
            ExpirationDate = expirationDate;
            Status = VoucherStatus.VALID;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Title, GuestId.ToString(), AquiredDate.ToString("M/d/yyyy h:mm:ss tt"), ExpirationDate.ToString("M/d/yyyy h:mm:ss tt"), Status.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Title = values[1];
            GuestId = Convert.ToInt32(values[2]);
            AquiredDate = DateTime.ParseExact(values[3], "M/d/yyyy h:mm:ss tt", null);
            ExpirationDate = DateTime.ParseExact(values[4], "M/d/yyyy h:mm:ss tt", null);
            Enum.TryParse(values[5], out VoucherStatus status);
            Status = status;

        }
    }
}
