using SIMS_HCI_Project.Domain.Models;

using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface INotificationRepository
    {
        List<Notification> GetAll();
        Notification GetById(int id);
        List<Notification> GetUnreadByUserId(int userId);

        void Add(Notification notification);
        void Update(Notification notification);
    }
}