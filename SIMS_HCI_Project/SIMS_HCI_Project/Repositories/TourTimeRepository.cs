using SIMS_HCI_Project.Domain.DTOs;
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
                Load();
            }
        }

        private void Load()
        {
            _tourTimes = _fileHandler.Load();
        }

        private void Save()
        {
            _fileHandler.Save(_tourTimes);
        }

        private int GenerateId()
        {
            return _tourTimes.Count == 0 ? 1 : _tourTimes[_tourTimes.Count - 1].Id + 1;
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

        public List<TourTime> GetAllInDateRange(int guideId, DateRange dateRange)
        {
            return _tourTimes.FindAll(tt => tt.Tour.GuideId == guideId && dateRange.DoesOverlap(new DateRange(tt.DepartureTime, tt.Tour.Duration)));
        }

        public void Add(TourTime tourTime)
        {
            tourTime.Id = GenerateId();
            _tourTimes.Add(tourTime);

            Save();
        }

        public void AddBulk(List<TourTime> tourTimes)
        {
            foreach (TourTime tourTime in tourTimes)
            {
                tourTime.Id = GenerateId();
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
    }
}
