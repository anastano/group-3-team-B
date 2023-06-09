using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface IRegularTourRequestRepository
    {
        RegularTourRequest GetById(int id);
        List<RegularTourRequest> GetAll();
        List<RegularTourRequest> GetAllByGuestId(int guestId, bool? isPartOfComplex = null);
        List<RegularTourRequest> GetAllByGuestIdAndStatusAndYear(int guestId, TourRequestStatus status, int? year = null);
        List<RegularTourRequest> GetAllValidByParams(Location location, int guestNumber, string language, DateRange dateRange);
        List<RegularTourRequest> GetInvalidByParams(int locationId, string language);

        void Add(RegularTourRequest request);
        void Update(RegularTourRequest request);

        int GetRequestsCountByStatus(TourRequestStatus status, int guestId, int? selectedYear = null);
        int GetCountByYear(int year, string language = null, Location location = null);
        int GetCountByMonthInYear(int year, int month, string language = null, Location location = null);
        Location GetTopLocation();
        string GetTopLanguage();
    }
}
