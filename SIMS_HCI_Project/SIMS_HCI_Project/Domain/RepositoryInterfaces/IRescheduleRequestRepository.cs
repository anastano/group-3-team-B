using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IRescheduleRequestRepository
    {
        void EditStatus(int requestId, RescheduleRequestStatus status); // To Update #New
        int GenerateId();
        List<RescheduleRequest> GetAll();
        RescheduleRequest GetById(int id);
        List<RescheduleRequest> GetAllByOwnerId(int ownerId);
        List<RescheduleRequest> GetPendingByOwnerId(int ownerId);
        void Add(RescheduleRequest request);
        void NotifyObservers();
        void Save();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}