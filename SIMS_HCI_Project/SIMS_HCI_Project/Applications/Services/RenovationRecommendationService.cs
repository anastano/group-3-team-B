using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SIMS_HCI_Project.Applications.Services
{
    public class RenovationRecommendationService
    {
        private readonly IRenovationRecommendationRepository _recommendationRepository;

        public RenovationRecommendationService()
        {
            _recommendationRepository = Injector.Injector.CreateInstance<IRenovationRecommendationRepository>();
        }

        public RenovationRecommendation GetById(int id)
        {
            return _recommendationRepository.GetById(id);
        }
        public List<RenovationRecommendation> GetByOwnerId(int ownerId)
        {
            return _recommendationRepository.GetByOwnerId(ownerId);
        }

        public List<RenovationRecommendation> GetAll()
        {
            return _recommendationRepository.GetAll();
        }
        public void Add(RenovationRecommendation recommendation, RatingGivenByGuest rating)
        {
            if (recommendation != null)
            {
                recommendation.RatingId = rating.Id;
                recommendation.Rating = rating;
                _recommendationRepository.Add(recommendation);
            }
        }

        public List<RenovationRecommendation> OwnerSearch(string accommodationName, int ownerId)
        {
            return _recommendationRepository.OwnerSearch(accommodationName, ownerId);
        }
    }
}
