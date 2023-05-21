using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
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
            DateRange requestDateRange = new DateRange(request.WantedStart, request.WantedEnd);

            foreach (AccommodationReservation reservation in reservationService.GetAllReserevedByAccommodationId(request.AccommodationReservation.AccommodationId))
            {
                if (requestDateRange.DoesOverlap(new DateRange(reservation.Start, reservation.End)) && reservation.Id != request.AccommodationReservationId)
                {
                    overlappingReservations.Add(reservation);
                }
            }
            return overlappingReservations;
        }

        public void EditStatus(int requestId, RescheduleRequestStatus status)
        {
            _requestRepository.EditStatus(requestId, status);
        }
        public void Add(RescheduleRequest rescheduleRequest)
        {
            _requestRepository.Add(rescheduleRequest);
        }
    }
}
