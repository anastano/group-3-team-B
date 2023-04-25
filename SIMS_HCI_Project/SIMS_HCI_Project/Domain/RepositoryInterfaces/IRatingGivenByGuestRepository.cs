using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface IRatingGivenByGuestRepository
    {
        RatingGivenByGuest GetById(int id);
        RatingGivenByGuest GetByReservationId(int reservationId);
        List<RatingGivenByGuest> GetAll();
        void Add(RatingGivenByGuest rating);
        bool isReservationRated(int reservationId);
        void NotifyObservers();
        void Save();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        List<RatingGivenByGuest> GetByOwnerId(int ownerId);
    }
}
