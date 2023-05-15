﻿using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class RenovationService
    {
        private readonly IRenovationRepository _renovationRepository;

        public RenovationService()
        {
            _renovationRepository = Injector.Injector.CreateInstance<IRenovationRepository>();
        }

        public Renovation GetById(int id)
        {
            return _renovationRepository.GetById(id);
        }

        public List<Renovation> GetAll()
        {
            return _renovationRepository.GetAll();
        }

        public List<Renovation> GetByOwnerId(int ownerId)
        { 
            return _renovationRepository.GetByOwnerId(ownerId);
        }

        public List<Renovation> GetByAccommodationId(int accommodationId)
        {
            return _renovationRepository.GetByAccommodationId(accommodationId);
        }

        public List<Renovation> GetAvailableRenovations(Accommodation accommodation, DateTime enteredStart, DateTime enteredEnd, int daysNumber)
        {
            List<Renovation> availableRenovations = new List<Renovation>();
            DateTime potentialStart = enteredStart;
            DateTime endDate = enteredEnd;

            while (potentialStart <= endDate)
            {
                DateTime potentialEnd = potentialStart.AddDays(daysNumber - 1);

                if (potentialEnd > endDate) return availableRenovations;
                bool dateRangeOverlaps = false;

                foreach (Renovation renovation in GetByAccommodationId(accommodation.Id))
                {
                    if (IsDateRangeOverlapping(potentialStart, potentialEnd, renovation))
                    {
                        dateRangeOverlaps = true;
                        break;
                    }
                }

                if (!dateRangeOverlaps)
                {
                    availableRenovations.Add(new Renovation(accommodation.Id, potentialStart, potentialEnd));
                }
                potentialStart = potentialStart.AddDays(1);

            }
            return availableRenovations;
        }

        public bool IsDateRangeOverlapping(DateTime potentialStart, DateTime potentialEnd, Renovation renovation)
        {
            bool isPotentialStartOverlap = potentialStart >= renovation.Start && potentialStart <= renovation.End;
            bool isPotentialEndOverlap = potentialEnd >= renovation.Start && potentialEnd <= renovation.End;
            bool isPotentialRangeOverlap = potentialStart <= renovation.Start && potentialEnd >= renovation.End;
            return isPotentialStartOverlap || isPotentialEndOverlap || isPotentialRangeOverlap;
        }

        public void Add(Renovation renovation)
        {
            _renovationRepository.Add(renovation);
        }

        public bool CancelRenovation(Renovation renovation)
        {
            if (DateTime.Today.AddDays(5) <= renovation.Start)
            {
                _renovationRepository.Delete(renovation);
                return true;
            }

            return false;
        }

        public void ConnectRenovationsWithAccommodations(AccommodationService accommodationService)
        {
            foreach (Renovation renovation in GetAll())
            {
                renovation.Accommodation = accommodationService.GetById(renovation.AccommodationId);
            }
        }

        public void NotifyObservers()
        {
            _renovationRepository.NotifyObservers();
        }

        public void Subscribe(IObserver observer)
        {
            _renovationRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _renovationRepository.Unsubscribe(observer);
        }
    }
}