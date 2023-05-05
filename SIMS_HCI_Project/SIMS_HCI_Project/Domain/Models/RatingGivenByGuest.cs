using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SIMS_HCI_Project.Domain.Models
{
    public class RatingGivenByGuest : ISerializable
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public AccommodationReservation Reservation { get; set; }
        public int Cleanliness { get; set; }
        public int Correctness { get; set; }
        public string AdditionalComment { get; set; }
        public List<string> Images { get; set; }
        public int RenovationRecommendationId { get; set; }
        public RenovationRecommendation RenovationRecommendation { get; set; }

        public RatingGivenByGuest()
        {
            Cleanliness = 5;
            Correctness = 5;
            Images = new List<string>();
        }

        public RatingGivenByGuest(int reservationId, int cleanliness, int correctness, string additionalComment, List<string> images)
        {
            ReservationId = reservationId;
            Cleanliness = cleanliness;
            Correctness = correctness;
            AdditionalComment = additionalComment;
            Images = images;
        }

        public RatingGivenByGuest(RatingGivenByGuest temp) 
        {
            Id= temp.Id;
            ReservationId = temp.ReservationId;
            Cleanliness = temp.Cleanliness;
            Correctness = temp.Correctness;
            AdditionalComment= temp.AdditionalComment;
            Images = new List<string>();
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                ReservationId.ToString(),
                Cleanliness.ToString(),
                Correctness.ToString(),
                AdditionalComment,
                string.Join(",", Images)

            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            ReservationId = int.Parse(values[1]);
            Cleanliness = int.Parse(values[2]);
            Correctness = int.Parse(values[3]);
            AdditionalComment = values[4];
            Images = new List<string>(values[5].Split(","));
        }
    }
}
