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
    public class OwnerGuestRatingController : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly OwnerGuestRatingFileHandler _fileHandler;

        private static List<OwnerGuestRating> _ratings;

        public OwnerGuestRatingController()
        {
            if (_ratings == null)
            {
                _ratings = new List<OwnerGuestRating>();
            }

            _fileHandler = new OwnerGuestRatingFileHandler();
            _observers = new List<IObserver>();

        }

        public List<OwnerGuestRating> GetAll()
        {
            return _ratings;
        }


        public void Load() // load from file
        {
            _ratings = _fileHandler.Load();
        }


        public void Save() //save to file
        {
            _fileHandler.Save(_ratings);
        }

        public int GenerateId()
        {
            if (_ratings.Count == 0)
            {
                return 1;
            }
            else
            {
                return _ratings[_ratings.Count - 1].Id + 1;
            }
        }


        public void Add(OwnerGuestRating rating)
        {
            rating.Id = GenerateId();
            _ratings.Add(rating);
            NotifyObservers();
            Save();
        }

        public void Remove(OwnerGuestRating rating)
        {
            // TO DO
        }

        public void Edit(OwnerGuestRating rating)
        {
            // TO DO
        }

        public OwnerGuestRating FindById(int id)
        {
            return _ratings.Find(r => r.Id == id);
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
