using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Controller
{
    public class LocationController
    {
        private LocationFileHandler _fileHandler;

        private List<Location> _locations;

        public LocationController()
        {
            _fileHandler = new LocationFileHandler();
            _locations = _fileHandler.Load();
        }

        public List<Location> GetAll()
        {
            return _locations;
        }

        public Location Save(Location location) //checks if location exists and saves it
        {
            Location foundLocation = FindByCountryAndCity(location.Country, location.City);
            if (foundLocation == null)
            {
                location.Id = GenerateId();
                _locations.Add(location);
                _fileHandler.Save(_locations);
                return location;
            }
            else
            {
                return foundLocation;
            }
        }

        public Location FindByCountryAndCity(string country, string city)
        {
            return _locations.FirstOrDefault(l => l.City.ToLower() == city.ToLower() && l.Country.ToLower() == country.ToLower());
        }


        public Location FindById(int id)
        {
            return _locations.Find(l => l.Id == id);
        }

        public int GenerateId()
        {
            if (_locations.Count == 0) return 1;
            return _locations[_locations.Count - 1].Id + 1;
        }

    }
}
