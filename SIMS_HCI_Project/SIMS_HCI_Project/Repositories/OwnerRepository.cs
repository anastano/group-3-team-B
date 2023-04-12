using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly OwnerFileHandler _fileHandler;

        private static List<Owner> _owners;

        public OwnerRepository()
        {
            _fileHandler = new OwnerFileHandler();
            _owners = _fileHandler.Load();
        }

        public Owner GetById(int id)
        {
            return _owners.Find(o => o.Id == id);
        }

        public List<Owner> GetAll()
        {
            return _owners;
        }

    }
}
