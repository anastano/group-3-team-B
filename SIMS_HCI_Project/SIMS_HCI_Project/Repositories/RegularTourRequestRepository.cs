using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;

namespace SIMS_HCI_Project.Repositories
{
    public class RegularTourRequestRepository : IRegularTourRequestRepository
    {
        private readonly RegularTourRequestFileHandler _fileHandler;

        private static List<RegularTourRequest> _requests;

        public RegularTourRequestRepository()
        {

            _fileHandler = new RegularTourRequestFileHandler();
            _requests = _fileHandler.Load();
            UpdateStatusForInvlid();
        }

        private void Save()
        {
            _fileHandler.Save(_requests);
        }

        public int GenerateId()
        {
            return _requests.Count == 0 ? 1 : _requests[_requests.Count - 1].Id + 1;
        }

        public List<RegularTourRequest> GetAll()
        {
            return _requests;
        }

        public RegularTourRequest GetById(int id)
        {
            return _requests.Find(r => r.Id == id);
        }

        public List<RegularTourRequest> GetAllByGuestId(int guestId)
        {
            return _requests.FindAll(r => r.GuestId == guestId);
        }

        public List<RegularTourRequest> GetAllByGuestIdNotPartOfComplex(int guestId)
        {
            UpdateStatusForInvlid();
            return _requests.FindAll(r => r.GuestId == guestId && r.IsPartOfComplex == false);
        }

        public List<RegularTourRequest> GetByGuestIdAndStatus(int guestId, RegularRequestStatus status)
        {
            return _requests.FindAll(r => r.GuestId == guestId && r.Status == status);
        }

        public List<RegularTourRequest> GetByGuestIdAndStatusAndYear(int guestId, RegularRequestStatus status, int year)
        {
            return _requests.FindAll(r => r.GuestId == guestId && r.Status == status && r.SubmittingDate.Year == year);
        }

        public List<RegularTourRequest> GetValidByParams(Location location, int guestNumber, string language, DateRange dateRange)
        {
            return _requests.FindAll(r => (location == null || r.Location.Equals(location))
                                        && (guestNumber == 0 || r.GuestNumber == guestNumber)
                                        && (language == null || language.Equals("") || r.Language.Equals(language))
                                        && (dateRange == null || (r.DateRange.IsInside(dateRange)))
                                        && r.Status == RegularRequestStatus.PENDING);
        }

        public void Add(RegularTourRequest request)
        {
            request.Id = GenerateId();
            _requests.Add(request);

            Save();
        }

        public void Update(RegularTourRequest request)
        {
            RegularTourRequest requestUpdated = GetById(request.Id);
            requestUpdated = request;

            Save();
        }

        public void EditStatus(int requestId, RegularRequestStatus status)
        {
            RegularTourRequest request = _requests.Find(r => r.Id == requestId);
            request.Status = status;
            Save();
        }

        public void UpdateStatusForInvlid() //for those that arent part of complex, may edit when start working on complex tours
        {
            foreach (var request in _requests)
            {
                if (request.IsPartOfComplex == false && DateTime.Now > request.DateRange.Start.AddHours(-48) && request.Status == RegularRequestStatus.PENDING)
                {
                    request.Status = RegularRequestStatus.INVALID;
                    Save();
                }
            }
        }

        public int GetRequestsCountByStatus(RegularRequestStatus status, int guestId)
        {
            return _requests.Where(r => r.GuestId == guestId && r.Status==status).Count();
        }

        public int GetRequestsCountByStatus(RegularRequestStatus status, int guestId, int selectedYear)
        {
            return _requests.Where(r => r.GuestId == guestId && r.Status == status && r.DateRange.Start.Year == selectedYear).Count();
        }

        public int GetCountByYear(int year, string language = null, Location location = null)
        {
            return _requests.Where(r => r.SubmittingDate.Year == year && (language == null || r.Language.Equals(language)) && (location == null || r.Location.Equals(location))).Count();
        }

        public int GetCountByMonthInYear(int year, int month, string language = null, Location location = null)
        {
            return _requests.Where(r => r.SubmittingDate.Year == year && r.SubmittingDate.Month == month && (language == null || r.Language.Equals(language)) && (location == null || r.Location.Equals(location))).Count();
        }
    }
}
