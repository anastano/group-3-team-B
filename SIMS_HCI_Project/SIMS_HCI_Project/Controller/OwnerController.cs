using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Controller
{
    public class OwnerController : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly OwnerFileHandler _fileHandler;

        private static List<Owner> _owners;

        public OwnerController()
        {
            if (_owners == null)
            {
                _owners = new List<Owner>();
            }

            _fileHandler = new OwnerFileHandler();
            _observers = new List<IObserver>();

        }

        public List<Owner> GetAll()
        {
            return _owners;
        }

        public void Load() // load from file
        {
            _owners = _fileHandler.Load();
        }


        public void Save() //save to file
        {
            _fileHandler.Save(_owners);
        }

        public void Add(Accommodation accommodation)
        {
            //TO DO IF NEEDED
        }

        public void Remove(Accommodation accommodation)
        {
            //TO DO IF NEEDED
        }

        public void Edit(Accommodation accommodation)
        {
            //TO DO IF NEEDED
        }

        public Owner FindById(string id)
        {

            foreach (Owner owner in _owners)
            {
                if (owner.Id == id) return owner;
            }

            return null;
        }

        public void FillOwnerAccommodationList()
        {
            AccommodationController accommodationController = new AccommodationController();

            foreach(Owner owner in _owners)
            {
                owner.Accommodations.Clear();

                foreach(Accommodation accommodation in accommodationController.GetAll())
                {
                    if(accommodation.OwnerId == owner.Id)
                    {
                        owner.Accommodations.Add(accommodation);
                    }
                }
            }
        }

        public void AddAccommodationToOwner(Accommodation accommodation)
        {
            AccommodationController accommodationController = new AccommodationController();
            Owner owner = FindById(accommodation.OwnerId);
            owner.Accommodations.Add(accommodation);
        }

        public List<Accommodation> GetAccommodationByOwnerId(string ownerId)
        {
            Owner owner = FindById(ownerId);
            return owner.Accommodations;
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
