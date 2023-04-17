using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourTimeRepository
    {
        TourTime GetById(int id);
        List<TourTime> GetAll();
        List<TourTime> GetAllByGuideId(int id);
        void CancelTour(TourTime tourTime);
        void CheckAndUpdateStatus();
    }
}