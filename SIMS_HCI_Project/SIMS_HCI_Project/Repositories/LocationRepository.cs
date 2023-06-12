using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private LocationFileHandler _fileHandler;
        private List<Location> _locations;

        public LocationRepository()
        {
            _fileHandler = new LocationFileHandler();
            Load();
        }

        private void Load()
        {
            _locations = _fileHandler.Load();
        }

        private void Save()
        {
            _fileHandler.Save(_locations);
        }

        private int GenerateId()
        {
            return _locations.Count == 0 ? 1 : _locations[_locations.Count - 1].Id + 1;
        }

        public Location GetById(int id)
        {
            return _locations.Find(l => l.Id == id);
        }

        public List<Location> GetAll()
        {
            return _locations;
        }

        public Location GetByCountryAndCity(string country, string city)
        {
            return _locations.FirstOrDefault(l => l.City.ToLower() == city.ToLower() && l.Country.ToLower() == country.ToLower());
        }

        public Location GetOrAdd(Location location)
        {
            Location foundLocation = GetByCountryAndCity(location.Country, location.City);
            if (foundLocation == null)
            {
                location.Id = GenerateId();
                _locations.Add(location);
                Save();
                return location;
            }
            else
            {
                return foundLocation;
            }
        }
        public List<String> GetAllCountries()
        {
            return _locations.Select(location => location.Country).Distinct().ToList();
        }
        public List<String> GetAllCities()
        {
            return _locations.Select(location => location.City).Distinct().ToList();
        }
        public List<String> GetCitiesByCountry(String country)
        {
            return _locations.FindAll(l => l.Country == country).Select(l => l.City ).ToList();
        }
        public Location GetLocation(String country, String city)
        {
            return _locations.Find(l => (l.Country == country && l.City == city));
        }
    }
}
