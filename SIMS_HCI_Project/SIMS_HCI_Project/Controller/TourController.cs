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

        private readonly List<Tour> _tours;

        public TourController()
        {
            _fileHandler = new TourFileHandler();

            _tourKeyPointController = new TourKeyPointController();
            _locationController = new LocationController();
            _tourTimeController = new TourTimeController();

            _tours = _fileHandler.Load();
        }

        public List<Tour> GetAll()
        {
            return _tours;
        }

        public Tour Save(Tour tour, Location location, List<TourKeyPoint> keyPoints, List<DateTime> tourTimes, List<string> images)
        {
            //TODO: Simplify logic. Location setter to set LocationId too and similar
            tour.Id = GenerateNextId();

            tour.Location = _locationController.Save(location);
            tour.LocationId = tour.Location.Id;

            tour.KeyPoints = _tourKeyPointController.SaveMultiple(keyPoints);
            tour.KeyPointsIds = tour.KeyPoints.Select(c => c.Id).ToList();

            tour.Images = images;

            List<TourTime> departureTimes = _tourTimeController.ConvertDateTimesToTourTimes(tour.Id, tourTimes);
            tour.DepartureTimes = _tourTimeController.SaveMultiple(departureTimes);

            tour.Guide.AddTour(tour);
            _tours.Add(tour);
            _fileHandler.Save(_tours);

            return tour;
        }

        public List<Tour> GetAllByGuideId(string id)
        {
            return _tours.FindAll(t => t.GuideId == id);
        }

        private int GenerateNextId()
        {
            if (_tours.Count == 0) return 1;
            return _tours[_tours.Count - 1].Id + 1;
        }

        public void LoadConnections()
        {
            /* TODO */
        }
    }
}
