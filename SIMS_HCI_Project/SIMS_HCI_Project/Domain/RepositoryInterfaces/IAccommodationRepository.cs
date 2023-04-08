using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IAccommodationRepository
    {
        Accommodation FindById(int id);
        int GenerateId();
        List<Accommodation> GetAll();
        List<string> GetImages(int id);
        void Load();
        void NotifyObservers();
        void Save();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}