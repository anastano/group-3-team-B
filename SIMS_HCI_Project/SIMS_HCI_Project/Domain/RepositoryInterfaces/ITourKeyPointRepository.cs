using System.Collections.Generic;
using SIMS_HCI_Project.Domain.Models;


namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourKeyPointRepository
    {
        void Load();
        void Save();
        List<TourKeyPoint> GetAll();
        TourKeyPoint FindById(int id);
        List<TourKeyPoint> FindByIds(List<int> ids);
    }
}
