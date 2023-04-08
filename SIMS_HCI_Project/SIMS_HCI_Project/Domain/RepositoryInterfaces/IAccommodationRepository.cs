using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IAccommodationRepository
    {
        Accommodation GetById(int id);
        int GenerateId();
        List<Accommodation> GetAll();
        List<Accommodation> GetByOwnerId(int id);
        List<string> GetImages(int id);
        void NotifyObservers();
        void Delete(Accommodation accommodation, Owner owner);
        void Save();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}