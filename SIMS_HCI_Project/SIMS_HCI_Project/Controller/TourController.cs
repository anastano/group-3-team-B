﻿using SIMS_HCI_Project.FileHandler;
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

            if(_tours == null)
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

        public void Add(Tour tour)
        {
            tour.Id = GenerateId();

            tour.Location = _locationController.Save(tour.Location);
            tour.LocationId = tour.Location.Id;

            _tourKeyPointController.AddMultiple(tour.KeyPoints);
            tour.KeyPointsIds = tour.KeyPoints.Select(c => c.Id).ToList();

            _tourTimeController.AssignTourToTourTimes(tour, tour.DepartureTimes);
            _tourTimeController.AddMultiple(tour.DepartureTimes);

            tour.Guide.Tours.Add(tour);
            tour.Guide.AddTodaysTourTimes(tour.DepartureTimes);

            _tours.Add(tour);

            Save();
        }

        private int GenerateId()
        {
            if (_tours.Count == 0) return 1;
            return _tours[_tours.Count - 1].Id + 1;
        }

        public Tour FindById(int id)
        {
            return _tours.Find(t => t.Id == id);
        }

        public List<Tour> GetAllByGuideId(int id)
        {
            return _tours.FindAll(t => t.GuideId == id);
        }

        public void ConnectLocations()
        {
            foreach(Tour tour in _tours)
            {
                tour.Location = _locationController.FindById(tour.LocationId);
            }
        }

        public void ConnectKeyPoints()
        {
            foreach (Tour tour in _tours)
            {
                tour.KeyPoints = _tourKeyPointController.FindByIds(tour.KeyPointsIds);
            }
        }

        public void ConnectDepartureTimes()
        {
            foreach(TourTime tourTime in _tourTimeController.GetAll())
            {
                tourTime.Tour = FindById(tourTime.TourId);
                tourTime.Tour.DepartureTimes.Add(tourTime);
            }
        }

        public void LoadConnections()
        {
            ConnectLocations();
            ConnectKeyPoints();
            ConnectDepartureTimes();
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

        public List<Tour> Search(string city, string country)
        {
            var result = from _tour in _tours
                         where (_tour.Location.Country.ToLower().Contains(country.ToLower()))
                         && (_tour.Location.City.ToLower().Contains(city.ToLower()))
                         select _tour;
            return result.ToList();
        }

        public List<string> GetImages(int id)
        {
            return _tours.Find(t => t.Id == id).Images;
        }
    }
}
