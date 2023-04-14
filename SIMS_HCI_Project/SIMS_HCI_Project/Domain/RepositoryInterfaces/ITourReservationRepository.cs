using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourReservationRepository
    {
        void Load();
        void Save();
        List<TourReservation> CancelReservationsByTour(int tourTimeId);
        List<TourReservation> GetAllByTourTimeId(int id);
        List<TourReservation> GetAllByGuestId(int id);
        List<TourReservation> GetAll();
        TourReservation FindById(int id);
        int GenerateId();
        void Add(TourReservation tourReservation);
        void NotifyObservers();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);

    }
}