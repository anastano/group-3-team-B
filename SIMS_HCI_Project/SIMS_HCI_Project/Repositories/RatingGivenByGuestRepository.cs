using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class RatingGivenByGuestRepository : ISubject, IRatingGivenByGuestRepository
    {

        private readonly List<IObserver> _observers;
        private readonly RatingGivenByGuestFileHandler _fileHandler;

        private static List<RatingGivenByGuest> _ratings;

        public RatingGivenByGuestRepository()
        {
            _fileHandler = new RatingGivenByGuestFileHandler();
            _ratings = _fileHandler.Load();
            _observers = new List<IObserver>();
        }
        public int GenerateId()
        {
            return _ratings.Count == 0 ? 1 : _ratings[_ratings.Count - 1].Id + 1;
        }
        public void Save()
        {
            _fileHandler.Save(_ratings);
        }
        public RatingGivenByGuest GetById(int id)
        {
            return _ratings.Find(r => r.Id == id);
        }
        public List<RatingGivenByGuest> GetAll()
        {
            return _ratings;
        }
        public void Add(RatingGivenByGuest rating)
        {
            rating.Id = GenerateId();
            _ratings.Add(rating);
            Save();
            NotifyObservers();
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
