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
        public int AttendanceId { get; set; }
        public GuestTourAttendance Attendance { get; set; }
        public TourRatingGrades RatingGrades { get; set; } // maybe change names
        public string Comment { get; set; }
        public List<string> Images { get; set; }
        public bool IsValid { get; set; }

        public TourRating()
        {
            RatingGrades = new TourRatingGrades();
            Comment = "";
            Images = new List<string>();
            Attendance = new GuestTourAttendance();
            IsValid = true;
        }

        public double AverageRating => RatingGrades.AverageRating();
    }
}
