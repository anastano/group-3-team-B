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

        public List<AccommodationReservation> GetOverlappingReservations(RescheduleRequest request, AccommodationReservationService reservationService)
        {
            List<AccommodationReservation> overlappingReservations = new List<AccommodationReservation>();

            foreach (AccommodationReservation reservation in reservationService.GetAllReserevedByAccommodationId(request.AccommodationReservation.AccommodationId))
            {
                if (IsDateRangeOverlapping(reservation, request) && reservation.Id != request.AccommodationReservationId)
                {
                    overlappingReservations.Add(reservation);
                }
            }
            return overlappingReservations;
        }

        public bool IsDateRangeOverlapping(AccommodationReservation reservation, RescheduleRequest request)
        {
            bool startOverlaps = reservation.Start >= request.WantedStart && reservation.Start <= request.WantedEnd;
            bool endOverlaps = reservation.End >= request.WantedStart && reservation.End <= request.WantedEnd;

            return startOverlaps || endOverlaps;
        }

        public void EditStatus(int requestId, RescheduleRequestStatus status)
        {
            _requestRepository.EditStatus(requestId, status);
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
