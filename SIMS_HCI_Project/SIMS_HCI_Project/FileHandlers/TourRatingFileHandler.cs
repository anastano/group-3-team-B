using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Model;
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
                rating.GuestId = int.Parse(csvValues[1]);
                rating.ReservationId = int.Parse(csvValues[2]);
                rating.GuideId = int.Parse(csvValues[3]);
                rating.OverallExperience = int.Parse(csvValues[4]);
                rating.Organisation = int.Parse(csvValues[5]);
                rating.Interestingness = int.Parse(csvValues[6]);
                rating.GuidesKnowledge = int.Parse(csvValues[7]);
                rating.GuidesLanguage = int.Parse(csvValues[8]);
                rating.Comment = csvValues[9];
                rating.Images = new List<string>(csvValues[10].Split(","));
                rating.IsValid = bool.Parse(csvValues[11]);

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
                    rating.GuestId.ToString(),
                    rating.ReservationId.ToString(),
                    rating.GuideId.ToString(),
                    rating.OverallExperience.ToString(),
                    rating.Organisation.ToString(),
                    rating.Interestingness.ToString(),
                    rating.GuidesKnowledge.ToString(),
                    rating.GuidesLanguage.ToString(),
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
