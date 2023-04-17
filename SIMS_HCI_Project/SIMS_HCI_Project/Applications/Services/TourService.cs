using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Domain.Models;
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

        public List<Tour> GetAll()
        {
            return _tourRepository.GetAll();
        }

        public Tour GetById(int id)
        {
            return _tourRepository.GetById(id);
        }

        #region Connections
        public void LoadConnections()
        {
            ConnectDepartureTimes();
            ConnectLocations();
            ConnectKeyPoints();
        }

        public void ConnectDepartureTimes()
        {
            foreach (TourTime tourTime in _tourTimeRepository.GetAll())
            {
                tourTime.Tour = GetById(tourTime.TourId);
                tourTime.Tour.DepartureTimes.Add(tourTime);
            }
        }

        public void ConnectLocations()
        {
            foreach (Tour tour in _tourRepository.GetAll())
            {
                tour.Location = _locationRepository.GetById(tour.LocationId);
            }
        }

        public void ConnectKeyPoints()
        {
            foreach (Tour tour in _tourRepository.GetAll())
            {
                tour.KeyPoints = _tourKeyPointRepository.GetByIds(tour.KeyPointsIds);
            }
        }
        #endregion
    }
}
