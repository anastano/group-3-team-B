using System;
using System.Collections.Generic;
using System.Linq;
using SIMS_HCI_Project.Serializer;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class TourRating : ISerializable
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

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                GuestId.ToString(),
                ReservationId.ToString(),
                GuideId.ToString(),
                OverallExperience.ToString(),
                Organisation.ToString(),
                Interestingness.ToString(),
                GuidesKnowledge.ToString(),
                GuidesLanguage.ToString(),
                Comment,
                string.Join(",", Images),
                IsValid.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            GuestId = int.Parse(values[1]);
            ReservationId = int.Parse(values[2]);
            GuideId = int.Parse(values[3]);
            OverallExperience = int.Parse(values[4]);
            Organisation = int.Parse(values[5]);
            Interestingness = int.Parse(values[6]);
            GuidesKnowledge = int.Parse(values[7]);
            GuidesLanguage = int.Parse(values[8]);
            Comment = values[9];
            Images = new List<string>(values[10].Split(","));
            IsValid = bool.Parse(values[11]);
        }

        public double GetAverageRating
        {
            get
            {
                return (double)(OverallExperience + Organisation + Interestingness + GuidesKnowledge + GuidesLanguage) / 5;
            }
        }
    }
}
