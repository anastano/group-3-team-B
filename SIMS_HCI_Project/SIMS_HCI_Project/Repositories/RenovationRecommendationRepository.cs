using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;

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
            return _recommendations.Find(r => r.Id == id);
        }

        public List<RenovationRecommendation> GetByOwnerId(int ownerId)
        {
            return _recommendations.FindAll(r => r.Rating.Reservation.Accommodation.OwnerId == ownerId);
        }

        public List<RenovationRecommendation> GetByAccommodationId(int accommodationId)
        {
            return _recommendations.FindAll(r => r.Rating.Reservation.Accommodation.Id == accommodationId);
        }

        public List<RenovationRecommendation> GetAll()
        {
            return _recommendations;
        }

        public int GetRecommendationCountByYearAndAccommodationId(int year, int accommodationId) 
        {
            return GetByAccommodationId(accommodationId).FindAll(r => r.Rating.Reservation.Start.Year == year).Count();
        }

        public int GetRecommendationCountByMonthAndAccommodationId(int monthIndex, int year, int accommodationId) 
        {
            return GetByAccommodationId(accommodationId).FindAll(r => r.Rating.Reservation.Start.Year == year
                        && r.Rating.Reservation.Start.Month == monthIndex).Count();
        }

        public void Add(RenovationRecommendation recommendation)
        {
            recommendation.Id = GenerateId();
            _recommendations.Add(recommendation);
            Save();
        }

        public List<RenovationRecommendation> OwnerSearch(string accommodationName, int ownerId)
        {
            List<RenovationRecommendation> recommendations = GetByOwnerId(ownerId);

            var filtered = from _recommendation in recommendations
                           where (string.IsNullOrEmpty(accommodationName) || _recommendation.Rating.Reservation.Accommodation.Name.ToLower().Contains(accommodationName.ToLower()))
                           select _recommendation;

            return filtered.ToList();
        }
    }
}
