using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Commands.Global;
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
        public GuideNavigationCommands NavigationCommands { get; set; }
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

        private string _tourInProgressImage;
        public string TourInProgressImage
        {
            get { return _tourInProgressImage; }
            set
            {
                _tourInProgressImage = value;
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
            NavigationCommands = new GuideNavigationCommands();
        }

        private void LoadTourInProgress()
        {
            TourInProgress = ((Guide)App.Current.Properties["CurrentUser"]).GetActiveTour();
            if(TourInProgress != null)
            {
                TourInProgressImage = TourInProgress.Tour.Images.First();
            }
        }

        private void LoadTodaysTours()
        {
            TodaysTours = new ObservableCollection<TourTime>(_tourService.GetTodaysToursByGuide(((User)App.Current.Properties["CurrentUser"]).Id));
        }
    }
}
