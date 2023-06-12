using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface IComplexTourRequestRepository
    {
        ComplexTourRequest GetById(int id);
        List<ComplexTourRequest> GetAll();
        List<ComplexTourRequest> GetAllByGuestId(int guestId);

        void Add(ComplexTourRequest request);
        void Update(ComplexTourRequest request);
    }
}