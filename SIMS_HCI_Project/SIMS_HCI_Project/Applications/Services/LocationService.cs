using SIMS_HCI_Project.Domain.Models;
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

        public Location GetOrAdd(Location location)
        {
            return _locationRepository.GetOrAdd(location);
        }

        public List<Location> GetAll()
        {
            return _locationRepository.GetAll();
        }

    }
}
