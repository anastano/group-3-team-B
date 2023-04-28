using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IAccommodationReservationRepository
    {
        List<AccommodationReservation> GetAll();
        AccommodationReservation GetById(int id);
        List<AccommodationReservation> GetByOwnerId(int id);
        List<AccommodationReservation> GetByGuestId(int id);
        List<AccommodationReservation> GetByAccommodationId(int accommodationId);
        List<AccommodationReservation> GetAllByStatusAndGuestId(int id, AccommodationReservationStatus status);
        void EditStatus(int id, AccommodationReservationStatus status);
        void EditReservation(RescheduleRequest request);
        void ConvertReservedReservationIntoCompleted(DateTime currentDate);
        void Save();
        void NotifyObservers();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        List<AccommodationReservation> OwnerSearch(string accommodationName, string guestName, string guestSurname, int ownerId);
    }
}