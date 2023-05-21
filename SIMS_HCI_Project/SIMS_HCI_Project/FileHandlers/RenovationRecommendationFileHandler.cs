using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SIMS_HCI_Project.FileHandlers
{
    public class RenovationRecommendationFileHandler
    {
        private const string FilePath = "../../../Resources/Database/renovationRecommendation.csv";
        private const char Delimiter = '|';

        public RenovationRecommendationFileHandler()
        {

        }

        public List<RenovationRecommendation> Load()
        {
            List<RenovationRecommendation> recommendations = new List<RenovationRecommendation>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                RenovationRecommendation recommendation = new RenovationRecommendation();

                recommendation.Id = int.Parse(csvValues[0]);
                recommendation.RatingId = int.Parse(csvValues[1]);
                Enum.TryParse(csvValues[2], out UrgencyRenovationLevel level);
                recommendation.UrgencyLevel = level;
                recommendation.Comment = csvValues[3];

                recommendations.Add(recommendation);
            }

            return recommendations;
        }

        public void Save(List<RenovationRecommendation> recommendations)
        {
            StringBuilder csv = new StringBuilder();

            foreach (RenovationRecommendation recommendation in recommendations)
            {
                string[] csvValues =
                {
                    recommendation.Id.ToString(),
                    recommendation.RatingId.ToString(),
                    recommendation.UrgencyLevel.ToString(),
                    recommendation.Comment
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}
