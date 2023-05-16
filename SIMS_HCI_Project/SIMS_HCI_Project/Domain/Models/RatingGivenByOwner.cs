using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class RatingGivenByOwner
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

        public RatingGivenByOwner(RatingGivenByOwner rating)
        {
            Id = rating.Id;
            ReservationId = rating.ReservationId;
            Cleanliness = rating.Cleanliness;
            RuleCompliance = rating.RuleCompliance;
            AdditionalComment = rating.AdditionalComment;
        }
    }
}
