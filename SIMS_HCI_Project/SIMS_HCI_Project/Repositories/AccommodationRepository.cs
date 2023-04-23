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
            _fileHandler = new AccommodationFileHandler();
            _accommodations = _fileHandler.Load();

            _observers = new List<IObserver>();

        }

        public int GenerateId()
        {
            return _accommodations.Count == 0 ? 1 : _accommodations[_accommodations.Count - 1].Id + 1;
        }

        public void Save()
        {
            _fileHandler.Save(_accommodations);
        }

        public Accommodation GetById(int id)
        {
            return _accommodations.Find(a => a.Id == id);
        }

        public List<Accommodation> GetAll()
        {
            return _accommodations;
        }

        public List<Accommodation> GetByOwnerId(int ownerId)
        {
            return _accommodations.FindAll(a => a.OwnerId == ownerId);
        }

        public List<string> GetImages(int id)
        {
            return _accommodations.Find(a => a.Id == id).Images;
        }

        public void Add(Accommodation accommodation)
        {
            accommodation.Id = GenerateId();
            _accommodations.Add(accommodation);
            NotifyObservers();
            Save();
        }

        public void Delete(Accommodation accommodation)
        {
            _accommodations.Remove(accommodation);
            NotifyObservers();
            Save();
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
