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
 
        public TourTimeRepository()
        {
            _fileHandler = new TourTimeFileHandler();

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

        public List<TourTime> GetAllByGuideId(int guideId)
        {
            return _tourTimes.FindAll(tt => tt.Tour.GuideId == guideId);
        }

        public void Add(TourTime tourTime)
        {
            tourTime.Id = GenerateId();
            tourTime.CurrentKeyPointIndex = 0; // move to model #New
            _tourTimes.Add(tourTime);

            Save();
        }

        public void AddMultiple(List<TourTime> tourTimes)
        {
            foreach (TourTime tourTime in tourTimes)
            {
                tourTime.Id = GenerateId();
                tourTime.CurrentKeyPointIndex = 0; // move to model #New
                _tourTimes.Add(tourTime);
            }
            Save();
        }

        public void Update(TourTime tourTime)
        {
            TourTime tourTimeUpdated = GetById(tourTime.Id);
            tourTimeUpdated = tourTime;

            Save();
        }

        public bool HasTourInProgress(int guideId)
        {
            return _tourTimes.Any(tt => tt.Tour.GuideId == guideId && tt.Status == TourStatus.IN_PROGRESS);
        }

        private int GenerateId()
        {
            return _tourTimes.Count == 0 ? 1 : _tourTimes[_tourTimes.Count - 1].Id + 1;
        }

        public void CheckAndUpdateStatus()
        {
            DateTime now = DateTime.Now;
            foreach (TourTime tourTime in _tourTimes)
            {
                if (tourTime.DepartureTime < now)
                {
                    tourTime.Status = TourStatus.COMPLETED;
                    Save();
                }
            }
        }

        public void AssignTourToTourTimes(Tour tour, List<TourTime> tourTimes)
        {
            foreach (TourTime tourTime in tourTimes)
            {
                tourTime.TourId = tour.Id;
                tourTime.Tour = tour;
            }
        }
    }
}
