﻿using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
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
            _notificationRepository.MarkAsRead(notificationId);
        }

        public void NotifyGuestsWithSimilarRequests(Tour tour)
        {
            foreach (RegularTourRequest regularTourRequest in _regularTourRequestRepository.GetInvalidByParams(tour.LocationId, tour.Language))
            {
                string Message = "Created new tour which fulfills some of your requirements from your unfulfilled requests. Tour is created with id: [" + tour.Id + "]. Click to see details";
                _notificationRepository.Add(new Notification(Message, regularTourRequest.GuestId, false, NotificationType.NEW_TOUR));
            }
        }
    }
}
