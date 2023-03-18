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
        private TourReservationController _tourReservationController = new TourReservationController();
        private static List<TourReservation> _reservations = new List<TourReservation>();


        public TourTimeController()
        {
            if (_tourTimes == null)
            {
                _fileHandler = new TourTimeFileHandler();
                _tourTimes = _fileHandler.Load();
            }
        }

        public void ReduceAvailable(TourTime tourTime, int requestedPartySize)
        {
            TourTime tt = FindById(tourTime.Id);

            tt.Available -= requestedPartySize; 
        }

        public void ConnectAvailablePlaces()
        {
            foreach (TourTime tt in _tourTimes)
            {
                _reservations = _tourReservationController.GetReservationsByTourTime(tt.Id);
                tt.Available = tt.Tour.MaxGuests;
                 
                if(_reservations == null)
                {
                    tt.Available = tt.Tour.MaxGuests;

                }
                else
                {
                    foreach(TourReservation tr in _reservations)
                    {
                        tt.Available -= tr.PartySize;
                    }
                }
            }
        }

        public TourTime FindById(int id)
        {
            return _tourTimes.Find(tt => tt.Id == id);
        }
        public List<TourTime> GetAll()
        {
            return _tourTimes;
        }

        public TourTime Save(TourTime tourTime)
        {
            tourTime.Id = GenerateId();
            tourTime.CurrentKeyPoint = tourTime.Tour.KeyPoints.First();

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

        public List<TourTime> GetAllByGuideId(string id)
        {
            return _tourTimes.FindAll(tt => tt.Tour.GuideId == id);
        }

        private int GenerateId()
        {
            if (_tourTimes.Count == 0) return 1;
            return _tourTimes[_tourTimes.Count - 1].Id + 1;
        }

        public List<TourTime> ConvertDateTimesToTourTimes(int tourId, List<DateTime> tourDateTimes)
        {
            List<TourTime> departureTimes = new List<TourTime>();
            foreach(DateTime dt in tourDateTimes)
            {
                departureTimes.Add(new TourTime(tourId, dt));
            }

            return departureTimes;
        }
    }
}
