using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourRepository
    {
        Tour GetById(int id);
        List<Tour> GetAll();
        List<Tour> GetAllByGuide(int guideId);
        void Add(Tour tour);
    }
}