using SIMS_HCI_Project.Applications.Services;
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
        TourReservation GetById(int id);
        List<TourReservation> CancelReservationsByTour(int tourTimeId);
        List<TourReservation> GetAllByTourTimeId(int id);
        List<TourReservation> GetAllByGuestId(int id);
        List<TourReservation> GetActiveByGuestId(int id);
        List<TourReservation> GetAll();
        int GenerateId();
        void Add(TourReservation tourReservation);
        void NotifyObservers();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        List<TourReservation> GetUnratedReservations(int guestId, GuestTourAttendanceService guestTourAttendanceService, TourRatingService tourRatingService, TourTimeService tourTimeService);
        TourReservation GetByGuestAndTour(int guestId, int tourTimeId);
        void CancelReservation(int reservationId);
    }
}