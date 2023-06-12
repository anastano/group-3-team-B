using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SIMS_HCI_Project.Applications.Services
{
    internal class ComplexTourRequestsService
    {
        private readonly IRegularTourRequestRepository _regularTourRequestRepository;
        private readonly IComplexTourRequestRepository _complexTourRequestRepository;

        public ComplexTourRequestsService()
        {
            _regularTourRequestRepository = Injector.Injector.CreateInstance<IRegularTourRequestRepository>();
            _complexTourRequestRepository = Injector.Injector.CreateInstance<IComplexTourRequestRepository>();
            UpdateStatusForInvalid();
        }

        public ComplexTourRequest GetById(int id)
        {
            return _complexTourRequestRepository.GetById(id);
        }

        public List<ComplexTourRequest> GetAll()
        {
            return _complexTourRequestRepository.GetAll();
        }

        public Tour AcceptRequest(RegularTourRequest request, Guide guide, DateTime departureTime)
        {
            if (request.ComplexTourRequest.HasAcceptedPart(guide.Id) || 
                guide.IsBusy(new DateRange(departureTime, 2)) || 
                request.ComplexTourRequest.IsTimeSlotScheduled(new DateRange(departureTime, 2))) return null;

            RegularTourRequestService regularTourRequestService = new RegularTourRequestService();
            Tour tourFromRequest = regularTourRequestService.AcceptRequest(request, guide, departureTime);

            if (request.ComplexTourRequest.AllPartsAccepted())
            {
                request.ComplexTourRequest.Accept();
                _complexTourRequestRepository.Update(request.ComplexTourRequest);
            }

            return tourFromRequest;
        }

        public List<DateTime> GeneratePossibleDepartureTimes(RegularTourRequest request, Guide guide)
        {
            List<DateTime> possibleDepartureTimes = new List<DateTime>();

            for (DateTime possibleDepartureTime = request.DateRange.Start > DateTime.Now ? request.DateRange.Start.Date : DateTime.Now.AddDays(1).Date; possibleDepartureTime < request.DateRange.End; possibleDepartureTime = possibleDepartureTime.AddHours(2))
            {
                DateRange possibleTimeSlot = new DateRange(possibleDepartureTime, 2);
                if (guide.IsBusy(possibleTimeSlot) || request.ComplexTourRequest.IsTimeSlotScheduled(possibleTimeSlot)) continue;

                possibleDepartureTimes.Add(possibleDepartureTime);
            }

            return possibleDepartureTimes;
        }
        
        public List<ComplexTourRequest> GetAllByGuestId(int guestId)
        {
            return _complexTourRequestRepository.GetAllByGuestId(guestId);
        }

        public void Add(ComplexTourRequest request)
        {
            _complexTourRequestRepository.Add(request);
        }

        public void Update(ComplexTourRequest request)
        {
            _complexTourRequestRepository.Update(request);
        }

        private void UpdateStatusForInvalid() 
        {
            List<ComplexTourRequest> complexRequests = _complexTourRequestRepository.GetAll();
            foreach (var complexRequest in complexRequests)
            {
                CheckStartDateRange(complexRequest);
            }
        }

        private void CheckStartDateRange(ComplexTourRequest complexRequest)
        {
            foreach (var regularRequest in complexRequest.TourRequests)
            {
                if (DateTime.Now > regularRequest.DateRange.Start.AddHours(-48) && complexRequest.Status == TourRequestStatus.PENDING)
                {
                    complexRequest.Invalidate();
                    _complexTourRequestRepository.Update(complexRequest);
                }
            }
        }
    }
}
