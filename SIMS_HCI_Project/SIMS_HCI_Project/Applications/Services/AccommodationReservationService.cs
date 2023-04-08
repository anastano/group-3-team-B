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

        public void Save()
        {
            _reservationRepository.Save();
        }

        public AccommodationReservation GetById(int id)
        {
            return _reservationRepository.GetById(id);
        }

        public List<AccommodationReservation> GetAll()
        {
            return _reservationRepository.GetAll();
        }

        public List<AccommodationReservation> GetInProgressByOwnerId(int ownerId)
        {
            List<AccommodationReservation> reservationsInProgress = new List<AccommodationReservation>();

            foreach (AccommodationReservation reservation in _reservationRepository.GetByOwnerId(ownerId))
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

        public bool IsReservedOrRescheduled(AccommodationReservation reservation)
        {
            return reservation.Status == AccommodationReservationStatus.RESERVED || reservation.Status == AccommodationReservationStatus.RESCHEDULED;
        }

        public void ConnectReservationsWithAccommodations(AccommodationService accommodationService)
        {
            foreach (AccommodationReservation reservation in _reservationRepository.GetAll())
            {
                reservation.Accommodation = accommodationService.GetById(reservation.AccommodationId);
            }
        }

        public void FillOwnerReservationList(Owner owner)
        {
            owner.Reservations = _reservationRepository.GetByOwnerId(owner.Id);
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
