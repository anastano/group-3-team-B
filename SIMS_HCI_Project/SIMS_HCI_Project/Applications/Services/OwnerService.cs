using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class OwnerService
    {
        private readonly IOwnerRepository _ownerRepository;

        public OwnerService()
        {
            _ownerRepository = Injector.Injector.CreateInstance<IOwnerRepository>();
        }

        public Owner FindById(int id)
        {
            return _ownerRepository.FindById(id);
        }

        public List<Owner> GetAll()
        {
            return _ownerRepository.GetAll();
        }

        public void Load()
        {
           _ownerRepository.Load();
        }

        public void Save()
        {
            _ownerRepository.Save();
        }

    }
}
