using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class TourRepository : ITourRepository
    {
        private TourFileHandler _fileHandler;

        private static List<Tour> _tours;

        public TourRepository()
        {
            _fileHandler = new TourFileHandler();

            if (_tours == null)
            {
                Load();
            }
        }

        public List<Tour> GetAll()
        {
            return _tours;
        }

        public void Load()
        {
            _tours = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_tours);
        }

        public Tour GetById(int id)
        {
            return _tours.Find(t => t.Id == id);
        }

    }
}
