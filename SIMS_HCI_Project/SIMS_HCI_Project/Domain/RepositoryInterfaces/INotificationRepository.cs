using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface INotificationRepository
    {
        List<Notification> GetAll();
        Notification GetById(int id);
        List<Notification> GetUnreadByUserId(int userId);

        void MarkAsRead(int notificationId); // to Update #New

        void Add(Notification notification);
        void Update(Notification notification);

        void NotifyObservers();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}