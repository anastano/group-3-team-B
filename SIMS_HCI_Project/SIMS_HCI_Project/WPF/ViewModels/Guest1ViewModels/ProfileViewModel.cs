using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Views.Guest1Views;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class ProfileViewModel
    {
        private NotificationService _notificationService;
        public ObservableCollection<Notification> Notifications { get; set; }
        public Guest1 Guest { get; set; }
        public ProfileView ProfileView { get; set; }
        public ProfileViewModel(ProfileView profileView, Guest1 guest)
        {
            _notificationService = new NotificationService();
            ProfileView = profileView;
            Guest = guest;
            Notifications = new ObservableCollection<Notification>(_notificationService.GetUnreadByUserId(Guest.Id));
        }
    }
}
