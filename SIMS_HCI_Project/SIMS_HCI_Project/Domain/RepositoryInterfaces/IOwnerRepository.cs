using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IOwnerRepository
    {
        Owner FindById(int id);
        List<Owner> GetAll();
        void Load();
        void Save();
    }
}