using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Repositories;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;

namespace SIMS_HCI_Project.Applications.Services
{
    public class TourRatingService
    {
        private readonly ITourRatingRepository _tourRatingRepository;

        public TourRatingService()
        {
            _tourRatingRepository = Injector.Injector.CreateInstance<ITourRatingRepository>();
        }

        public void Save()
        {
            _tourRatingRepository.Save();
        }

        public void Load()
        {
            _tourRatingRepository.Load();
        }

        public TourRating GetById(int id)
        {
            return _tourRatingRepository.GetById(id);
        }

        public List<TourRating> GetAll()
        {
            return _tourRatingRepository.GetAll();
        }

        public void Add(TourRating tourRating)
        {
            _tourRatingRepository.Add(tourRating);
        }

        public void NotifyObservers()
        {
            _tourRatingRepository.NotifyObservers();
        }

        public void Subscribe(IObserver observer)
        {
            _tourRatingRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _tourRatingRepository.Unsubscribe(observer);
        }
    }
}
