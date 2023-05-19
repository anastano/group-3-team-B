using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Injector;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace SIMS_HCI_Project.Applications.Services
{
    public class AccommodationReservationService
    {
        private readonly IAccommodationReservationRepository _reservationRepository;

        public AccommodationReservationService()
        {
            _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
        }

        public AccommodationReservation GetById(int id)
        {
            return _reservationRepository.GetById(id);
        }
        public List<AccommodationReservation> GetByOwnerId(int ownerId)
        {
            return _reservationRepository.GetByOwnerId(ownerId);
        }

        public List<AccommodationReservation> GetByAccommodationId(int accommodationId) 
        {
            return _reservationRepository.GetByAccommodationId(accommodationId);
        }

        public List<AccommodationReservation> GetAllReserevedByAccommodationId(int accommodationId)
        {
            return _reservationRepository.GetAllReservedByAccommodationId(accommodationId);
        }

        public List<AccommodationReservation> GetAllByStatusAndGuestId(int id, AccommodationReservationStatus status)
        {
            return _reservationRepository.GetAllByStatusAndGuestId(id, status);
        }
        public List<AccommodationReservation> GetByGuestId(int id)
        {
            return _reservationRepository.GetByGuestId(id);
        }
        public List<AccommodationReservation> GetInProgressByOwnerId(int ownerId)
        {
            List<AccommodationReservation> reservationsInProgress = new List<AccommodationReservation>();

            foreach (AccommodationReservation reservation in GetByOwnerId(ownerId))
            {
                if ((new DateRange(reservation.Start, reservation.End)).IsInProgress())
                {
                    reservationsInProgress.Add(reservation);
                }
            }
            return reservationsInProgress;
        } 

        public void Add(AccommodationReservation reservation)
        {
            _reservationRepository.Add(reservation);
        }
        public void EditStatus(int reservationId, AccommodationReservationStatus status)
        {
            _reservationRepository.EditStatus(reservationId, status);
        }
        public void EditReservation(RescheduleRequest request)
        {
            _reservationRepository.EditReservation(request);
        }
        public List<AccommodationReservation> OwnerSearch(string accommodationName, string guestName, string guestSurname, int ownerId)
        {
            return _reservationRepository.OwnerSearch(accommodationName, guestName, guestSurname, ownerId);
        }
        public void ConvertReservedReservationIntoCompleted(DateTime currentDate)
        {
            _reservationRepository.ConvertReservedReservationIntoCompleted(currentDate);
        }
        public void ConvertReservationsIntoRated(RatingGivenByGuestService ratingGivenByGuestService)
        {
            foreach (AccommodationReservation reservation in _reservationRepository.GetAll())
            {
                reservation.isRated = ratingGivenByGuestService.IsReservationRated(reservation.Id);
            }
        }
        public void CancelReservation(NotificationService notificationService, AccommodationReservation reservation)
        {
            String Message = "Reservation for " + reservation.Accommodation.Name + " with id: " + reservation.Id + " has been cancelled";
            notificationService.Add(new Notification(Message, reservation.Accommodation.OwnerId, false));
            _reservationRepository.EditStatus(reservation.Id, AccommodationReservationStatus.CANCELLED);
        }
        public List<AccommodationReservation> GetAvailableReservations(Accommodation accommodation, Guest1 guest, DateTime start, DateTime end, int daysNumber, int guestsNumber)
        {
            List<AccommodationReservation> availableReservations = new List<AccommodationReservation>();
            DateTime potentialStart = start;
            DateTime potentialEnd = start.AddDays(daysNumber - 1);
            DateRange potentialDateRange = new DateRange(potentialStart, potentialEnd);
            while (potentialEnd <= end)
            {
                 if (!OverlapsWithRenovations(accommodation, potentialDateRange) && !OverlapsWithReservations(accommodation, potentialDateRange))
                 {
                     availableReservations.Add(new AccommodationReservation(accommodation, guest, potentialStart, potentialEnd, guestsNumber));
                 }

                 potentialStart = potentialStart.AddDays(1);
                 potentialEnd = potentialStart.AddDays(daysNumber - 1);
                 potentialDateRange = new DateRange(potentialStart, potentialEnd);
            }
            return availableReservations;
        }
        public bool OverlapsWithRenovations(Accommodation accommodation, DateRange potentialDateRange)
        {
            RenovationService renovationService = new RenovationService();
            foreach (Renovation renovation in renovationService.GetByAccommodationId(accommodation.Id))
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
            foreach (AccommodationReservation reservation in GetAllReserevedByAccommodationId(accommodation.Id))
            {
                if (potentialDateRange.DoesOverlap(new DateRange(reservation.Start, reservation.End)))
                {
                    return true;
                }
            }
            return false;
        }
        public List<AccommodationReservation> GetSuggestedAvailableReservations(Accommodation accommodation, Guest1 guest, DateTime start, DateTime end, int daysNumber, int guestsNumber)
        {
            List<AccommodationReservation> accommodationReservations = GetAllReserevedByAccommodationId(accommodation.Id);
            return GetAvailableReservations(accommodation, guest,  end, end.AddDays(30), daysNumber, guestsNumber);
        }
    }
}
