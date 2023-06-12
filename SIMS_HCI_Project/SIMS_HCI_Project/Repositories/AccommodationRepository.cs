using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation = SIMS_HCI_Project.Domain.Models.Accommodation;

namespace SIMS_HCI_Project.Repositories
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private readonly AccommodationFileHandler _fileHandler;

        private static List<Accommodation> _accommodations;

        public AccommodationRepository()
        {
            _fileHandler = new AccommodationFileHandler();
            if(_accommodations == null)
            {
                _accommodations = _fileHandler.Load();
            }

        }
        public int GenerateId()
        {
            return _accommodations.Count == 0 ? 1 : _accommodations[_accommodations.Count - 1].Id + 1;
        }

        public void Save()
        {
            _fileHandler.Save(_accommodations);
        }

        public Accommodation GetById(int id)
        {
            return _accommodations.Find(a => a.Id == id);
        }

        public List<Accommodation> GetAll()
        {
            return _accommodations;
        }

        public List<Accommodation> GetByOwnerId(int ownerId)
        {
            return _accommodations.FindAll(a => a.OwnerId == ownerId);
        }

        public List<Accommodation> GetByLocationIdAndOwnerId(int locationId, int ownerId)
        {
            return _accommodations.FindAll(a => a.LocationId == locationId && a.OwnerId == ownerId);
        }
        public List<Accommodation> GetAllSortedBySuperFlag()
        {
            return _accommodations.OrderByDescending(a => a.Owner.SuperFlag).ToList();
        }
        public List<Accommodation> Search(string name, string country, string city, string type, string guestsNumber, string reservationDays)
        {
            var filtered = from accommodation in _accommodations
                           where (string.IsNullOrEmpty(name) || accommodation.Name.ToLower().Contains(name.ToLower()))
                           && (string.IsNullOrEmpty(country) || accommodation.Location.Country.ToLower().Contains(country.ToLower()))
                           && (string.IsNullOrEmpty(city) || accommodation.Location.City.ToLower().Contains(city.ToLower()))
                           && (string.IsNullOrEmpty(type) || Enum.GetName(accommodation.Type).Equals(type))
                           && (string.IsNullOrEmpty(guestsNumber) || int.Parse(guestsNumber) <= accommodation.MaxGuests)
                           && (string.IsNullOrEmpty(reservationDays) || int.Parse(reservationDays) >= accommodation.MinimumReservationDays)
                           orderby accommodation.Owner.SuperFlag descending
                           select accommodation;
            return filtered.ToList();
        }
        public List<string> GetImages(int id)
        {
            return _accommodations.Find(a => a.Id == id).Images;
        }

        public void Add(Accommodation accommodation)
        {
            accommodation.Id = GenerateId();
            _accommodations.Add(accommodation);
            Save();
        }

        public void Delete(Accommodation accommodation)
        {
            _accommodations.Remove(accommodation);
            Save();
        }

    }
}
