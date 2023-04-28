using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Repositories;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Controller;
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
        public List<Accommodation> GetAllSortedBySuperFlag()
        {
            return _accommodationRepository.GetAllSortedBySuperFlag();
        }
        public List<Accommodation> Search(string name, string country, string city, string type, int guestsNumber, int reservationDays)
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

        public void Delete(Accommodation accommodation)
        {
            _accommodationRepository.Delete(accommodation);
        }

        public void ConnectAccommodationsWithLocations(LocationService locationService)
        {
            foreach (Accommodation accommodation in GetAll())
            {
                accommodation.Location = locationService.GetById(accommodation.LocationId);
            }
        }
        public void ConnectAccommodationsWithOwners(OwnerService ownerService)
        {
            foreach (Accommodation accommodation in GetAll())
            {
                accommodation.Owner = ownerService.GetById(accommodation.OwnerId);
            }
        }

        public void NotifyObservers()
        {
            _accommodationRepository.NotifyObservers();
        }

        public void Subscribe(IObserver observer)
        {
            _accommodationRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _accommodationRepository.Unsubscribe(observer);
        }   

    }
}
