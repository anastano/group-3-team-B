using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repository
{
    public class LocationRepository
    {
        private const string FilePath = "../../../Resources/Database/locations.csv";

        private readonly Serializer<Location> _serializer;

        private List<Location> _locations;

        public LocationRepository()
        {
            _serializer = new Serializer<Location>();
            _locations = _serializer.FromCSV(FilePath);
        }

        public List<Location> GetAll()
        {
            return _locations;
        }

        /// <summary>
        /// Saves location and returns location ID so it can be connected to other objects easily
        /// </summary>
        public int Save(Location location)
        {
            Location foundLocation = FindByCountryAndCity(location.Country, location.City);
            if (foundLocation == null)
            {
                location.Id = GenerateNextId();
                _locations.Add(location);
                _serializer.ToCSV(FilePath, _locations);
                return location.Id;
            }
            else
            {
                return foundLocation.Id;
            }
        }

        public Location FindByCountryAndCity(string country, string city)
        {
            return _locations.FirstOrDefault(l => l.City == city && l.Country == country);
        }

        private int GenerateNextId()
        {
            if (_locations.Count == 0) return 1;
            return _locations[_locations.Count - 1].Id + 1;
        }

    }
}
