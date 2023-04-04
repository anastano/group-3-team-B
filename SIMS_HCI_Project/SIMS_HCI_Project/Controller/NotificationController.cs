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
    public class NotificationController : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly NotificationFileHandler _fileHandler;

        private static List<Notification> _notifications;

        private readonly UserController _userController;

        public NotificationController()
        {
            if (_notifications == null)
            {
                _notifications = new List<Notification>();
            }

            _fileHandler = new NotificationFileHandler();
            _observers = new List<IObserver>();

            _userController = new UserController();

        }

        public List<Notification> GetAll()
        {
            return _notifications;
        }


        public void Load()
        {
            _notifications = _fileHandler.Load();
        }


        public void Save()
        {
            _fileHandler.Save(_notifications);
        }
        public int GenerateId()
        {
            if (_notifications.Count == 0)
            {
                return 1;
            }
            return _notifications[_notifications.Count - 1].Id + 1;
        }

        public void Add(Notification notification)
        {
            notification.Id = GenerateId();
            _notifications.Add(notification);
            NotifyObservers();
            Save();
        }

        public void Remove(Notification notification)
        {
            // TO DO
        }

        public void Edit(Notification notification)
        {
            // TO DO
        }

        public Notification FindById(int id)
        {
            return _notifications.Find(n => n.Id == id);
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
