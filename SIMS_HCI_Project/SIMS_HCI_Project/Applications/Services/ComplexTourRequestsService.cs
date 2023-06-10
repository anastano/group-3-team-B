using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public List<ComplexTourRequest> GetAll()
        {
            return _complexTourRequestRepository.GetAll();
        }

        public Tour AcceptRequest(RegularTourRequest request, int guideId, DateTime departureTime)
        {
            if (request.ComplexTourRequest.HasPart(guideId)) return null;

            /* I hate this */

            return null;
        }

    }
}
