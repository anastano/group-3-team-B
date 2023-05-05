using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<RenovationRecommendation> GetAll()
        {
            return _recommendationRepository.GetAll();
        }
        public void ConnectRecommendationsWithRatings(RatingGivenByGuestService ratingService)
        {
            foreach (RenovationRecommendation recommendation in GetAll())
            {
                recommendation.Rating = ratingService.GetById(recommendation.RatingId);
            }
        }
    }
}
