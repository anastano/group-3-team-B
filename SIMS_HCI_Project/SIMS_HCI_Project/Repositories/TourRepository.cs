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
    public class TourRepository : ITourRepository
    {
        private TourFileHandler _fileHandler;

        private static List<Tour> _tours;

        public TourRepository()
        {
            _fileHandler = new TourFileHandler();

            if (_tours == null)
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

        public Tour GetById(int id)
        {
            return _tours.Find(t => t.Id == id);
        }

        public List<Tour> GetAllByGuide(int guideId)
        {
            return _tours.FindAll(t => t.GuideId == guideId);
        }

        public void Add(Tour tour)
        {
            tour.Id = GenerateId();
            _tours.Add(tour);

            Save();
        }

        private int GenerateId()
        {
            return _tours.Count == 0 ? 1 : _tours[_tours.Count - 1].Id + 1;
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
