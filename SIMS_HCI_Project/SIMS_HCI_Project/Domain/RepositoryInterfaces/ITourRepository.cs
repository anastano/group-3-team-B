using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourRepository
    {
        void Load();
        void Save();
        Tour FindById(int id);
        List<Tour> GetAll();
    }
}