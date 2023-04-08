using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface ILocationRepository
    {
        Location FindByCountryAndCity(string country, string city);
        Location FindById(int id);
        Location FindOrAdd(Location location);
        int GenerateId();
        List<Location> GetAll();
        void Load();
        void Save();
    }
}