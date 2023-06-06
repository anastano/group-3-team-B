using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;

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
        private readonly IRegularTourRequestRepository _regularTourRequestRepository;

        public NotificationService()
        {
            _notificationRepository = Injector.Injector.CreateInstance<INotificationRepository>();
            _regularTourRequestRepository = Injector.Injector.CreateInstance<IRegularTourRequestRepository>();
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

        public void MarkAsRead(int notificationId)
        {
            Notification notification = _notificationRepository.GetById(notificationId);
            notification.IsRead = true;

            _notificationRepository.Update(notification);
        }

        public void NotifyGuestsWithSimilarRequests(Tour tour)
        {
            foreach (RegularTourRequest regularTourRequest in _regularTourRequestRepository.GetInvalidByParams(tour.LocationId, tour.Language))
            {
                string Message = "NEW TOUR - click to view details and book a tour. Tour's id: [" + tour.Id + "]";
                _notificationRepository.Add(new Notification(Message, regularTourRequest.GuestId, false, NotificationType.NEW_TOUR));
            }
        }
    }
}
