using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IRenovationRepository
    {
        List<Renovation> GetAll();
        Renovation GetById(int id);
        List<Renovation> GetByOwnerId(int ownerId);
        void NotifyObservers();
        void Save();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}