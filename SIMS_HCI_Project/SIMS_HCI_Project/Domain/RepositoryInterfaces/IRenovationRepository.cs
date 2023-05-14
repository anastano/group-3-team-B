using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IRenovationRepository
    {
        void Add(Renovation renovation);
        void Delete(Renovation renovation);
        List<Renovation> GetAll();
        List<Renovation> GetByAccommodationId(int accommodationId);
        Renovation GetById(int id);
        List<Renovation> GetByOwnerId(int ownerId);
        void NotifyObservers();
        void Save();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}