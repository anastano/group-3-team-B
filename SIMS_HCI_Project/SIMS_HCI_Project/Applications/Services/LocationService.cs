using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class LocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService()
        {
            _locationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
        }
        public Location GetById(int id)
        {
            return _locationRepository.GetById(id);
        }
        public List<String> GetAllCountries()
        {
            return _locationRepository.GetAllCountries();
        }
        public List<String> GetAllCities()
        {
            return _locationRepository.GetAllCities();
        }
        public List<String> GetCitiesByCountry(String country)
        {
            return _locationRepository.GetCitiesByCountry(country);
        }
        public Location GetLocation(String country, String city)
        {
            return _locationRepository.GetLocation(country, city);
        }
    }
}
