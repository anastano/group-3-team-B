using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface IRegularTourRequestRepository
    {
        void EditStatus(int requestId, RegularRequestStatus status);
        List<RegularTourRequest> GetAll();
        RegularTourRequest GetById(int id);
        List<RegularTourRequest> GetAllByGuestId(int guestId);
        List<RegularTourRequest> GetAllByGuestIdNotPartOfComplex(int guestId);
        List<RegularTourRequest> GetByGuestIdAndStatus(int guestId, RegularRequestStatus status);
        void Add(RegularTourRequest request);
        int GenerateId();
        int GetRequestsCountByStatus(RegularRequestStatus status, List<RegularTourRequest> requests);
        int GetRequestsCountByStatus(RegularRequestStatus status, int guestId);
    }
}
