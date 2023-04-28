﻿using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class TourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly ITourTimeRepository _tourTimeRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ITourKeyPointRepository _tourKeyPointRepository;

        public TourService()
        {
            _tourRepository = Injector.Injector.CreateInstance<ITourRepository>();
            _tourTimeRepository = Injector.Injector.CreateInstance<ITourTimeRepository>();
            _locationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            _tourKeyPointRepository = Injector.Injector.CreateInstance<ITourKeyPointRepository>();
        }

        public List<Tour> GetAllTourInformation()
        {
            return _tourRepository.GetAll();
        }

        public List<Tour> GetAllTourInformationByGuide(int guideId)
        {
            return _tourRepository.GetAllByGuide(guideId);
        }

        public Tour GetTourInformation(int tourId)
        {
            return _tourRepository.GetById(tourId);
        }

        public TourTime GetTourInstance(int tourTimeId)
        {
            return _tourTimeRepository.GetById(tourTimeId);
        }

        public List<TourTime> GetAllTourInstances()
        {
            return _tourTimeRepository.GetAll();
        }

        public List<TourTime> GetToursByGuide(int guideId)
        {
            return _tourTimeRepository.GetAllByGuideId(guideId);
        }

        public List<TourTime> GetTodaysToursByGuide(int guideId)
        {
            List<TourTime> guideTours = GetToursByGuide(guideId);
            return guideTours.Where(t => t.DepartureTime.Date == DateTime.Today).ToList();
        }

        public List<Tour> Search(string country, string city, int duration, string language, int guestsNum)
        {
            return _tourRepository.Search(country, city, duration, language, guestsNum);
        }

        public List<Tour> Search(string city, string country)
        {
            return _tourRepository.Search(city, country);
        }

        public TourTime GetActiveTour(int guideId)
        {
            List<TourTime> guideTours = GetToursByGuide(guideId);
            return guideTours.Find(t => t.Status == TourStatus.IN_PROGRESS);
        }

        public void Add(Tour tour)
        {
            tour.Location = _locationRepository.GetOrAdd(tour.Location);
            tour.LocationId = tour.Location.Id;

            _tourKeyPointRepository.AddMultiple(tour.KeyPoints);
            tour.KeyPointsIds = tour.KeyPoints.Select(c => c.Id).ToList();

            _tourRepository.Add(tour);

            _tourTimeRepository.AssignTourToTourTimes(tour, tour.DepartureTimes);
            _tourTimeRepository.AddMultiple(tour.DepartureTimes);
        }
    }
}
