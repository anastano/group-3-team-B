using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Controller
{
    public class TourTimeController
    {
        private TourTimeFileHandler _fileHandler;

        private static List<TourTime> _tourTimes;
        private GuestTourAttendanceController _guestTourAttendanceController;

        public TourTimeController()
        {
            _fileHandler = new TourTimeFileHandler();
            _guestTourAttendanceController = new GuestTourAttendanceController();

            if (_tourTimes == null)
            {
                _tourTimes = _fileHandler.Load();
            }
        }

        public List<TourTime> GetAll()
        {
            return _tourTimes;
        }

        public TourTime Save(TourTime tourTime)
        {
            tourTime.Id = GenerateId();
            tourTime.CurrentKeyPoint = tourTime.Tour.KeyPoints.First();
            tourTime.CurrentKeyPointIndex = 0;

            _tourTimes.Add(tourTime);
            _fileHandler.Save(_tourTimes);

            return tourTime;
        }

        public List<TourTime> SaveMultiple(List<TourTime> tourTimes)
        {
            List<TourTime> result = new List<TourTime>();

            foreach (TourTime tt in tourTimes)
            {
                result.Add(Save(tt));
            }

            return result;
        }

        public void AssignTourToTourTimes(Tour tour, List<TourTime> tourTimes)
        {
            foreach (TourTime tourTime in tourTimes)
            {
                tourTime.TourId = tour.Id;
                tourTime.Tour = tour;
            }
        }

        public void ConnectGuestAttendances()
        {
            foreach (TourTime tourTime in _tourTimes)
            {
                tourTime.GuestAttendances = _guestTourAttendanceController.GetByTourId(tourTime.Id);
            }
        }

        public void ConnectCurrentKeyPoints()
        {
            foreach (TourTime tourTime in _tourTimes)
            {
                tourTime.CurrentKeyPoint = tourTime.Tour.KeyPoints[tourTime.CurrentKeyPointIndex];
            }
        }

        public void LoadConnections()
        {
            ConnectGuestAttendances();
            ConnectCurrentKeyPoints();
        }

        public List<TourTime> GetAllByGuideId(string id)
        {
            return _tourTimes.FindAll(tt => tt.Tour.GuideId == id);
        }

        public List<TourTime> GetTodaysByGuideId(string id)
        {
            return _tourTimes.FindAll(tt => tt.Tour.GuideId == id && tt.DepartureTime.Date == DateTime.Today);
        }

        private int GenerateId()
        {
            if (_tourTimes.Count == 0) return 1;
            return _tourTimes[_tourTimes.Count - 1].Id + 1;
        }

        public void StartTour(TourTime tourTime)
        {
            if (tourTime.Status == TourStatus.NOT_STARTED && !HasTourInProgress(tourTime.Tour.GuideId))
            {
                tourTime.Status = TourStatus.IN_PROGRESS;
                _guestTourAttendanceController.GenerateByTour(tourTime);
                ConnectGuestAttendances();
                _fileHandler.Save(_tourTimes);
            }
        }

        public bool HasTourInProgress(string guideId)
        {
            return _tourTimes.Any(tt => tt.Tour.GuideId.Equals(guideId) && tt.Status == TourStatus.IN_PROGRESS);
        }

        public void MoveToNextKeyPoint(TourTime tourTime)
        {
            if (IsAtLastKeyPoint(tourTime))
            {
                EndTour(tourTime);
            }
            else
            {
                tourTime.CurrentKeyPointIndex++;
                tourTime.CurrentKeyPoint = tourTime.Tour.KeyPoints[tourTime.CurrentKeyPointIndex];
                _fileHandler.Save(_tourTimes);
            }
        }

        public bool IsAtLastKeyPoint(TourTime tourTime)
        {
            return tourTime.CurrentKeyPointIndex >= tourTime.Tour.KeyPoints.Count - 1;
        }

        public void EndTour(TourTime tourTime)
        {
            tourTime.Status = TourStatus.COMPLETED;
            _guestTourAttendanceController.UpdateAfterTourEnd(tourTime);
            _fileHandler.Save(_tourTimes);
        }
    }
}
