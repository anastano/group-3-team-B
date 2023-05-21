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
            Load();
        }

        private void Load()
        {
            _requests = _fileHandler.Load();
        }

        private void Save()
        {
            _fileHandler.Save(_requests);
        }

        private int GenerateId()
        {
            return _requests.Count == 0 ? 1 : _requests[_requests.Count - 1].Id + 1;
        }

        public RegularTourRequest GetById(int id)
        {
            return _requests.Find(r => r.Id == id);
        }

        public List<RegularTourRequest> GetAllByGuestId(int guestId)
        {
            return _requests.FindAll(r => r.GuestId == guestId);
        }

        public List<RegularTourRequest> GetAllByGuestThatArentPartOfComplex(int guestId)
        {
            UpdateStatusForInvlid();
            return _requests.FindAll(r => r.GuestId == guestId && r.IsPartOfComplex == false);
        }

        public List<RegularTourRequest> GetAllByGuestId(int guestId, bool? isPartOfComplex = null)
        {
            return _requests.FindAll(r => r.GuestId == guestId && (isPartOfComplex == null || r.IsPartOfComplex == isPartOfComplex));
        }

        public List<RegularTourRequest> GetByGuestIdAndStatusAndYear(int guestId, RegularRequestStatus status, int? year = null)
        {
            return _requests.FindAll(r => r.GuestId == guestId && r.Status == status && (year == null ||  r.SubmittingDate.Year == year));
        }

        public List<RegularTourRequest> GetAllValidByParams(Location location, int guestNumber, string language, DateRange dateRange)
        {
            return _requests.FindAll(r => (location == null || r.Location.Equals(location))
                                        && (guestNumber == 0 || r.GuestNumber == guestNumber)
                                        && (language == null || language.Equals("") || r.Language.Equals(language))
                                        && (dateRange == null || (r.DateRange.IsInside(dateRange)))
                                        && r.Status == RegularRequestStatus.PENDING);
        }

        public List<RegularTourRequest> GetInvalidByParams(int locationId, string language)
        {
            return _requests.FindAll(r => (r.LocationId == locationId || r.Language.Contains(language))
                                    && ( r.Status == RegularRequestStatus.INVALID ));
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

        public int GetRequestsCountByStatus(RegularRequestStatus status, int guestId, int? selectedYear = null)
        {
            return _requests.Where(r => r.GuestId == guestId && r.Status==status && (selectedYear == null || r.DateRange.Start.Year == selectedYear)).Count();
        }

        public int GetCountByYear(int year, string language = null, Location location = null)
        {
            return _requests.Where(r => r.SubmittingDate.Year == year && (language == null || r.Language.Equals(language)) && (location == null || r.Location.Equals(location))).Count();
        }

        public int GetCountByMonthInYear(int year, int month, string language = null, Location location = null)
        {
            return _requests.Where(r => r.SubmittingDate.Year == year && r.SubmittingDate.Month == month && (language == null || r.Language.Equals(language)) && (location == null || r.Location.Equals(location))).Count();
        }

        public Location GetTopLocation()
        {
            return _requests.Where(r => r.SubmittingDate > DateTime.Now.AddYears(-1)).GroupBy(r => r.LocationId).OrderByDescending(r => r.Count()).First().First().Location;
        }

        public string GetTopLanguage()
        {
            return _requests.Where(r => r.SubmittingDate > DateTime.Now.AddYears(-1)).GroupBy(r => r.Language).OrderByDescending(r => r.Count()).First().First().Language;
        }

        
    }
}
