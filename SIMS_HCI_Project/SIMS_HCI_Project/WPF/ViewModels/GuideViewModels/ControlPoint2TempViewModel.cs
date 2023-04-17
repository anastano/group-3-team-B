using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class ControlPoint2TempViewModel : INotifyPropertyChanged
    {
        private TourTimeService _tourTimeService;
        private TourVoucherService _tourVoucherService;
        private TourReservationService _tourReservationService;
        private TourService _tourService;
        private GuestTourAttendanceService _guestTourAttendanceService;
        private TourStatisticsService _tourStatisticsService;
        private TourRatingService _tourRatingService;

        private ObservableCollection<TourTime> _allTourTimes;
        public ObservableCollection<TourTime> AllTourTimes
        {
            get { return _allTourTimes;  }
            set
            {
                _allTourTimes = value;
                OnPropertyChanged();
            }
        }
        public TourTime SelectedTourTime { get; set; }

        public RelayCommand CancelTourCommand { get; set; }
        public RelayCommand SeeStatistics { get; set; }
        public RelayCommand SeeRatings { get; set; }
        public RelayCommand MarkRatingInvalid { get; set; }

        private TourStatisticsInfo _selectedTourStatistics;
        public TourStatisticsInfo SelectedTourStatistics
        {
            get { return _selectedTourStatistics; }
            set
            {
                _selectedTourStatistics = value;
                OnPropertyChanged();
            }
        }

        public TourTime AllTimeTopTour { get; set; }
        private TourTime _selectedYearTopTour;
        public TourTime SelectedYearTopTour
        {
            get { return _selectedYearTopTour; }
            set
            {
                _selectedYearTopTour = value;
                OnPropertyChanged();
            }
        }

        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                UpdateTopTourByYear();
            }
        }
        public List<int> YearsWithTours { get; set; }

        private ObservableCollection<TourRating> _selectedTourRatings;
        public ObservableCollection<TourRating> SelectedTourRatings
        {
            get { return _selectedTourRatings; }
            set
            {
                _selectedTourRatings = value;
                OnPropertyChanged();
            }
        }
        public TourRating SelectedRating { get; set; } 

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Guide _guide;

        public ControlPoint2TempViewModel(Guide guide)
        {
            LoadFromFiles();
            CancelTourCommand = new RelayCommand(Excuted_CancelTourCommand, CanExecute_CancelTourCommand);
            SeeStatistics = new RelayCommand(Excuted_SeeStatisticsCommand, CanExecute_SeeStatisticsCommand);
            SeeRatings = new RelayCommand(Excuted_SeeRatingsCommand, CanExecute_SeeRatingsCommand);
            MarkRatingInvalid = new RelayCommand(Excuted_MarkRatingInvalidCommand, CanExecute_MarkRatingInvalidCommand);

            _guide = guide;

            AllTimeTopTour = _tourStatisticsService.GetTopTour();
            YearsWithTours = _tourTimeService.GetYearsWithToursByGuide(guide.Id);
            SelectedYear = YearsWithTours.First();
            UpdateTopTourByYear();

            LoadAllTours();
            SelectedTourTime = AllTourTimes.First();
            SelectedTourStatistics = _tourStatisticsService.GetTourStatistics(SelectedTourTime.Id);

            LoadRatings();
        }

        private void LoadFromFiles()
        {
            _tourTimeService = new TourTimeService();
            _tourVoucherService = new TourVoucherService();
            _tourReservationService = new TourReservationService();
            _tourService = new TourService();
            _guestTourAttendanceService = new GuestTourAttendanceService();
            _tourStatisticsService = new TourStatisticsService();
            _tourRatingService = new TourRatingService();

            _tourService.ConnectDepartureTimes();
            _tourRatingService.LoadConnections();
            _guestTourAttendanceService.LoadConnections();
        }

        public void Excuted_CancelTourCommand(object obj)
        {
            _tourTimeService.CancelTour(SelectedTourTime, _tourVoucherService, _tourReservationService);
            LoadAllTours();
        }

        public bool CanExecute_CancelTourCommand(object obj)
        {
            return true;
        }

        public void Excuted_SeeStatisticsCommand(object obj)
        {
            SelectedTourStatistics = _tourStatisticsService.GetTourStatistics(SelectedTourTime.Id);
        }

        public bool CanExecute_SeeStatisticsCommand(object obj)
        {
            return true;
        }

        private void UpdateTopTourByYear()
        {
            SelectedYearTopTour = _tourStatisticsService.GetTopTourByYear(SelectedYear);
        }

        public void Excuted_SeeRatingsCommand(object obj)
        {
            LoadRatings();
        }

        public bool CanExecute_SeeRatingsCommand(object obj)
        {
            return true;
        }

        public void Excuted_MarkRatingInvalidCommand(object obj)
        {
            _tourRatingService.MarkAsInvalid(SelectedRating);

            LoadRatings();
        }

        public bool CanExecute_MarkRatingInvalidCommand(object obj)
        {
            return true;
        }

        private void LoadAllTours()
        {
            AllTourTimes = new ObservableCollection<TourTime>(_tourTimeService.GetAllByGuideId(_guide.Id));
        }

        private void LoadRatings()
        {
            SelectedTourRatings = new ObservableCollection<TourRating>(_tourRatingService.GetByTourId(SelectedTourTime.Id));
        }
    }
}
