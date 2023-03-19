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
    public class Guest1Controller : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly Guest1FileHandler _fileHandler;

        private static List<Guest1> _guests;

        public Guest1Controller()
        {
            if (_guests == null)
            {
                _guests = new List<Guest1>();
            }

            _fileHandler = new Guest1FileHandler();
            _observers = new List<IObserver>();

        }

        public List<Guest1> GetAll()
        {
            return _guests;
        }

        public void Load() // load from file
        {
            _guests = _fileHandler.Load();
        }


        public void Save() //save to file
        {
            _fileHandler.Save(_guests);
        }

        public void Add(AccommodationReservation accommodationReservation)
        {
            //TO DO IF NEEDED
        }

        public void Remove(AccommodationReservation accommodationReservation)
        {
            //TO DO IF NEEDED
        }

        public void Edit(AccommodationReservation accommodationReservation)
        {
            //TO DO IF NEEDED
        }

        public Guest1 FindById(string id)
        {
            return _guests.Find(g => g.Id == id);
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
