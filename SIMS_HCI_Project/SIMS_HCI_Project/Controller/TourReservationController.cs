using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Observer;
using System.Collections.Generic;

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
                Load();
            }

            _observers = new List<IObserver>();
        }

        public List<TourReservation> GetAll()
        {
            return _reservations;
        }

        public void Load()
        {
            _reservations = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_reservations);
        }
        public void Add(TourReservation tourReservation)
        {
            tourReservation.Id = GenerateId();

            _reservations.Add(tourReservation);
            Save();
        }
        public TourReservation FindById(int id)
        {
            return _reservations.Find(r => r.Id == id);
        }

        public List<TourReservation> GetAllByGuestId(int id)
        {
            return _reservations.FindAll(r => r.Guest2Id == id);
        }

        public List<TourReservation> GetAllByTourTimeId(int id)
        {
            return _reservations.FindAll(r => r.TourTimeId == id);
        }

        public int GenerateId()
        {
            if(_reservations.Count == 0)
            {
                return 1;
            }
            return _reservations[_reservations.Count - 1].Id + 1;
        }
        public void ConnectTourTimes()
        {
            TourTimeController tourTimeController = new TourTimeController();
            foreach(TourReservation tourReservation in _reservations)
            {
                tourReservation.TourTime = tourTimeController.FindById(tourReservation.TourTimeId);
            }
        }

        public void ConnectVouchers()
        {
            TourVoucherController tourVoucherController = new TourVoucherController();
            foreach(TourReservation tourReservation in _reservations)
            {
                tourReservation.TourVoucher = tourVoucherController.FindById(tourReservation.VoucherUsedId);
            }
        }
        public void LoadConnections()
        {
            ConnectTourTimes();
            ConnectVouchers();
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
