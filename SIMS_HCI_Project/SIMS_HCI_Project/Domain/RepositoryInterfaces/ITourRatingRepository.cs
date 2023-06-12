using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;


namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourRatingRepository
    {
        TourRating GetById(int id);
        List<TourRating> GetAll();
        List<TourRating> GetAllByTourId(int tourTimeId);
        List<TourRating> GetLastYearAllByLanguageAndGuide(int guideId, string language);

        void Add(TourRating rating);
        void Update(TourRating tourRating);

        bool IsRated(int id);
    }
}
