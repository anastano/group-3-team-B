using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SIMS_HCI_Project.Model
{
    public class OwnerGuestRating : ISerializable
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string GuestId { get; set; }
        public int ReservationId { get; set; }

        public int Cleanliness { get; set; }
        public int RuleCompliance { get; set; }
        public string AdditionalComment { get; set; }

        public OwnerGuestRating() {
            Cleanliness = 5;
            RuleCompliance = 5;
        }

        public OwnerGuestRating(int id, string ownerId, string guestId, int reservationId, int cleanliness, int ruleCompliance, string additionalComment)
        {
            Id = id;
            OwnerId = ownerId;
            GuestId = guestId;
            ReservationId = reservationId;
            Cleanliness = cleanliness;
            RuleCompliance = ruleCompliance;
            AdditionalComment = additionalComment;
        }

        public OwnerGuestRating(OwnerGuestRating temp) 
        {
            Id= temp.Id;
            OwnerId = temp.OwnerId;
            GuestId = temp.GuestId;
            ReservationId = temp.ReservationId;
            Cleanliness = temp.Cleanliness;
            RuleCompliance = temp.RuleCompliance;
            AdditionalComment= temp.AdditionalComment;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                OwnerId,
                GuestId,
                ReservationId.ToString(),
                Cleanliness.ToString(),
                RuleCompliance.ToString(),
                AdditionalComment

            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            OwnerId = values[1];
            GuestId = values[2];
            ReservationId = int.Parse(values[3]);
            Cleanliness = int.Parse(values[4]);
            RuleCompliance = int.Parse(values[5]);
            AdditionalComment = values[6];
        }
    }
}
