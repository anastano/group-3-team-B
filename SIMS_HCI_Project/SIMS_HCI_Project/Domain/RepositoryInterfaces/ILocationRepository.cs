using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface ILocationRepository
    {
        Location GetByCountryAndCity(string country, string city);
        Location GetById(int id);
        Location GetOrAdd(Location location);
        List<Location> GetAll();
        void Save();
    }
}