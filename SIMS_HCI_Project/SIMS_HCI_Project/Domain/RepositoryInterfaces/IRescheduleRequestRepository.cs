using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IRescheduleRequestRepository
    {
        void EditStatus(int requestId, RescheduleRequestStatus status); // To Update #New
        List<RescheduleRequest> GetAll();
        RescheduleRequest GetById(int id);
        List<RescheduleRequest> GetAllByOwnerId(int ownerId);
        List<RescheduleRequest> GetPendingByOwnerId(int ownerId);
        void Add(RescheduleRequest request);
        void Save();
        List<RescheduleRequest> GetByAccommodationId(int accommodaitonId);
        int GetReshedulingCountByYearAndAccommodationId(int year, int accommodationId);
        int GetReshedulingCountByMonthAndAccommodationId(int monthIndex, int year, int accommodationId);
    }
}