using System;
using System.Collections.Generic;
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

        public RegularTourRequestService()
        {
            _regularTourRequestRepository = Injector.Injector.CreateInstance<IRegularTourRequestRepository>();
            _locationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
        }

        public RegularTourRequest GetById(int id)
        {
            return _regularTourRequestRepository.GetById(id);
        }

        public List<RegularTourRequest> GetAll()
        {
            return _regularTourRequestRepository.GetAll();
        }

        public List<RegularTourRequest> GetAllByGuestId(int id)
        {
            return _regularTourRequestRepository.GetAllByGuestId(id);
        }

        public List<RegularTourRequest> GetAllByGuestIdNotPartOfComplex(int guestId)
        {
            return _regularTourRequestRepository.GetAllByGuestIdNotPartOfComplex(guestId);
        }

        public List<RegularTourRequest> GetByGuestIdAndStatus(int ig, RegularRequestStatus status)
        {
            return _regularTourRequestRepository.GetByGuestIdAndStatus(ig, status);
        }

        public List<RegularTourRequest> GetByParams(Location location, int guestNumber, string language, DateRange dateRange)
        {
            return _regularTourRequestRepository.GetByParams(location, guestNumber, language, dateRange);
        }

        public void EditStatus(int requestId, RegularRequestStatus status)
        {
            _regularTourRequestRepository.EditStatus(requestId, status);
        }

        public void Add(RegularTourRequest request)
        {
            request.Location = _locationRepository.GetOrAdd(request.Location);
            request.LocationId = request.Location.Id;

            _regularTourRequestRepository.Add(request);
        }
    }
}
