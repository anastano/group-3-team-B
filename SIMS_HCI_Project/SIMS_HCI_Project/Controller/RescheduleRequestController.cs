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
    public class RescheduleRequestController : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly RescheduleRequestFileHandler _fileHandler;

        private static List<RescheduleRequest> _requests;

        public RescheduleRequestController()
        {
            _fileHandler = new RescheduleRequestFileHandler();
            Load();
            _observers = new List<IObserver>();
        }
        public void ConnectRequestsWithReservations(AccommodationReservationController accommodationReservationController)
        {
            foreach (RescheduleRequest request in _requests)
            {
                request.AccommodationReservation = accommodationReservationController.FindById(request.AccommodationReservationId);
            }
        }
        public List<RescheduleRequest> GetAll()
        {
            return _requests;
        }

        public List<RescheduleRequest> GetPendingRequestsByOwnerId(int ownerId)
        {
            return _requests.FindAll(r => r.AccommodationReservation.Accommodation.OwnerId == ownerId && r.Status == RescheduleRequestStatus.PENDING);
        }

        public void EditStatus(RescheduleRequest request, RescheduleRequestStatus status)
        {
            request.Status = status;
            Save();
            NotifyObservers();
        }

        public void Load()
        {
            _requests = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_requests);
        }

        public int GenerateId()
        {
            if (_requests.Count == 0)
            {
                return 1;
            }
            return _requests[_requests.Count - 1].Id + 1;

        }
        public void Add(RescheduleRequest request)
        {
            request.Id = GenerateId();
            _requests.Add(request);
            Save();
            NotifyObservers();
        }

        public void Remove(AccommodationReservation reservation)
        {
            // TO DO
        }
        public RescheduleRequest FindById(int id)
        {
            return _requests.Find(r => r.Id == id);
        }
        public static List<RescheduleRequest> GetAllByReservationId(int id)
        {
            return _requests.FindAll(a => a.AccommodationReservationId == id);
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
