﻿using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Services;
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
using System.Windows;
using System.Windows.Controls;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class Guest1MainViewModel : INotifyPropertyChanged
    {
        private NavigationService _navigationService;
        private AccommodationService _accommodationService;
        private AccommodationReservationService _reservationService;
        private NotificationService _notificationService;
        private SuperGuestTitleService _titleService;
        private RatingGivenByGuestService _ratingGivenByGuestService;
        private RenovationService _renovationService;
        private UserService _userService;
        public Guest1MainView Guest1MainView { get; set; }
        public Guest1 Guest { get; set; }
        public RelayCommand ShowReservationsCommand { get; set; }
        public RelayCommand SearchAccommodationCommand { get; set; }
        public RelayCommand ShowRatingsCommand { get; set; }
        public RelayCommand ShowProfileCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }
        public RelayCommand ChangePage { get; set; }
        public Button Menu { get; set; }
        public Grid grid;
        public Grid Grid

        {
            get => grid;
            set
            {
                if (value != grid)
                {
                    grid = value;
                    OnPropertyChanged();
                }
            }

        }

        private object _currentViewModel;

        public object CurrentViewModel

        {
            get => _currentViewModel;
            set
            {
                if (value != _currentViewModel)
                {
                    _currentViewModel = value;
                    OnPropertyChanged();
                }
            }

        }
        private int selected;
        public int SelectedItem
        {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged();
            }
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Guest1MainViewModel(Guest1MainView guest1MainView, Guest1 guest)
        {
            _navigationService = new NavigationService();
            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
            Guest1MainView = guest1MainView;
            Guest = guest;
            LoadFromFiles();
            InitCommands();
            Grid = Guest1MainView.GridMenu;
            Menu = Guest1MainView.CloseMenuButton;
            _navigationService.Navigate(new ProfileViewModel(Guest), "My profile");
            SelectedItem = -1;
        }
        private void OnCurrentViewModelChanged()
        {
            CurrentViewModel = _navigationService.NavigationStore.CurrentViewModel;
            Title = _navigationService.NavigationStore.Title;
            OnPropertyChanged(nameof(CurrentViewModel));
            OnPropertyChanged(nameof(Title));
        }

        public void LoadFromFiles()
        {
            _accommodationService = new AccommodationService();
            _reservationService = new AccommodationReservationService();
            _notificationService = new NotificationService();
            _titleService = new SuperGuestTitleService();
            _ratingGivenByGuestService = new RatingGivenByGuestService();
            _renovationService = new RenovationService();
            _userService = new UserService();
            
            _reservationService.ConvertReservedReservationIntoCompleted(DateTime.Now);
            _reservationService.ConvertReservationsIntoRated(_ratingGivenByGuestService);
            _titleService.UpdateTitles();
            _titleService.ConvertActiveTitlesIntoExpired(DateTime.Now);
            _accommodationService.ConvertAccommodationIntoRenovated(_renovationService);
            _userService.FillOwnerSuperFlag(_ratingGivenByGuestService);
        }
        public bool CanExecute(object obj)
        {
            return true;
        }
        public void ExecutedLogoutCommand(object obj)
        {
            foreach (Notification notification in _notificationService.GetUnreadByUserId(Guest.Id))
            {
                _notificationService.MarkAsRead(notification.Id);
            }
            Guest1MainView.Close();
        }
        public void ExecutedShowProfileCommand(object obj)
        {
            _navigationService.Navigate(new ProfileViewModel(Guest), "My profile");
            SelectedItem = -1;
        }
        private void ExecutedChangePageCommand(object obj)
        {
            if (SelectedItem == 0)
            {
                _navigationService.Navigate(new AccommodationSearchViewModel(Guest, _navigationService), "Search Accommodations");
                SelectedItem = -1;
                CustomizeGridSize();
            }
            else if (SelectedItem == 1)
            {
                _navigationService.Navigate(new ReservationsViewModel(Guest, _navigationService, 0), "My Reservations");
                SelectedItem = -1;
                CustomizeGridSize();
            }
            else if (SelectedItem == 2)
            {
                _navigationService.Navigate(new ForumsViewModel(Guest, _navigationService, 0), "Forums");
                SelectedItem = -1;
                CustomizeGridSize();
            }
            else if (SelectedItem == 3)
            {
                _navigationService.Navigate(new QuickReserveViewModel(Guest, _navigationService), "Quick Reserve");
                SelectedItem = -1;
                CustomizeGridSize();
            }
            else if (SelectedItem == 4)
            {
                _navigationService.Navigate(new MyRatingsViewModel(Guest), "My rating");
                CustomizeGridSize();
            }
        }
        private void CustomizeGridSize()
        {
            if (Grid.Width == 200)
                Menu.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
        public void InitCommands()
        {
            ShowProfileCommand = new RelayCommand(ExecutedShowProfileCommand, CanExecute);
            LogoutCommand = new RelayCommand(ExecutedLogoutCommand, CanExecute);
            ChangePage = new RelayCommand(ExecutedChangePageCommand, CanExecute);
        }
    }
}
