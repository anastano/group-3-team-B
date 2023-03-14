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

        private TourKeyPointController _tourKeyPointController;
        private LocationController _locationController;
        private TourTimeController _tourTimeController;

        private static List<Tour> _tours;

        public TourController()
        {
            _fileHandler = new TourFileHandler();

            _tourKeyPointController = new TourKeyPointController();
            _locationController = new LocationController();
            _tourTimeController = new TourTimeController();

            Load();
        }

        public List<Tour> GetAll()
        {
            return _tours;
        }

        public Tour Save(Tour tour)
        {
            tour.Id = GenerateId();

            tour.Location = _locationController.Save(tour.Location);
            tour.LocationId = tour.Location.Id;

            tour.KeyPoints = _tourKeyPointController.SaveMultiple(tour.KeyPoints);
            tour.KeyPointsIds = tour.KeyPoints.Select(c => c.Id).ToList();

            _tourTimeController.AssignTourToTourTimes(tour, tour.DepartureTimes);
            tour.DepartureTimes = _tourTimeController.SaveMultiple(tour.DepartureTimes);

            tour.Guide.AddTourTimes(tour.DepartureTimes);
            _tours.Add(tour);
            _fileHandler.Save(_tours);

            return tour;
        }

        public List<Tour> GetAllByGuideId(string id)
        {
            return _tours.FindAll(t => t.GuideId == id);
        }

        private int GenerateId()
        {
            if (_tours.Count == 0) return 1;
            return _tours[_tours.Count - 1].Id + 1;
        }

        public void Load()
        {
            _tours = _fileHandler.Load();
        }


        public Tour FindById(int id)
        {
            return _tours.Find(t => t.Id == id);
        }

        public void ConnectToursLocations()
        {
            foreach(Tour tour in _tours)
            {
                tour.Location = _locationController.FindById(tour.LocationId);
            }
        }

        public void ConnectDepartureTimes()
        {
            /* TODO: Organize startup load better */
            foreach(TourTime tourTime in _tourTimeController.GetAll())
            {
                tourTime.Tour = FindById(tourTime.TourId);
                tourTime.Tour.DepartureTimes.Add(tourTime);
            }
        }

        public List<Tour> Search(string country, string city, int duration, string language, int guestsNum)
        {
            var filtered = from _tour in _tours
                           where (string.IsNullOrEmpty(country) || _tour.Location.Country.ToLower().Contains(country.ToLower()))
                           && (string.IsNullOrEmpty(city) || _tour.Location.City.ToLower().Contains(city.ToLower()))
                           && (duration == 0 || duration >= _tour.Duration)
                           && (guestsNum == 0 || guestsNum <= _tour.MaxGuests)
                           && (string.IsNullOrEmpty(language) || _tour.Language.ToLower().Contains(language.ToLower()))
                           select _tour;

            return filtered.ToList();
        }
    }
}
