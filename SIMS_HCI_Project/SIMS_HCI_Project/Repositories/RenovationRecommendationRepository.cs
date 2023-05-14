using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class RenovationRecommendationRepository : IRenovationRecommendationRepository
    {
        private RenovationRecommendationFileHandler _fileHandler;
        private List<RenovationRecommendation> _recommendations;

        public RenovationRecommendationRepository()
        {
            _fileHandler = new RenovationRecommendationFileHandler();
            _recommendations = _fileHandler.Load();
        }

        public int GenerateId()
        {
            return _recommendations.Count == 0 ? 1 : _recommendations[_recommendations.Count - 1].Id + 1;
        }

        public void Save()
        {
            _fileHandler.Save(_recommendations);
        }

        public RenovationRecommendation GetById(int id)
        {
            return _recommendations.Find(l => l.Id == id);
        }

        public List<RenovationRecommendation> GetAll()
        {
            return _recommendations;
        }
        public void Add(RenovationRecommendation recommendation)
        {
            recommendation.Id = GenerateId();
            _recommendations.Add(recommendation);
            Save();
        }
    }
}
