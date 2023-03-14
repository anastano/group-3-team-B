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
    public class AccommodationReservationController : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly AccommodationReservationFileHandler _fileHandler;

        private static List<AccommodationReservation> _reservations;



        public AccommodationReservationController()
        {
            if (_reservations == null)
            {
                _reservations = new List<AccommodationReservation>();
            }

            _fileHandler = new AccommodationReservationFileHandler();
            _observers = new List<IObserver>();

        }

        public List<AccommodationReservation> GetAll()
        {
            return _reservations;
        }


        public void Load() // load from file
        {
            _reservations = _fileHandler.Load();
        }


        public void Save() //save to file
        {
            _fileHandler.Save(_reservations);
        }

        public int GenerateId()
        {
            if (_reservations.Count == 0)
            {
                return 1;
            }
            else
            {
                return _reservations[_reservations.Count - 1].Id + 1;
            }
        }


        public void Add(AccommodationReservation reservation)
        {
            // TO DO
        }

        public void Remove(AccommodationReservation reservation)
        {
            // TO DO
        }

        public void Edit(AccommodationReservation reservation)
        {
            // TO DO
        }

        public AccommodationReservation FindById(int id)
        {
            return _reservations.Find(r => r.Id == id);
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
