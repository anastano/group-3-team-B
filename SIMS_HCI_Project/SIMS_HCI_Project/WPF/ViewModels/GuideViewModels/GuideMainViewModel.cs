using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views;
using SIMS_HCI_Project.WPF.Views.GuideViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class GuideMainViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Commands
        public RelayCommand SeeAllTours { get; set; }
        public RelayCommand SeeStatistics { get; set; }
        public RelayCommand SignOut { get; set; }
        #endregion

        private TourTime _tourInProgress;
        public TourTime TourInProgress
        {
            get { return _tourInProgress; }
            set
            {
                _tourInProgress = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TourTime> _todaysTours;
        public ObservableCollection<TourTime> TodaysTours
        {
            get { return _todaysTours; }
            set
            {
                _todaysTours = value;
                OnPropertyChanged();
            }
        }

        private TourService _tourService;
        private GuestTourAttendanceService _guestTourAttendanceService;

        public GuideMainViewModel()
        {
            _tourService = new TourService();
            _guestTourAttendanceService = new GuestTourAttendanceService();

            LoadTourInProgress();
            LoadTodaysTours();
            InitCommands();
        }

        private void InitCommands()
        {
            SeeAllTours = new RelayCommand(ExecutedSeeAllToursCommand, CanExecuteCommand);
            SeeStatistics = new RelayCommand(ExecutedSeeStatisticsCommand, CanExecuteCommand);
            SignOut = new RelayCommand(ExecutedSignOutCommand, CanExecuteCommand);
        }

        private void LoadTourInProgress()
        {
            TourInProgress = _tourService.GetActiveTour(((User)App.Current.Properties["CurrentUser"]).Id);
        }

        private void LoadTodaysTours()
        {
            TodaysTours = new ObservableCollection<TourTime>(_tourService.GetTodaysToursByGuide(((User)App.Current.Properties["CurrentUser"]).Id));
        }

        private void ExecutedSeeAllToursCommand(object obj)
        {
            Window allTours = new AllToursView(_tourService);
            allTours.Show();
        }

        private void ExecutedSeeStatisticsCommand(object obj)
        {
            Window toursStatistics = new AllToursStatisticsView();
            toursStatistics.Show();
        }

        private void ExecutedSignOutCommand(object obj)
        {
            Window logIn = new LoginWindow();
            for (int i = App.Current.Windows.Count - 1; i >= 0; i--)
            {
                if (App.Current.Windows[i] == logIn) continue;
                App.Current.Windows[i].Close();
            }
            logIn.Show();
        }

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }

    }
}
