using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class RatingGivenByGuestRepository : IRatingGivenByGuestRepository
    {

        private readonly RatingGivenByGuestFileHandler _fileHandler;

        private static List<RatingGivenByGuest> _ratings;

        public RatingGivenByGuestRepository()
        {
            _fileHandler = new RatingGivenByGuestFileHandler();
            if (_ratings == null)
            {
                _ratings = _fileHandler.Load();
            }
        }
        public int GenerateId()
        {
            return _ratings.Count == 0 ? 1 : _ratings[_ratings.Count - 1].Id + 1;
        }
        public void Save()
        {
            _fileHandler.Save(_ratings);
        }
        public RatingGivenByGuest GetById(int id)
        {
            return _ratings.Find(r => r.Id == id);
        }
        public RatingGivenByGuest GetByReservationId(int reservationId)
        {
            return _ratings.Find(r => r.ReservationId == reservationId);
        }
        public List<RatingGivenByGuest> GetAll()
        {
            return _ratings;
        }

        public List<RatingGivenByGuest> GetByOwnerId(int ownerId)
        {
            return _ratings.FindAll(r => r.Reservation.Accommodation.OwnerId == ownerId);
        }
        public bool IsReservationRated(int reservationId)
        {
            return _ratings.Any(r => r.ReservationId == reservationId);
        }
        public void Add(RatingGivenByGuest rating)
        {
            rating.Id = GenerateId();
            _ratings.Add(rating);
            Save();
        }

    }
}
