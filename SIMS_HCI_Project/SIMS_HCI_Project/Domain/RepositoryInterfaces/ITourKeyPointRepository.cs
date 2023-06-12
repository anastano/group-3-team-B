using System.Collections.Generic;
using SIMS_HCI_Project.Domain.Models;


namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourKeyPointRepository
    {
        List<TourKeyPoint> GetAll();
        TourKeyPoint GetById(int id);
        List<TourKeyPoint> GetByIds(List<int> ids);

        void Add(TourKeyPoint tourKeyPoint);
        void AddBulk(List<TourKeyPoint> tourKeyPoints);
    }
}
