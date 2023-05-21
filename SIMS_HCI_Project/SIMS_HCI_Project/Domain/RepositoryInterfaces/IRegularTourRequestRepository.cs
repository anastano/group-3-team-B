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
        List<RegularTourRequest> GetAll();
        RegularTourRequest GetById(int id);
        List<RegularTourRequest> GetAllByGuestId(int guestId);
        List<RegularTourRequest> GetAllByGuestThatArentPartOfComplex(int guestId);
        List<RegularTourRequest> GetAllByGuestIdAndStatus(int guestId, RegularRequestStatus status);
        List<RegularTourRequest> GetAllByGuestIdAndStatusAndYear(int guestId, RegularRequestStatus status, int year);
        List<RegularTourRequest> GetAllValidByParams(Location location, int guestNumber, string language, DateRange dateRange);
        void Add(RegularTourRequest request);
        int GenerateId();
        int GetRequestsCountByStatus(RegularRequestStatus status, int guestId);
        int GetRequestsCountByStatusAndYear(RegularRequestStatus status, int guestId, int selectedYear);
        List<RegularTourRequest> GetAllByGuestIdNotPartOfComplex(int guestId);
        List<RegularTourRequest> GetByGuestIdAndStatus(int guestId, RegularRequestStatus status);
        List<RegularTourRequest> GetByGuestIdAndStatusAndYear(int guestId, RegularRequestStatus status, int year);
        List<RegularTourRequest> GetValidByParams(Location location, int guestNumber, string language, DateRange dateRange);
        List<RegularTourRequest> GetInvalidByParams(int locationId, string language);
        void Add(RegularTourRequest request);
        Location GetTopLocation();
        string GetTopLanguage();
        void Update(RegularTourRequest request);
        void EditStatus(int requestId, RegularRequestStatus status);
    }
}
