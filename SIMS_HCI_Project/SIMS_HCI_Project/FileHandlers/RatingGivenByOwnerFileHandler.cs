using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class RatingGivenByOwnerFileHandler
    {
        private const string path = "../../../Resources/Database/ratingsGivenByOwner.csv";
        private const char Delimiter = '|';
        public RatingGivenByOwnerFileHandler()
        {
        }

        public List<RatingGivenByOwner> Load()
        {
            List<RatingGivenByOwner> ratings = new List<RatingGivenByOwner>();

            foreach (string line in File.ReadLines(path))
            {
                string[] csvValues = line.Split(Delimiter);
                RatingGivenByOwner rating = new RatingGivenByOwner();

                rating.Id = int.Parse(csvValues[0]);
                rating.ReservationId = int.Parse(csvValues[1]);
                rating.Cleanliness = int.Parse(csvValues[2]);
                rating.RuleCompliance = int.Parse(csvValues[3]);
                rating.AdditionalComment = csvValues[4];

                ratings.Add(rating);
            }

            return ratings;
        }

        public void Save(List<RatingGivenByOwner> ratings)
        {
            StringBuilder csv = new StringBuilder();

            foreach (RatingGivenByOwner rating in ratings)
            {
                string[] csvValues =
                {
                    rating.Id.ToString(),
                    rating.ReservationId.ToString(),
                    rating.Cleanliness.ToString(),
                    rating.RuleCompliance.ToString(),
                    rating.AdditionalComment
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(path, csv.ToString());
        }
    }
}
