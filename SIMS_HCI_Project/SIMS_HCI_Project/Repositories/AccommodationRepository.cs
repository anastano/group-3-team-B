using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class AccommodationRepository : ISubject, IAccommodationRepository
    {
        private readonly List<IObserver> _observers;
        private readonly AccommodationFileHandler _fileHandler;

        private static List<Accommodation> _accommodations;

        public AccommodationRepository()
        {
            if (_accommodations == null)
            {
                _accommodations = new List<Accommodation>();
            }

            _fileHandler = new AccommodationFileHandler();
            _observers = new List<IObserver>();

        }

        public Accommodation FindById(int id)
        {
            return _accommodations.Find(a => a.Id == id);
        }

        public int GenerateId()
        {
            return _accommodations.Count == 0 ? 1 : _accommodations[_accommodations.Count - 1].Id + 1;
        }
        public List<Accommodation> GetAll()
        {
            return _accommodations;
        }

        public List<string> GetImages(int id)
        {
            return _accommodations.Find(a => a.Id == id).Images;
        }

        public void Load()
        {
            _accommodations = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_accommodations);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

    }
}
