using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;

namespace SIMS_HCI_Project.Applications.Services
{
    public class RegularTourRequestService
    {
        private readonly IRegularTourRequestRepository _regularTourRequestRepository;

        public RegularTourRequestService()
        {
            _regularTourRequestRepository = Injector.Injector.CreateInstance<IRegularTourRequestRepository>();
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

        public List<RegularTourRequest> GetByGuestIdAndStatus(int ig, RegularRequestStatus status)
        {
            return _regularTourRequestRepository.GetByGuestIdAndStatus(ig, status);
        }

        public void EditStatus(int requestId, RegularRequestStatus status)
        {
            _regularTourRequestRepository.EditStatus(requestId, status);
        }

        public void Add(RegularTourRequest request)
        {
            _regularTourRequestRepository.Add(request);
        }
    }
}
