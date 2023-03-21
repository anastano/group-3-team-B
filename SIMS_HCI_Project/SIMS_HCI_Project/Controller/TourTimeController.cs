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
        private TourReservationController _tourReservationController = new TourReservationController();
        private static List<TourReservation> _reservations = new List<TourReservation>();

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

        public void Load()
        {
            _tourTimes = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_tourTimes);
        }

        public void Add(TourTime tourTime)
        {
            tourTime.Id = GenerateId();

            tourTime.CurrentKeyPoint = tourTime.Tour.KeyPoints.First();
            tourTime.CurrentKeyPointIndex = 0;

            _tourTimes.Add(tourTime);

            Save();
        }

        public void AddMultiple(List<TourTime> tourTimes)
        {
            foreach (TourTime tourTime in tourTimes)
            {
                tourTime.Id = GenerateId();

                tourTime.CurrentKeyPoint = tourTime.Tour.KeyPoints.First();
                tourTime.CurrentKeyPointIndex = 0;

                _tourTimes.Add(tourTime);
            }
            Save();
        }

        private int GenerateId()
        {
            if (_tourTimes.Count == 0) return 1;
            return _tourTimes[_tourTimes.Count - 1].Id + 1;
        }

        public TourTime FindById(int id)
        {
            return _tourTimes.Find(tt => tt.Id == id);
        }

        public List<TourTime> GetAllByGuideId(string id)
        {
            return _tourTimes.FindAll(tt => tt.Tour.GuideId == id);
        }

        public List<TourTime> GetTodaysByGuideId(string id)
        {
            return _tourTimes.FindAll(tt => tt.Tour.GuideId == id && tt.DepartureTime.Date == DateTime.Today);
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
                tourTime.GuestAttendances = _guestTourAttendanceController.GetAllByTourId(tourTime.Id);
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

        public void ReduceAvailablePlaces(TourTime tourTime, int requestedPartySize)
        {
            TourTime tt = FindById(tourTime.Id);

            tt.Available -= requestedPartySize;
        }

        public void ConnectAvailablePlaces()
        {
            foreach (TourTime tt in _tourTimes)
            {
                _reservations = _tourReservationController.GetAllByTourTimeId(tt.Id);
                tt.Available = tt.Tour.MaxGuests;

                if (_reservations == null)
                {
                    tt.Available = tt.Tour.MaxGuests;

                }
                else
                {
                    foreach (TourReservation tr in _reservations)
                    {
                        tt.Available -= tr.PartySize;
                    }
                }
            }
        }

        public void StartTour(TourTime tourTime)
        {
            if (tourTime.Status == TourStatus.NOT_STARTED && !HasTourInProgress(tourTime.Tour.GuideId))
            {
                tourTime.Status = TourStatus.IN_PROGRESS;
                _guestTourAttendanceController.GenerateAttendancesByTour(tourTime);
                ConnectGuestAttendances();
                Save();
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
                Save();
            }
        }

        public bool IsAtLastKeyPoint(TourTime tourTime)
        {
            return tourTime.CurrentKeyPointIndex >= tourTime.Tour.KeyPoints.Count - 1;
        }

        public void EndTour(TourTime tourTime)
        {
            tourTime.Status = TourStatus.COMPLETED;
            _guestTourAttendanceController.UpdateGuestStatusesAfterTourEnd(tourTime);
            Save();
        }
    }
}
