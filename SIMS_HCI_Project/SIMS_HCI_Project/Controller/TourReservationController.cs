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
    internal class TourReservationController : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly TourReservationFileHandler _fileHandler;
        private static List<TourReservation> _reservations;

        public TourReservationController()
        {
            _fileHandler = new TourReservationFileHandler();
            if (_reservations == null)
            {
                _reservations = _fileHandler.Load();
            }

            _observers = new List<IObserver>();
        }

        public TourReservation FindById(int id)
        {
            return _reservations.Find(r => r.Id == id);
        }

        public List<TourReservation> GetAllByGuestId(string id)
        {
            return _reservations.FindAll(r => r.Guest2Id == id);
        }
        public void Load()
        {
            _reservations = _fileHandler.Load();
        }
        public void Save(TourReservation tourReservation)
        {
            tourReservation.Id = GenerateId();

            _reservations.Add(tourReservation);
            _fileHandler.Save(_reservations);
        }



        public List<TourReservation> GetReservationsByTourTime(int id)
        {
            return _reservations.FindAll(r => r.TourTimeId == id);
        }

        public List<TourReservation> GetAll()
        {
            return _reservations;
        }

        public int GenerateId()
        {
            if(_reservations.Count == 0)
            {
                return 1;
            }
            return _reservations[_reservations.Count - 1].Id + 1;
        }

        public void Add(TourReservation reservation)
        {
            //TODO
        }

        public void Remove(TourReservation reservation)
        {
            //TODO
        }

        public void Edit(TourReservation reservation)
        {
            //TODO
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
