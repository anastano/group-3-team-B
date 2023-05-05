using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;

namespace SIMS_HCI_Project.Repositories
{
    public class RegularTourRequestRepository : IRegularTourRequestRepository
    {
        private readonly RegularTourRequestFileHandler _fileHandler;

        private static List<RegularTourRequest> _requests;
        private LocationRepository _locationRepository;

        public RegularTourRequestRepository()
        {

            _fileHandler = new RegularTourRequestFileHandler();
            _requests = _fileHandler.Load();
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


        public List<RegularTourRequest> GetByGuestIdAndStatus(int guestId, RegularRequestStatus status)
        {
            return _requests.FindAll(r => r.GuestId == guestId && r.Status == status);
        }

        public void Add(RegularTourRequest request)
        {
            request.Id = GenerateId();
            _requests.Add(request);

            Save();
        }

        public void EditStatus(int requestId, RegularRequestStatus status)
        {
            RegularTourRequest request = _requests.Find(r => r.Id == requestId);
            request.Status = status;
            Save();
        }

        public void Save()
        {
            _fileHandler.Save(_requests);
        }
    }
}
