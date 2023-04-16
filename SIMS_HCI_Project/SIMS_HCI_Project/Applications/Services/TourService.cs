using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class TourService
    {
        /* NOT DONE */
        private readonly ITourRepository _tourRepository;

        public TourService()
        {
            _tourRepository = Injector.Injector.CreateInstance<ITourRepository>();
        }

        public void Load()
        {
            _tourRepository.Load();
        }

        public void Save()
        {
            _tourRepository.Save();
        }

        public List<Tour> GetAll()
        {
            return _tourRepository.GetAll();
        }

        public Tour FindById(int id)
        {
            return _tourRepository.FindById(id);
        }

        public void ConnectDepartureTimes(TourTimeService tourTimeService)
        {
            foreach (TourTime tourTime in tourTimeService.GetAll())
            {
                tourTime.Tour = FindById(tourTime.TourId);
                tourTime.Tour.DepartureTimes.Add(tourTime);
            }
        }

        public void ConnectLocations(LocationService locationService)
        {
            foreach (Tour tour in _tourRepository.GetAll())
            {
                tour.Location = locationService.GetById(tour.LocationId);
            }
        }

        public void ConnectKeyPoints(TourKeyPointService tourKeyPointService)
        {
            foreach (Tour tour in _tourRepository.GetAll())
            {
                tour.KeyPoints = tourKeyPointService.FindByIds(tour.KeyPointsIds);
            }
        }
    }
}
