using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public interface IRenovationRecommendationRepository
    {
        RenovationRecommendation GetById(int id);
        List<RenovationRecommendation> GetAll();
        void Add(RenovationRecommendation recommendation);
        List<RenovationRecommendation> GetByAccommodationId(int accommodationId);
        int GetRecommendationCountByYearAndAccommodationId(int year, int accommodationId);
        int GetRecommendationCountByMonthAndAccommodationId(int monthIndex, int year, int accommodationId);
    }
}
