using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IRenovationRepository
    {
        void Add(Renovation renovation);
        void Delete(Renovation renovation);
        List<Renovation> GetAll();
        List<Renovation> GetByAccommodationId(int accommodationId);
        Renovation GetById(int id);
        List<Renovation> GetByOwnerId(int ownerId);
    }
}