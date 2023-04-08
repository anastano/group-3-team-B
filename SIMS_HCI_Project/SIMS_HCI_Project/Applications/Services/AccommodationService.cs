using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Repositories;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Controller;

namespace SIMS_HCI_Project.Applications.Services
{
    public class AccommodationService
    {
        private readonly IAccommodationRepository _accommodationRepository;

        public AccommodationService()
        {
            _accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
        }

        public Accommodation FindById(int id)
        {
            return _accommodationRepository.FindById(id);
        }

        public List<Accommodation> GetAll()
        {
            return _accommodationRepository.GetAll();
        }

        public List<string> GetImages(int id)
        {
            return _accommodationRepository.GetImages(id);
        }

        public void Load()
        {
           _accommodationRepository.Load();
        }

        public void Save()
        {
            _accommodationRepository.Save();
        }

        public void ConnectAccommodationsWithLocations(LocationService locationService)
        {
            foreach (Accommodation accommodation in _accommodationRepository.GetAll())
            {
                accommodation.Location = locationService.FindById(accommodation.LocationId);
            }
        }

        public void FillOwnerAccommodationList(int ownerId, OwnerService ownerService)
        {
            // check if its better to create FindByOwnerId in AccommodationRepository
            ownerService.FindById(ownerId).Accommodations = _accommodationRepository.GetAll().FindAll(a => a.OwnerId == ownerId);
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
