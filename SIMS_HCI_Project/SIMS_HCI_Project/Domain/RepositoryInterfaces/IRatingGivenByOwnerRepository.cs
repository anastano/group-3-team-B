using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IRatingGivenByOwnerRepository
    {
        void Add(RatingGivenByOwner rating);
        List<RatingGivenByOwner> GetAll();
        RatingGivenByOwner GetById(int id);
        RatingGivenByOwner GetByReservationId(int reservationId);
        List<RatingGivenByOwner> GetByGuestId(int ownerId);
        void NotifyObservers();
        void Save();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}