using SIMS_HCI_Project.Domain.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SIMS_HCI_Project.FileHandlers
{
    public class TourRatingFileHandler
    {
        private const string FilePath = "../../../Resources/Database/tourRatings.csv";
        private const char Delimiter = '|';

        public TourRatingFileHandler() { }

        public List<TourRating> Load()
        {
            List<TourRating> ratings = new List<TourRating>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                TourRating rating = new TourRating();

                rating.Id = int.Parse(csvValues[0]);
                rating.AttendanceId = int.Parse(csvValues[1]);
                rating.RatingGrades.OverallExperience = int.Parse(csvValues[2]);
                rating.RatingGrades.Organisation = int.Parse(csvValues[3]);
                rating.RatingGrades.Interestingness = int.Parse(csvValues[4]);
                rating.RatingGrades.GuidesKnowledge = int.Parse(csvValues[5]);
                rating.RatingGrades.GuidesLanguage = int.Parse(csvValues[6]);
                rating.Comment = csvValues[7];
                rating.Images = new List<string>(csvValues[8].Split(","));
                rating.IsValid = bool.Parse(csvValues[9]);

                ratings.Add(rating);
            }

            return ratings;
        }

        public void Save(List<TourRating> ratings)
        {
            StringBuilder csv = new StringBuilder();

            foreach (TourRating rating in ratings)
            {
                string[] csvValues =
                {
                    rating.Id.ToString(),
                    rating.Attendance.Id.ToString(),
                    rating.RatingGrades.OverallExperience.ToString(),
                    rating.RatingGrades.Organisation.ToString(),
                    rating.RatingGrades.Interestingness.ToString(),
                    rating.RatingGrades.GuidesKnowledge.ToString(),
                    rating.RatingGrades.GuidesLanguage.ToString(),
                    rating.Comment,
                    string.Join(",", rating.Images),
                    rating.IsValid.ToString()
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}
