using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourReservationRepository
    {
        TourReservation GetById(int id);
        List<TourReservation> GetAll();
        List<TourReservation> GetAllByTourTimeId(int id);
        List<TourReservation> GetAllByGuestId(int id);
        TourReservation GetByGuestAndTour(int guestId, int tourTimeId);
        List<TourReservation> GetActiveByGuestId(int id);
        void Add(TourReservation tourReservation);
        void BulkUpdate(List<TourReservation> tourReservations);
        void NotifyObservers();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}