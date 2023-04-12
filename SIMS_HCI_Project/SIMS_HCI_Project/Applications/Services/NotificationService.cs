using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class NotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService()
        {
            _notificationRepository = Injector.Injector.CreateInstance<INotificationRepository>();
        }

        public void Save()
        {
            _notificationRepository.Save();
        }

        public Notification GetById(int id)
        {
            return _notificationRepository.GetById(id);
        }

        public List<Notification> GetAll()
        {
            return _notificationRepository.GetAll();
        }

        public List<Notification> GetUnreadByUserId(int userId)
        {
            return _notificationRepository.GetUnreadByUserId(userId);
        }

        public void Add(Notification notification)
        {
            _notificationRepository.Add(notification);
        }

        public  void MarkAsRead(int notificationId)
        { 
            _notificationRepository.MarkAsRead(notificationId);
        }

        public void NotifyObservers()
        {
            _notificationRepository.NotifyObservers();
        }

        public void Subscribe(IObserver observer)
        {
            _notificationRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _notificationRepository.Unsubscribe(observer);
        }
    }
}
