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
    public class RenovationRepository : ISubject, IRenovationRepository
    {
        private readonly List<IObserver> _observers;
        private readonly RenovationFileHandler _fileHandler;

        private static List<Renovation> _renovations;

        public RenovationRepository()
        {
            _fileHandler = new RenovationFileHandler();
            _renovations = _fileHandler.Load();

            _observers = new List<IObserver>();

        }
        public int GenerateId()
        {
            return _renovations.Count == 0 ? 1 : _renovations[_renovations.Count - 1].Id + 1;
        }

        public void Save()
        {
            _fileHandler.Save(_renovations);
        }

        public Renovation GetById(int id)
        {
            return _renovations.Find(a => a.Id == id);
        }

        public List<Renovation> GetAll()
        {
            return _renovations;
        }

        public List<Renovation> GetByOwnerId(int ownerId)
        {
            return _renovations.FindAll(r => r.Accommodation.OwnerId == ownerId);
        }

        public List<Renovation> GetByAccommodationId(int accommodationId)
        {
            return _renovations.FindAll(r => r.AccommodationId == accommodationId);
        }

        public void Add(Renovation renovation)
        {
            renovation.Id = GenerateId();
            _renovations.Add(renovation);
            NotifyObservers();
            Save();
        }

        public void Delete(Renovation renovation)
        {
            _renovations.Remove(renovation);
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
