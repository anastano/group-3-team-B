using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class TourRatingGrades
    {
        public int OverallExperience { get; set; }
        public int Organisation { get; set; }
        public int Interestingness { get; set; }
        public int GuidesKnowledge { get; set; }
        public int GuidesLanguage { get; set; }

        public TourRatingGrades()
        {
            OverallExperience = 5;
            Organisation = 5;
            Interestingness = 5;
            GuidesKnowledge = 5;
            GuidesLanguage = 5;
        }

        public double AverageRating()
        {
            return (double)(OverallExperience + Organisation + Interestingness + GuidesKnowledge + GuidesLanguage) / 5;
        }
    }
}
