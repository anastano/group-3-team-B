using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class GuideMainViewModel : INotifyPropertyChanged
    {
        private TourTimeService _tourTimeService;
        private TourVoucherService _tourVoucherService;
        private TourReservationService _tourReservationService;
        private TourService _tourService;
        private GuestTourAttendanceService _guestTourAttendanceService;

        public ObservableCollection<TourTime> AllTourTimes { get; set; }
        public TourTime SelectedTourTime { get; set; }

        public RelayCommand CancelTourCommand { get; set; }
        public RelayCommand SeeStatistics { get; set; }

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

        public Tour AllTimeTopTour { get; set; }
        public Tour SelectedYearTopTour { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GuideMainViewModel(Guide guide)
        {
            LoadFromFiles();
            CancelTourCommand = new RelayCommand(Excuted_CancelTourCommand, CanExecute_CancelTourCommand);
            SeeStatistics = new RelayCommand(Excuted_SeeStatisticsCommand, CanExecute_SeeStatisticsCommand);
            
            AllTimeTopTour = new Tour();
            SelectedTourTime = new TourTime();

            AllTourTimes = new ObservableCollection<TourTime>(_tourTimeService.GetAllByGuideId(guide.Id));
            SelectedTourTime = AllTourTimes.First();
            SelectedTourStatistics = _guestTourAttendanceService.GetTourStatistics(SelectedTourTime.TourId);
        }

        private void LoadFromFiles()
        {
            _tourTimeService = new TourTimeService();
            _tourVoucherService = new TourVoucherService();
            _tourReservationService = new TourReservationService();
            _tourService = new TourService();
            _guestTourAttendanceService = new GuestTourAttendanceService();

            _tourService.ConnectDepartureTimes(_tourTimeService);
            _guestTourAttendanceService.LoadConnections();
        }


        public void Excuted_CancelTourCommand(object obj)
        {
            _tourTimeService.CancelTour(SelectedTourTime, _tourVoucherService, _tourReservationService);
        }

        public bool CanExecute_CancelTourCommand(object obj)
        {
            return true;
        }

        public void Excuted_SeeStatisticsCommand(object obj)
        {
            SelectedTourStatistics = _guestTourAttendanceService.GetTourStatistics(SelectedTourTime.TourId);
        }

        public bool CanExecute_SeeStatisticsCommand(object obj)
        {
            return true;
        }

    }
}
