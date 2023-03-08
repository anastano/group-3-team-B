using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
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

        public void Add(Location location)
        {
            _locations.Add(location);
            //NotifyObservers();
            _fileHandler.Save(_locations);
        }


        public List<Location> GetAll()
        {
            return _locations;
        }

        public Location Save(Location location)
        {
            Location foundLocation = FindByCountryAndCity(location.Country, location.City);
            if (foundLocation == null)
            {
                location.Id = GenerateNextId();
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

        public int GenerateNextId()
        {
            if (_locations.Count == 0) return 1;
            return _locations[_locations.Count - 1].Id + 1;
        }

        public bool LocationExsists(string country, string city)
        {
            foreach(Location location in _locations)
            {
                if(location.Country == country && location.City==city)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
