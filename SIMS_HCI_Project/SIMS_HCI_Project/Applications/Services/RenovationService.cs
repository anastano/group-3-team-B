using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
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
            DateTime potentialEnd = enteredStart.AddDays(daysNumber - 1);
            DateRange potentialDateRange = new DateRange(potentialStart, potentialEnd);

            while (potentialEnd <= enteredEnd)
            {
                if (!OverlapsWithRenovations(accommodation, potentialDateRange) && !OverlapsWithReservations(accommodation, potentialDateRange))
                {
                    availableRenovations.Add(new Renovation(accommodation.Id, potentialStart, potentialEnd));
                }

                potentialStart = potentialStart.AddDays(1);
                potentialEnd = potentialStart.AddDays(daysNumber - 1);
                potentialDateRange = new DateRange(potentialStart, potentialEnd);
            }
            return availableRenovations;
        }

        public bool OverlapsWithRenovations(Accommodation accommodation, DateRange potentialDateRange)
        {
            foreach (Renovation renovation in GetByAccommodationId(accommodation.Id))
            {
                if (potentialDateRange.DoesOverlap(new DateRange(renovation.Start, renovation.End)))
                {
                    return true;
                }
            }
            return false;
        }

        public bool OverlapsWithReservations(Accommodation accommodation, DateRange potentialDateRange)
        {
            AccommodationReservationService reservationService = new AccommodationReservationService();

            foreach (AccommodationReservation reservation in reservationService.GetAllReserevedByAccommodationId(accommodation.Id))
            {
                if (potentialDateRange.DoesOverlap(new DateRange(reservation.Start, reservation.End)))
                {
                    return true;
                }
            }
            return false;
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
        public bool IsAccommodationRenovated(int accommodationId)
        {
            foreach(Renovation renovation in GetByAccommodationId(accommodationId))
            {
                if(renovation.End.AddYears(1) >= DateTime.Today)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
