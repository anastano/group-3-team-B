using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourTimeRepository
    {
        TourTime GetById(int id);
        List<TourTime> GetAll();
        List<TourTime> GetAllByGuideId(int guideId);
        List<TourTime> GetAllInDateRange(int guideId, DateRange dateRange);

        void Add(TourTime tourTime);
        void AddBulk(List<TourTime> tourTimes);
        void Update(TourTime tourTime);
    }
}