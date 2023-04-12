using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class RatingGivenByOwner : ISerializable
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public AccommodationReservation Reservation { get; set; }
        public int Cleanliness { get; set; }
        public int RuleCompliance { get; set; }
        public string AdditionalComment { get; set; }

        public RatingGivenByOwner()
        {
            Cleanliness = 5;
            RuleCompliance = 5;
        }

        public RatingGivenByOwner(int id, int reservationId, int cleanliness, int ruleCompliance, string additionalComment)
        {
            Id = id;
            ReservationId = reservationId;
            Cleanliness = cleanliness;
            RuleCompliance = ruleCompliance;
            AdditionalComment = additionalComment;
        }

        public RatingGivenByOwner(OwnerGuestRating rating)
        {
            Id = rating.Id;
            ReservationId = rating.ReservationId;
            Cleanliness = rating.Cleanliness;
            RuleCompliance = rating.RuleCompliance;
            AdditionalComment = rating.AdditionalComment;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
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
            ReservationId = int.Parse(values[1]);
            Cleanliness = int.Parse(values[2]);
            RuleCompliance = int.Parse(values[3]);
            AdditionalComment = values[4];
        }
    }
}
