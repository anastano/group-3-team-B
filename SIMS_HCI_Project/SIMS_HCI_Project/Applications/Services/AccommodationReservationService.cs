using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Injector;
using SIMS_HCI_Project.Observer;
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
        private readonly IUserRepository _userRepository;

        public AccommodationReservationService()
        {
            _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            _userRepository = Injector.Injector.CreateInstance<IUserRepository>();
        }

        public AccommodationReservation GetById(int id)
        {
            return _reservationRepository.GetById(id);
        }

        public List<AccommodationReservation> GetAll()
        {
            return _reservationRepository.GetAll();
        }

        public List<AccommodationReservation> GetByOwnerId(int ownerId)
        {
            return _reservationRepository.GetByOwnerId(ownerId);
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
                if (IsInProgress(reservation) && IsReserved(reservation))
                {
                    reservationsInProgress.Add(reservation);
                }
            }
            return reservationsInProgress;
        }
        
        public bool IsInProgress(AccommodationReservation reservation)
        {
            return DateTime.Today >= reservation.Start && DateTime.Today <= reservation.End;
        }
        public bool IsReservationActive(AccommodationReservation reservation)
        {
            return reservation.End >= DateTime.Today;
        }
        public bool IsReserved(AccommodationReservation reservation)
        {
            return reservation.Status == AccommodationReservationStatus.RESERVED;
        }
        public bool IsCompleted(AccommodationReservation reservation)
        {
            return reservation.Status == AccommodationReservationStatus.COMPLETED;
        }

        public bool IsWithinFiveDaysAfterCheckout(AccommodationReservation reservation)
        {
            return DateTime.Today <= reservation.End.AddDays(5);
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
        ///////////  provjeriti da li je sve na dobro mjestu jer je ovo poslovna logika
        /////////////////////////////////////////////////////////
        public List<AccommodationReservation> GetAvailableReservations(Accommodation accommodation, Guest1 guest, DateTime start, DateTime end, int daysNumber, int guestsNumber)
        {
            List<AccommodationReservation> availableReservations = new List<AccommodationReservation>();
            DateTime potentialStart = start;
            DateTime endDate = end;
            while (potentialStart <= endDate)
            {
                DateTime potentialEnd = potentialStart.AddDays(daysNumber - 1);

                if (potentialEnd > endDate) return availableReservations;
                bool dateRangeOverlaps = false;

                foreach (AccommodationReservation reservation in GetAllReserevedByAccommodationId(accommodation.Id))
                {
                    if (IsDateRangeOverlapping(potentialStart, potentialEnd, reservation))
                    {
                        dateRangeOverlaps = true;
                        break;
                    }
                }
                if (!dateRangeOverlaps)
                {
                    availableReservations.Add(new AccommodationReservation(accommodation, guest, potentialStart, potentialEnd, guestsNumber));
                }
                potentialStart = potentialStart.AddDays(1);

            }
            return availableReservations;
        }
        public bool IsDateRangeOverlapping(DateTime potentialStart, DateTime potentialEnd, AccommodationReservation reservation)
        {
            bool isPotentialStartOverlap = potentialStart >= reservation.Start && potentialStart <= reservation.End;
            bool isPotentialEndOverlap = potentialEnd >= reservation.Start && potentialEnd <= reservation.End;
            bool isPotentialRangeOverlap = potentialStart <= reservation.Start && potentialEnd >= reservation.End;
            return isPotentialStartOverlap || isPotentialEndOverlap || isPotentialRangeOverlap;
        }
        public bool CheckIfSuggestionIsNeeded(List<AccommodationReservation> availableReservations)
        {
            if (availableReservations.Count == 0) return true;
            return false;
        }
        public List<AccommodationReservation> GetSuggestedAvailableReservations(Accommodation accommodation, Guest1 guest, DateTime start, DateTime end, int daysNumber, int guestsNumber)
        {
            List<AccommodationReservation> accommodationReservations = GetAllReserevedByAccommodationId(accommodation.Id);
            //uzima krajnji datum i razliku dateRange i dodaje jos 15 dana
            TimeSpan timeDifference = end - start;
            double totalDays = timeDifference.TotalDays;
            return GetAvailableReservations(accommodation, guest,  end.AddDays(totalDays + 15), end.AddDays(15), daysNumber, guestsNumber);
        }
        public void NotifyObservers()
        {
            _reservationRepository.NotifyObservers();
        }
        public void Subscribe(IObserver observer)
        {
            _reservationRepository.Subscribe(observer);
        }
        public void Unsubscribe(IObserver observer)
        {
            _reservationRepository.Unsubscribe(observer);
        }
    }
}
