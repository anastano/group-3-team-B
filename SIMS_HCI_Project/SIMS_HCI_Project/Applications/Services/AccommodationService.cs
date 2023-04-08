﻿using SIMS_HCI_Project.Domain.Models;
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

        public void Save()
        {
            _accommodationRepository.Save();
        }

        public Accommodation GetById(int id)
        {
            return _accommodationRepository.GetById(id);
        }

        public List<Accommodation> GetAll()
        {
            return _accommodationRepository.GetAll();
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
            foreach (Accommodation accommodation in _accommodationRepository.GetAll())
            {
                accommodation.Location = locationService.GetById(accommodation.LocationId);
            }
        }

        public void FillOwnerAccommodationList(Owner owner)
        {
            owner.Accommodations = _accommodationRepository.GetByOwnerId(owner.Id);
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
