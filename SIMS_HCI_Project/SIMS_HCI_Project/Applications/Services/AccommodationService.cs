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

        public List<string> GetImages(int id)
        {
            return _accommodationRepository.GetImages(id);
        }

        public void Delete(Accommodation accommodation, Owner owner)
        {
            _accommodationRepository.Delete(accommodation, owner);
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
        public void FillOwnerAccommodationList(Owner owner)
        {
            owner.Accommodations = GetByOwnerId(owner.Id);
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
