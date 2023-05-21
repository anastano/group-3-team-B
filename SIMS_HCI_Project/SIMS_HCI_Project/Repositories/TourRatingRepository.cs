using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;
using SIMS_HCI_Project.Observer;

namespace SIMS_HCI_Project.Repositories
{
    public class TourRatingRepository : ITourRatingRepository
    {
        private readonly TourRatingFileHandler _fileHandler;
        private static List<TourRating> _ratings;

        public TourRatingRepository()
        {
            _fileHandler = new TourRatingFileHandler();
            if (_ratings == null)
            {
                Load();
            }
        }

        private void Load()
        {
            _ratings = _fileHandler.Load();
        }

        private void Save()
        {
            _fileHandler.Save(_ratings);
        }

        private int GenerateId()
        {
            return _ratings.Count == 0 ? 1 : _ratings[_ratings.Count - 1].Id + 1;
        }

        public TourRating GetById(int id)
        {
            return _ratings.Find(r => r.Id == id);
        }

        public List<TourRating> GetAll()
        {
            return _ratings;
        }

        public List<TourRating> GetAllByTourId(int tourTimeId)
        {
            return _ratings.FindAll(r => r.Attendance.TourReservation.TourTimeId == tourTimeId);
        }

        public void Add(TourRating rating)
        {
            rating.Id = GenerateId();
            _ratings.Add(rating);

            Save();
        }

        public void Update(TourRating tourRating)
        {
            TourRating toUpdate = GetById(tourRating.Id);
            toUpdate = tourRating;

            Save();
        }

        public bool IsRated(int id)
        {
            return _ratings.Any(r => r.Attendance.TourReservationId == id);
        }
    }
}
