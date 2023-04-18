using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IAccommodationReservationRepository
    {
        AccommodationReservation GetById(int id);
        int GenerateId();
        List<AccommodationReservation> GetAll();
        void NotifyObservers();
        void Save();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        List<AccommodationReservation> GetByOwnerId(int id);
        List<AccommodationReservation> GetByAccommodationId(int accommodationId);
        List<AccommodationReservation> GetAllByStatusAndGuestId(int id, AccommodationReservationStatus status);
        void EditStatus(int id, AccommodationReservationStatus status); // Be update #New
        void ConvertReservedAccommodationsIntoCompleted(DateTime currentDate);
        void EditReservation(RescheduleRequest request);
    }
}