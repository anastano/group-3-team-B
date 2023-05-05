using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class RenovationRecommendationFileHandler
    {
        private const string FilePath = "../../../Resources/Database/renovationRecommendation.csv";

        private readonly Serializer<RenovationRecommendation> _serializer;

        public RenovationRecommendationFileHandler()
        {
            _serializer = new Serializer<RenovationRecommendation>();
        }

        public List<RenovationRecommendation> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<RenovationRecommendation> recommendations)
        {
            _serializer.ToCSV(FilePath, recommendations);
        }
    }
}
