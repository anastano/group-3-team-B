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
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationFileHandler _fileHandler;

        private static List<Notification> _notifications;

        public NotificationRepository()
        {
            _fileHandler = new NotificationFileHandler();
            if(_notifications == null)
            {
                _notifications = _fileHandler.Load();
            }
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
        }

        public void Update(Notification notification)
        {
            Notification notificationUpdated = GetById(notification.Id);
            notificationUpdated = notification;

            Save();
        }

        public void MarkAsRead(int notificationId)
        { 
            Notification notification = _notifications.Find(n => n.Id == notificationId);
            notification.IsRead = true;
            Save();
        }
    }
}
