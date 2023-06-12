using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class ComplexTourRequestRepository : IComplexTourRequestRepository
    {
        private readonly ComplexTourRequestFileHandler _fileHandler;

        private static List<ComplexTourRequest> _requests;

        public ComplexTourRequestRepository()
        {

            _fileHandler = new ComplexTourRequestFileHandler();
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

        public ComplexTourRequest GetById(int id)
        {
            return _requests.Find(r => r.Id == id);
        }

        public List<ComplexTourRequest> GetAll()
        {
            return _requests;
        }

        public List<ComplexTourRequest> GetAllByGuestId(int guestId)
        {
            return _requests.FindAll(r => r.GuestId == guestId);
        }

        public void Add(ComplexTourRequest request)
        {
            request.Id = GenerateId();
            _requests.Add(request);

            Save();
        }

        public void Update(ComplexTourRequest request)
        {
            ComplexTourRequest requestUpdated = GetById(request.Id);
            requestUpdated = request;

            Save();
        }
    }
}
