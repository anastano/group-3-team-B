using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class ForumsViewModel : INotifyPropertyChanged
    {
        private NavigationService _navigationService;
        private readonly ForumService _forumService;
        private readonly NotificationService _notificationService;
        public Forum SelectedForum { get; set; }
        public Guest1 Guest { get; set; }
        public ObservableCollection<Forum> GuestForums { get; set; }
        public ObservableCollection<Forum> OtherForums { get; set; }
        public RelayCommand ShowMoreCommand { get; set; }
        public RelayCommand CloseForumCommand { get; set; }
        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                if (value != _selectedTabIndex)
                {
                    _selectedTabIndex = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ForumsViewModel(Guest1 guest, NavigationService navigationService, int selectedTabIndex)
        {
            _navigationService = navigationService;
            _forumService = new ForumService();
            _notificationService = new NotificationService();
            Guest = guest;
            GuestForums = new ObservableCollection<Forum>(_forumService.GetByGuestId(Guest.Id));
            OtherForums = new ObservableCollection<Forum>(_forumService.GetForumsExcludingGuests(Guest.Id));
            InitCommands();
            SelectedTabIndex = selectedTabIndex;

        }
        private MessageBoxResult ConfirmClose()
        {
            string sMessageBoxText = $"This forum will be closed, are you sure?";
            string sCaption = "Close Confirm";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }
        public void ExecutedShowMoreCommand(object obj)
        {
            /*if (result == MessageBoxResult.Yes)
            {
                _reservationService.CancelReservation(_notificationService, SelectedReservation);
                Update();
            }
            if (SelectedReservation != null)
            {
                _navigationService.Navigate(new RatingReservationViewModel(SelectedReservation, _navigationService), "Review");
            }*/
        }
        public void ExecutedCloseForumCommand(object obj)
        {
            
            if (SelectedForum != null)
            {
                MessageBoxResult result = ConfirmClose();
                if (result == MessageBoxResult.Yes)
                {
                    _forumService.CloseForum(SelectedForum.Id);
                    UpdateForums();
                }
            }
        }
        public bool CanExecute(object obj)
        {
            return true;
        }
        public void InitCommands()
        {
            ShowMoreCommand = new RelayCommand(ExecutedShowMoreCommand, CanExecute);
            CloseForumCommand = new RelayCommand(ExecutedCloseForumCommand, CanExecute);
        }
        public void UpdateForums()
        {
            GuestForums.Clear();
            foreach (Forum forum in _forumService.GetByGuestId(Guest.Id))
            {
                GuestForums.Add(forum);
            }
            OtherForums.Clear();
            foreach (Forum forum in _forumService.GetForumsExcludingGuests(Guest.Id))
            {
                OtherForums.Add(forum);
            }
        }
    }
}
