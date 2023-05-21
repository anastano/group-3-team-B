using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;

namespace SIMS_HCI_Project.Applications.Services
{
    public class RegularTourRequestService
    {
        private readonly IRegularTourRequestRepository _regularTourRequestRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ITourTimeRepository _tourTimeRepository;

        public RegularTourRequestService()
        {
            _regularTourRequestRepository = Injector.Injector.CreateInstance<IRegularTourRequestRepository>();
            _locationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            _notificationRepository = Injector.Injector.CreateInstance<INotificationRepository>();
            _tourTimeRepository = Injector.Injector.CreateInstance<ITourTimeRepository>();

            UpdateStatusForInvalid();
        }

        public RegularTourRequest GetById(int id)
        {
            return _regularTourRequestRepository.GetById(id);
        }

        public List<RegularTourRequest> GetAll()
        {
            return _regularTourRequestRepository.GetAll();
        }

        public List<RegularTourRequest> GetAllByGuestId(int id, bool? isPartOfComplex = null)
        {
            UpdateStatusForInvalid();
            return _regularTourRequestRepository.GetAllByGuestId(id, isPartOfComplex);
        }

        public List<RegularTourRequest> GetAllByGuestIdAndStatusAndYear(int ig, RegularRequestStatus status, int? year = null)
        {
            return _regularTourRequestRepository.GetAllByGuestIdAndStatusAndYear(ig, status, year);
        }
        
        public List<RegularTourRequest> GetAllValidByParams(Location location, int guestNumber, string language, DateRange dateRange)
        {
            return _regularTourRequestRepository.GetAllValidByParams(location, guestNumber, language, dateRange);
        }

        public void Add(RegularTourRequest request)
        {
            request.Location = _locationRepository.GetOrAdd(request.Location);
            request.LocationId = request.Location.Id;

            _regularTourRequestRepository.Add(request);
        }

        public void EditStatus(RegularTourRequest request, RegularRequestStatus status)
        {
            request.Status = status;
            _regularTourRequestRepository.Update(request);
        }

        public Tour AcceptRequest(RegularTourRequest request, int guideId, DateTime departureTime)
        {
            if (_tourTimeRepository.GetAllInDateRange(guideId, new DateRange(departureTime, 2)).Count != 0) return null;

            request.Accept();
            _regularTourRequestRepository.Update(request);

            Tour tourFromRequest = CreateTourFromRequest(request, guideId, departureTime);

            TourService tourService = new TourService(); // because creation of tour is complex
            tourService.Add(tourFromRequest);

            Notification notification = new Notification("Your request number for tour was accepted. Departure time: " + departureTime.ToShortDateString()
                                                            + ". Created tour number: [" + tourFromRequest.Id.ToString() + "].",
                                                            request.GuestId, false, NotificationType.TOUR_REQUEST_ACCEPTED);
            _notificationRepository.Add(notification);

            return tourFromRequest;
        }

        private Tour CreateTourFromRequest(RegularTourRequest request, int guideId, DateTime departureTime) // create constructor for this?
        {
            Tour tourFromRequest = new Tour()
                            {
                                Title = "Requested Tour by request " + request.Id.ToString(),
                                Language = request.Language,
                                Location = request.Location,
                                LocationId = request.LocationId,
                                Description = request.Description,
                                MaxGuests = request.GuestNumber,
                                Duration = 2,
                                GuideId = guideId,
                            };
            tourFromRequest.DepartureTimes.Add(new TourTime(tourFromRequest.Id, departureTime));
            tourFromRequest.KeyPoints.Add(new TourKeyPoint("Start"));
            tourFromRequest.KeyPoints.Add(new TourKeyPoint("End"));

            return tourFromRequest;
        }

        private void UpdateStatusForInvalid() //for those that arent part of complex, may edit when start working on complex tours
        {
            foreach (var request in _regularTourRequestRepository.GetAll())
            {
                if (!request.IsPartOfComplex && DateTime.Now > request.DateRange.Start.AddHours(-48) && request.Status == RegularRequestStatus.PENDING)
                {
                    request.Invalidate();
                    _regularTourRequestRepository.Update(request);
                }
            }
        }
    }
}
