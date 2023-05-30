using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace SIMS_HCI_Project.Applications.Services
{
    public class AccommodationService
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly ILocationRepository _locationRepository;

        public AccommodationService()
        {
            _accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            _locationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
        }

        public Accommodation GetById(int id)
        {
            return _accommodationRepository.GetById(id);
        }

        public List<Accommodation> GetAll()
        {
            return _accommodationRepository.GetAll();
        }
        public List<Accommodation> GetByOwnerId(int ownerId)
        {
            return _accommodationRepository.GetByOwnerId(ownerId);
        }

        public List<Accommodation> GetByLocationIdAndOwnerId(int locationId, int ownerId)
        {
            return _accommodationRepository.GetByLocationIdAndOwnerId(locationId, ownerId);
        }

        public List<Accommodation> GetAllSortedBySuperFlag()
        {
            return _accommodationRepository.GetAllSortedBySuperFlag();
        }
        public List<Accommodation> Search(string name, string country, string city, string type, string guestsNumber, string reservationDays)
        {
            return _accommodationRepository.Search(name, country, city, type, guestsNumber, reservationDays);
        }
        public List<string> GetImages(int id)
        {
            return _accommodationRepository.GetImages(id);
        }

        public void Add(Accommodation accommodation, Location location)
        {
            accommodation.Location = _locationRepository.GetOrAdd(location);
            accommodation.LocationId = accommodation.Location.Id;
            _accommodationRepository.Add(accommodation);
        }
 
        public void ConvertAccommodationIntoRenovated(RenovationService renovationService)
        {
            foreach (Accommodation accommodation in GetAll())
            {
                accommodation.IsRenovated = renovationService.IsAccommodationRenovated(accommodation.Id);
            }
        }

    }
}
