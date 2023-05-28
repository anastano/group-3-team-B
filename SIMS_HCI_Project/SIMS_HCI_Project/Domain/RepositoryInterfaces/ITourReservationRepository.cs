using SIMS_HCI_Project.Domain.Models;

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
        List<TourReservation> GetActiveByGuestId(int id);
        List<TourReservation> GetAllByGuestIdAndTourId(int guestId, int tourId);

        void Add(TourReservation tourReservation);
        void BulkUpdate(List<TourReservation> tourReservations);
    }
}