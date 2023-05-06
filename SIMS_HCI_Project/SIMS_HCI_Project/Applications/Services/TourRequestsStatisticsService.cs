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
    public class TourRequestsStatisticsService
    {
        private readonly IRegularTourRequestRepository _regularTourRequestRepository;

        public TourRequestsStatisticsService()
        {
            _regularTourRequestRepository = Injector.Injector.CreateInstance<IRegularTourRequestRepository>();
        }

        public TourRequestsStatisticsByStatus GetTourRequestsStatisticsByStatus( int guestId) //get all by guest id
        {
            List<RegularRequestStatus> statuses = new List<RegularRequestStatus> { RegularRequestStatus.PENDING, RegularRequestStatus.ACCEPTED, RegularRequestStatus.INVALID};

            Dictionary<RegularRequestStatus, int> requestsNumberByStatus = new Dictionary<RegularRequestStatus, int>();

            foreach (RegularRequestStatus status in statuses)
            {
                requestsNumberByStatus.Add(status, _regularTourRequestRepository.GetRequestsCountByStatus(status, guestId));
            }

            return new TourRequestsStatisticsByStatus(requestsNumberByStatus);
        }

        public TourRequestsStatisticsByStatus GetTourRequestsStatisticsByStatus(int guestId, int selectedYear) //get all by guest id
        {
            List<RegularRequestStatus> statuses = new List<RegularRequestStatus> { RegularRequestStatus.PENDING, RegularRequestStatus.ACCEPTED, RegularRequestStatus.INVALID };

            Dictionary<RegularRequestStatus, int> requestsNumberByStatus = new Dictionary<RegularRequestStatus, int>();

            foreach (RegularRequestStatus status in statuses)
            {
                requestsNumberByStatus.Add(status, _regularTourRequestRepository.GetRequestsCountByStatus(status, guestId, selectedYear));
            }

            return new TourRequestsStatisticsByStatus(requestsNumberByStatus);
        }
    }
}
