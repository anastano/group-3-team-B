using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Views.Guest1Views;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class ProfileViewModel : INotifyPropertyChanged
    {
        private NotificationService _notificationService;
        private RatingGivenByOwnerService _ratingService;
        private SuperGuestTitleService _titleService;
        private UserService _userService;
        public ObservableCollection<Notification> Notifications { get; set; }
        public Guest1 Guest { get; set; }
        public SuperGuestTitle SuperGuestTitle { get; set; }
        public string FullName { get; set; }
        public string YesNoMessage { get; set; }
        public int BonusPoints { get; set; }
        private double _averageRate;
        public double AverageRate
        {
            get => _averageRate;
            set
            {
                if (value != _averageRate)
                {
                    _averageRate = value;
                    OnPropertyChanged(nameof(_averageRate));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ProfileViewModel(Guest1 guest)
        {
            _notificationService = new NotificationService();
            _ratingService = new RatingGivenByOwnerService();
            _titleService = new SuperGuestTitleService();
            _userService = new UserService();
            Guest = guest;
            SuperGuestTitle = _titleService.GetGuestActiveTitle(Guest.Id);
            if(SuperGuestTitle == null)
            {
                SuperGuestTitle = new SuperGuestTitle();
            }
            FullName = Guest.GetFullName();
            Notifications = new ObservableCollection<Notification>(_notificationService.GetUnreadByUserId(Guest.Id));
            //potencijalna poruka ako nema obavjestenja
            AverageRate = _ratingService.GetGuestAverageRate(Guest);
            
        }
    }
}
