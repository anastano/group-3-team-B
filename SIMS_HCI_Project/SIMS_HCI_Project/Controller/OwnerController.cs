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

        public List<Owner> GetList()
        {
            return _owners;
        }

        public void LoadList() // load from file
        {
            _owners = _fileHandler.Load();
        }


        public void SaveList() //save to file
        {
            _fileHandler.Save(_owners);
        }

        public void Add(Accommodation accommodation)
        {
            //TO DO
        }

        public void Remove(Accommodation accommodation)
        {
            // TO DO
        }

        public void Edit(Accommodation accommodation)
        {
            // TO DO
        }

        public Owner FindById(string id)
        {

            foreach (Owner o in _owners)
            {
                if (o.Id == id) return o;
            }

            return null;
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
