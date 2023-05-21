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

        public TourRequestsStatisticsByStatus GetTourRequestsStatisticsByStatus( int guestId, int? selectedYear = null) //get all by guest id
        {
            List<RegularRequestStatus> statuses = new List<RegularRequestStatus> { RegularRequestStatus.PENDING, RegularRequestStatus.ACCEPTED, RegularRequestStatus.INVALID};

            Dictionary<RegularRequestStatus, int> requestsNumberByStatus = new Dictionary<RegularRequestStatus, int>();

            foreach (RegularRequestStatus status in statuses)
            {
                requestsNumberByStatus.Add(status, _regularTourRequestRepository.GetRequestsCountByStatus(status, guestId, selectedYear));
            }

            return new TourRequestsStatisticsByStatus(requestsNumberByStatus);
        }

        public Dictionary<string, int> GetTourRequestStatisticsByLanguages(int guestId)
        {
            Dictionary<string, int> requestsNumberByLanguage = new Dictionary<string, int>();
            foreach(RegularTourRequest request in _regularTourRequestRepository.GetAllByGuestId(guestId))
            {
                requestsNumberByLanguage[request.Language] = requestsNumberByLanguage.ContainsKey(request.Language) ? requestsNumberByLanguage[request.Language] + 1 : 1;
            }
            return requestsNumberByLanguage;  
        }

        public Dictionary<int, int> GetTourRequestStatisticsByLocationId(int guestId)
        {
            Dictionary<int, int> requestsNumberByLocationId = new Dictionary<int, int>();
            foreach (RegularTourRequest request in _regularTourRequestRepository.GetAllByGuestId(guestId))
            {
                requestsNumberByLocationId[request.LocationId] = requestsNumberByLocationId.ContainsKey(request.LocationId) ? requestsNumberByLocationId[request.LocationId] + 1 : 1;
            }
            return requestsNumberByLocationId;
        }

        public Dictionary<int, int> GetTourRequesPerYear(string language = null, Location location = null)
        {
            if (language == null && location == null) return null;

            Dictionary<int, int> requestPerYear = new Dictionary<int, int>();
            List<int> distinctYears = _regularTourRequestRepository.GetAll().Select(r => r.SubmittingDate.Year).Distinct().ToList();

            foreach (int year in distinctYears)
            {
                requestPerYear.Add(year, _regularTourRequestRepository.GetCountByYear(year, language, location));
            }

            return requestPerYear;
        }

        public Dictionary<int, int> GetTourRequesPerMonth(int year, string language = null, Location location = null)
        {
            if (language == null && location == null) return null;

            Dictionary<int, int> requestPerMonth = new Dictionary<int, int>();

            for (int month = 1; month < 12; month++)
            {
                requestPerMonth.Add(month, _regularTourRequestRepository.GetCountByMonthInYear(year, month, language, location));
            }

            return requestPerMonth;
        }

        public Location GetTopLocation()
        {
            return _regularTourRequestRepository.GetTopLocation();
        }

        public string GetTopLanguage()
        {
            return _regularTourRequestRepository.GetTopLanguage();
        }
    }
}
