using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class NotificationRepository : ISubject, INotificationRepository
    {
        private readonly List<IObserver> _observers;
        private readonly NotificationFileHandler _fileHandler;

        private static List<Notification> _notifications;

        public NotificationRepository()
        {
            _fileHandler = new NotificationFileHandler();
            _notifications = _fileHandler.Load();

            _observers = new List<IObserver>();

        }

        public int GenerateId()
        {
            return _notifications.Count == 0 ? 1 : _notifications[_notifications.Count - 1].Id + 1;
        }

        public void Save()
        {
            _fileHandler.Save(_notifications);
        }

        public Notification GetById(int id)
        {
            return _notifications.Find(a => a.Id == id);
        }

        public List<Notification> GetAll()
        {
            return _notifications;
        }

        public List<Notification> GetUnreadByUserId(int userId)
        {
            return _notifications.FindAll(n => n.UserId == userId && n.IsRead == false);
        }

        public void Add(Notification notification)
        {
            notification.Id = GenerateId();
            _notifications.Add(notification);            
            Save();
            NotifyObservers();
        }

        public void MarkAsRead(int notificationId)
        { 
            Notification notification = _notifications.Find(n => n.Id == notificationId);
            notification.IsRead = true;
            Save();
            NotifyObservers();
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
