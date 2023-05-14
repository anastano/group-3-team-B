using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    internal class RatingGivenByGuestFileHandler
    {
        private const string path = "../../../Resources/Database/ratingsGivenByGuest.csv";
        private const char Delimiter = '|';

        public RatingGivenByGuestFileHandler()
        {
            
        }
        public List<RatingGivenByGuest> Load()
        {
            List<RatingGivenByGuest> ratings = new List<RatingGivenByGuest>();

            foreach (string line in File.ReadLines(path))
            {
                string[] csvValues = line.Split(Delimiter);
                RatingGivenByGuest rating = new RatingGivenByGuest();

                rating.Id = int.Parse(csvValues[0]);
                rating.ReservationId = int.Parse(csvValues[1]);
                rating.Cleanliness = int.Parse(csvValues[2]);
                rating.Correctness = int.Parse(csvValues[3]);
                rating.AdditionalComment = csvValues[4];
                rating.Images = new List<string>(csvValues[5].Split(","));

                ratings.Add(rating);
            }

            return ratings;
        }

        public void Save(List<RatingGivenByGuest> ratings)
        {
            StringBuilder csv = new StringBuilder();

            foreach (RatingGivenByGuest rating in ratings)
            {
                string[] csvValues =
                {
                    rating.Id.ToString(),
                    rating.ReservationId.ToString(),
                    rating.Cleanliness.ToString(),
                    rating.Correctness.ToString(),
                    rating.AdditionalComment,
                    string.Join(",", rating.Images)
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(path, csv.ToString());
        }
    }
}
