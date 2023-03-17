using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Threading;


namespace SIMS_HCI_Project.Controller
{
    public class AccommodationController: ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly AccommodationFileHandler _fileHandler;

        private static List<Accommodation> _accommodations;

        private readonly OwnerController _ownerController;
        private readonly LocationController _locationController;

        public AccommodationController() 
        {
            if (_accommodations == null)
            {
                _accommodations= new List<Accommodation>();
            }

            _fileHandler = new AccommodationFileHandler();
            _observers = new List<IObserver>();

            _ownerController = new OwnerController();
            _locationController = new LocationController();

        }

        public List<Accommodation> GetAll()
        {
            return _accommodations;
        }


        public void Load()
        {
            _accommodations = _fileHandler.Load();
        }


        public void Save()
        {
            _fileHandler.Save(_accommodations);
        }


        public void ConnectAccommodationsWithLocations(LocationController locationController)
        {
            foreach (Accommodation accommodation in _accommodations)
            {
                accommodation.Location = locationController.FindById(accommodation.LocationId);
            }
        }

        public int GenerateId()
        {
            if (_accommodations.Count == 0)
            {
                return 1;
            }
            return _accommodations[_accommodations.Count - 1].Id + 1;
        }

        public void Add(Accommodation accommodation) //adds accommodation to accommodation list and to corresponding owner list
        {
            _accommodations.Add(accommodation);
            _ownerController.AddAccommodationToOwner(accommodation);
            NotifyObservers();
            Save();
        }

        public void Remove(Accommodation accommodation)
        {
            // TO DO
        }

        public void Edit(Accommodation accommodation) 
        {
            // TO DO
        }

        public Accommodation FindById(int id) 
        {
            return _accommodations.Find(a => a.Id == id);
        }


        public List<Accommodation> Search(string name, string country, string city, string type, int maxGuests, int reservationDays)
        {

            var filtered = from _accommodation in _accommodations
                           where (string.IsNullOrEmpty(name) || _accommodation.Name.ToLower().Contains(name.ToLower()))
                           && (string.IsNullOrEmpty(country) || _accommodation.Location.Country.ToLower().Contains(country.ToLower()))
                           && (string.IsNullOrEmpty(city) || _accommodation.Location.City.ToLower().Contains(city.ToLower()))
                           && (string.IsNullOrEmpty(type) || Accommodation.ConvertAccommodationTypeToString(_accommodation.Type).Equals(type))
                           && (maxGuests == 0 || maxGuests <= _accommodation.MaxGuests)
                           && (reservationDays == 0 || reservationDays >= _accommodation.MinimumReservationDays)
                           select _accommodation;

            return filtered.ToList();
        }

        public void Register(Accommodation accommodation, Location location)
        {
            accommodation.Id = GenerateId();

            accommodation.Location = _locationController.Save(location);
            accommodation.LocationId = location.Id;

            Add(accommodation);
        }

        public List<string> GetAllImages(int id)
        {
            return _accommodations.Find(a => a.Id == id).Images;
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }
    }
}
