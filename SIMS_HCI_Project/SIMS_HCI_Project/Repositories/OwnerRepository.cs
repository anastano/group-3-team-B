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
            if (_owners == null)
            {
                _owners = new List<Owner>();
            }

            _fileHandler = new OwnerFileHandler();
        }

        public Owner FindById(int id)
        {
            return _owners.Find(o => o.Id == id);
        }

        public List<Owner> GetAll()
        {
            return _owners;
        }

        public void Load()
        {
            _owners = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_owners);
        }

    }
}
