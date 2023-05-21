using System;
using System.Collections.Generic;
using System.Linq;
using SIMS_HCI_Project.Serializer;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class TourRating
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public int ReservationId { get; set; }
        public int GuideId { get; set; }
        public TourReservation TourReservation { get; set; }
        public int OverallExperience { get; set; }
        public int Organisation { get; set; }
        public int Interestingness { get; set; }
        public int GuidesKnowledge { get; set; }
        public int GuidesLanguage { get; set; }
        public string Comment { get; set; }
        public List<string> Images { get; set; }
        public bool IsValid { get; set; }
        public GuestTourAttendance Attendance { get; set; } 

        public TourRating()
        {
            OverallExperience = 5;
            Organisation = 5;
            Interestingness = 5;
            GuidesKnowledge = 5;
            GuidesLanguage = 5;
            Comment = "";
            Images = new List<string>();
            IsValid = true;
        }

        // for anastano: let's clean this up, no need for two constructors
        public TourRating( int guestId, int reservationId, int guideId, TourReservation tourReservation, int overallExperience, int organisation, int interestingness, int guidesKnowledge, int guidesLanguage, string comment, List<string> image)
        {
            
            GuestId = guestId;
            ReservationId = reservationId;
            GuideId = guideId;
            OverallExperience = overallExperience;
            Organisation = organisation;
            Interestingness = interestingness;
            GuidesKnowledge = guidesKnowledge;
            GuidesLanguage = guidesLanguage;
            Comment = comment;

            TourReservation = new TourReservation();
            Images = new List<string>();
            IsValid = true;
            //Images.Add(image);
        }

        public TourRating(int guestId, int reservationId, int guideId, TourReservation tourReservation, int overallExperience, int organisation, int interestingness, int guidesKnowledge, int guidesLanguage, string comment, string image)
        {

            GuestId = guestId;
            ReservationId = reservationId;
            GuideId = guideId;
            OverallExperience = overallExperience;
            Organisation = organisation;
            Interestingness = interestingness;
            GuidesKnowledge = guidesKnowledge;
            GuidesLanguage = guidesLanguage;
            Comment = comment;

            TourReservation = new TourReservation();
            Images = new List<string>();
            Images.Add(image);
            IsValid = true;
        }

        public double GetAverageRating => (double)(OverallExperience + Organisation + Interestingness + GuidesKnowledge + GuidesLanguage) / 5;
    }
}
