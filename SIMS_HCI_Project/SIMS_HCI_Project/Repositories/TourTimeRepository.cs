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
    public class TourTimeRepository : ITourTimeRepository
    {
        private TourTimeFileHandler _fileHandler;

        private static List<TourTime> _tourTimes;
        private TourVoucherController _tourVoucherController;
        private TourReservationController _tourReservationController = new TourReservationController();

        public TourTimeRepository()
        {
            _fileHandler = new TourTimeFileHandler();
            _tourVoucherController = new TourVoucherController();

            if (_tourTimes == null)
            {
                _tourTimes = _fileHandler.Load();
            }
        }

        public void Load()
        {
            _tourTimes = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_tourTimes);
        }

        public TourTime GetById(int id)
        {
            return _tourTimes.Find(tt => tt.Id == id);
        }

        public List<TourTime> GetAll()
        {
            return _tourTimes;
        }

        public List<TourTime> GetAllByGuideId(int id)
        {
            return _tourTimes.FindAll(tt => tt.Tour.GuideId == id);
        }

        public void CancelTour(TourTime tourTime)
        {
            tourTime.Status = TourStatus.CANCELED;
            Save();
        }
    }
}
