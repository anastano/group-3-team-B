using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourReservationRepository
    {
        void Load();
        void Save();
        List<TourReservation> CancelReservationsByTour(int tourTimeId);
        List<TourReservation> GetAllByTourTimeId(int id);
        TourReservation GetByGuestAndTour(int guestId, int tourTimeId);
    }
}