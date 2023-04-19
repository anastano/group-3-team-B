using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<AccommodationReservation> GetAll()
        {
            return _reservationRepository.GetAll();
        }

        public List<AccommodationReservation> GetByOwnerId(int ownerId)
        {
            return _reservationRepository.GetByOwnerId(ownerId);
        }

        public List<AccommodationReservation> GetByAccommodationId(int accommodationId)
        {
            return _reservationRepository.GetByAccommodationId(accommodationId);
        }

        public List<AccommodationReservation> GetAllByStatusAndGuestId(int id, AccommodationReservationStatus status)
        {
            return _reservationRepository.GetAllByStatusAndGuestId(id, status);
        }

        public List<AccommodationReservation> GetInProgressByOwnerId(int ownerId)
        {
            List<AccommodationReservation> reservationsInProgress = new List<AccommodationReservation>();

            foreach (AccommodationReservation reservation in GetByOwnerId(ownerId))
            {
                if (IsInProgress(reservation) && IsReservedOrRescheduled(reservation))
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

        public bool IsReservedOrRescheduled(AccommodationReservation reservation)
        {
            return reservation.Status == AccommodationReservationStatus.RESERVED || reservation.Status == AccommodationReservationStatus.RESCHEDULED;
        }

        public bool IsCompleted(AccommodationReservation reservation)
        {
            return reservation.Status == AccommodationReservationStatus.COMPLETED;
        }

        public bool IsWithinFiveDaysAfterCheckout(AccommodationReservation reservation)
        {
            return DateTime.Today <= reservation.End.AddDays(5);
        }

        public void EditStatus(int reservationId, AccommodationReservationStatus status)
        {
            _reservationRepository.EditStatus(reservationId, status);
        }

        public void EditReservation(RescheduleRequest request)
        {
            _reservationRepository.EditReservation(request);
        }

        public void ConnectReservationsWithAccommodations(AccommodationService accommodationService)
        {
            foreach (AccommodationReservation reservation in GetAll())
            {
                reservation.Accommodation = accommodationService.GetById(reservation.AccommodationId);
            }
        }

        public void ConnectReservationsWithGuests(Guest1Service guest1Service)
        {
            foreach (AccommodationReservation reservation in GetAll())
            {
                reservation.Guest = guest1Service.GetById(reservation.GuestId);
            }
        }
        public void FillOwnerReservationList(Owner owner)
        {
            owner.Reservations = GetByOwnerId(owner.Id);
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
