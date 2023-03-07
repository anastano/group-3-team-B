using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Controller
{
    public class TourController
    {
        private TourFileHandler _fileHandler;

        private readonly List<Tour> _tours;

        public TourController()
        {
            _fileHandler = new TourFileHandler();
            _tours = _fileHandler.Load();
        }

        public List<Tour> GetAll()
        {
            return _tours;
        }

        public Tour Save(Tour tour)
        {
            tour.Id = GenerateNextId();
            _tours.Add(tour);
            _fileHandler.Save(_tours);
            return tour;
        }

        private int GenerateNextId()
        {
            if (_tours.Count == 0) return 1;
            return _tours[_tours.Count - 1].Id + 1;
        }
    }
}
