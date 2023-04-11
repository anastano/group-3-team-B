using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class RescheduleRequestService
    {

        private readonly IRescheduleRequestRepository _requestRepository;

        public RescheduleRequestService()
        {
            _requestRepository = Injector.Injector.CreateInstance<IRescheduleRequestRepository>();
        }

        public void Save()
        {
            _requestRepository.Save();
        }

        public RescheduleRequest GetById(int id)
        {
            return _requestRepository.GetById(id);
        }

        public List<RescheduleRequest> GetAll()
        {
            return _requestRepository.GetAll();
        }
        public List<RescheduleRequest> GetAllByOwnerId(int ownerId)
        {
            return _requestRepository.GetAllByOwnerId(ownerId);
        }

        public List<RescheduleRequest> GetPendingByOwnerId(int ownerId)
        {
            return _requestRepository.GetPendingByOwnerId(ownerId);
        }

        public void EditStatus(int requestId, RescheduleRequestStatus status)
        {
            _requestRepository.EditStatus(requestId, status);
        }
        public void ConnectRequestsWithReservations(AccommodationReservationService reservationService)
        {
            foreach (RescheduleRequest request in _requestRepository.GetAll())
            {
                request.AccommodationReservation = reservationService.GetById(request.AccommodationReservationId);
            }
        }
        public void Add(RescheduleRequest rescheduleRequest)
        {
            _requestRepository.Add(rescheduleRequest);
        }

        public void NotifyObservers()
        {
            _requestRepository.NotifyObservers();
        }

        public void Subscribe(IObserver observer)
        {
            _requestRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _requestRepository.Unsubscribe(observer);
        }
    }
}
