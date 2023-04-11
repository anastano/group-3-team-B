using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Controller
{
    public class TourRatingController : ISubject 
    {
        private readonly List<IObserver> _observers;
        private readonly TourRatingFileHandler _fileHandler;
        private static List<TourRating> _ratings;
        

        public TourRatingController()
        {
            _fileHandler = new TourRatingFileHandler();
            if (_ratings == null)
            {
                Load();
            }

            _observers = new List<IObserver>();
        }

        public List<TourRating> GetAll()
        {
            return _ratings;
        }
        public TourRating GetById(int id)
        {
            return _ratings.Find(r => r.Id == id);
        }

        public void Load()
        {
            _ratings = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_ratings);
        }

        public int GenerateId()
        {
            if (_ratings.Count == 0)
            {
                return 1;
            }
            return _ratings[_ratings.Count - 1].Id + 1;

        }

        public void Add(TourRating rating)
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
