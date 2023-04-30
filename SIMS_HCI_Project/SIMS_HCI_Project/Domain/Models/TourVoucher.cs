using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum VoucherStatus { EXPIRED, USED, VALID};
    public class TourVoucher
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
    }
}
