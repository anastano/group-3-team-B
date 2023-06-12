using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class RescheduleRequestRepository : IRescheduleRequestRepository
    {
        private readonly RescheduleRequestFileHandler _fileHandler;

        private static List<RescheduleRequest> _requests;

        public RescheduleRequestRepository()
        {
            _fileHandler = new RescheduleRequestFileHandler();
            _requests = _fileHandler.Load();
        }

        public int GenerateId()
        {
            return _requests.Count == 0 ? 1 : _requests[_requests.Count - 1].Id + 1;
        }

        public void Save()
        {
            _fileHandler.Save(_requests);
        }

        public RescheduleRequest GetById(int id)
        {
            return _requests.Find(a => a.Id == id);
        }

        public List<RescheduleRequest> GetAll()
        {
            return _requests;
        }
        public List<RescheduleRequest> GetAllByOwnerId(int ownerId)
        {
            return _requests.FindAll(r => r.AccommodationReservation.Accommodation.OwnerId == ownerId);
        }

        public List<RescheduleRequest> GetByAccommodationId(int accommodaitonId)
        {
            return _requests.FindAll(r => r.AccommodationReservation.AccommodationId == accommodaitonId);
        }

        public List<RescheduleRequest> GetPendingByOwnerId(int ownerId)
        {
            return _requests.FindAll(r => r.AccommodationReservation.Accommodation.OwnerId == ownerId && r.Status == RescheduleRequestStatus.PENDING);
        }

        public int GetReshedulingCountByYearAndAccommodationId(int year, int accommodationId)
        {
            return GetByAccommodationId(accommodationId).FindAll(r => r.AccommodationReservation.Start.Year == year && r.Status == RescheduleRequestStatus.ACCEPTED).Count();
        }

        public int GetReshedulingCountByMonthAndAccommodationId(int monthIndex, int year, int accommodationId)
        {
            return GetByAccommodationId(accommodationId).FindAll(r => r.AccommodationReservation.Start.Year == year
                         && r.AccommodationReservation.Start.Month == monthIndex && r.Status == Domain.Models.RescheduleRequestStatus.ACCEPTED).Count();
        }

        public void EditStatus(int requestId, RescheduleRequestStatus status)
        {
            RescheduleRequest request = _requests.Find(r => r.Id == requestId);
            request.Status = status;
            Save();
        }
        public void Add(RescheduleRequest request)
        {
            request.Id = GenerateId();
            _requests.Add(request);
            Save();
        }
    }
}
