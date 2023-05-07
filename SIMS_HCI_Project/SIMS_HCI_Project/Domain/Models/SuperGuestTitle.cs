using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum TitleStatus { EXPIRED, ACTIVE };
    public class SuperGuestTitle : ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public Guest1 Guest { get; set; }
        public DateTime ActivationDate { get; set; }
        public int AvailablePoints { get; set; }
        public TitleStatus Status { get; set; }
        public SuperGuestTitle() { }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                GuestId.ToString(),
                ActivationDate.ToString("MM/dd/yyyy"),
                AvailablePoints.ToString(),
                Status.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            GuestId = int.Parse(values[1]);
            ActivationDate = DateTime.ParseExact(values[2], "MM/dd/yyyy", null);
            AvailablePoints = int.Parse(values[3]);
            Enum.TryParse(values[4], out TitleStatus status);
            Status = status;
        }
    }
}
