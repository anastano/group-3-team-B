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
    public class RescheduleRequestRepository : ISubject, IRescheduleRequestRepository
    {
        private readonly List<IObserver> _observers;
        private readonly RescheduleRequestFileHandler _fileHandler;

        private static List<RescheduleRequest> _requests;

        public RescheduleRequestRepository()
        {
            _fileHandler = new RescheduleRequestFileHandler();
            _requests = _fileHandler.Load();

            _observers = new List<IObserver>();

        }

        public int GenerateId()
        {
            return _requests.Count == 0 ? 1 : _requests[_requests.Count - 1].Id + 1;
        }

        public void Save()
        {
            _fileHandler.Save(_requests);
        }

        public RescheduleRequest GetById(int id)
        {
            return _requests.Find(a => a.Id == id);
        }

        public List<RescheduleRequest> GetAll()
        {
            return _requests;
        }
        public List<RescheduleRequest> GetAllByOwnerId(int ownerId)
        {
            return _requests.FindAll(r => r.AccommodationReservation.Accommodation.OwnerId == ownerId);
        }

        public List<RescheduleRequest> GetPendingByOwnerId(int ownerId)
        {
            return _requests.FindAll(r => r.AccommodationReservation.Accommodation.OwnerId == ownerId && r.Status == RescheduleRequestStatus.PENDING);
        }

        public void EditStatus(int requestId, RescheduleRequestStatus status)
        {
            RescheduleRequest request = _requests.Find(r => r.Id == requestId);
            request.Status = status;
            Save();
            NotifyObservers();
        }
        public void Add(RescheduleRequest request)
        {
            request.Id = GenerateId();
            _requests.Add(request);
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
