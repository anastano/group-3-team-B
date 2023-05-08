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
        List<RegularTourRequest> GetAllByGuestIdNotPartOfComplex(int guestId);
        List<RegularTourRequest> GetByGuestIdAndStatus(int guestId, RegularRequestStatus status);
        List<RegularTourRequest> GetValidByParams(Location location, int guestNumber, string language, DateRange dateRange);

        void Add(RegularTourRequest request);
        void Update(RegularTourRequest request);

        void EditStatus(int requestId, RegularRequestStatus status);
    }
}
