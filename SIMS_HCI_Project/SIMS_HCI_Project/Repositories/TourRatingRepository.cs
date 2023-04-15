using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;
using SIMS_HCI_Project.Observer;

namespace SIMS_HCI_Project.Repositories
{
    public class TourRatingRepository : ISubject, ITourRatingRepository
    {
        private readonly List<IObserver> _observers;
        private readonly TourRatingFileHandler _fileHandler;
        private static List<TourRating> _ratings;

        public TourRatingRepository()
        {
            _fileHandler = new TourRatingFileHandler();
            if (_ratings == null)
            {
                Load();
            }
            _observers = new List<IObserver>();

        }

        public void Load()
        {
            _ratings = _fileHandler.Load();
        }
        public void Save()
        {
            _fileHandler.Save(_ratings);
        }

        public List<TourRating> GetAll()
        {
            return _ratings;
        }

        public TourRating GetById(int id)
        {
            return _ratings.Find(r => r.Id == id);
        }
        public void Add(TourRating rating)
        {
            rating.Id = GenerateId();
            _ratings.Add(rating);
            Save();
            NotifyObservers();
        }

        public int GenerateId()
        {
            if (_ratings.Count == 0)
            {
                return 1;
            }
            return _ratings[_ratings.Count - 1].Id + 1;
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
