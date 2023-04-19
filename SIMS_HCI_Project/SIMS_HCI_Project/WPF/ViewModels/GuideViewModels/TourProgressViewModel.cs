using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class TourProgressViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Commands
        public RelayCommand MarkGuestPresent { get; set; }
        public RelayCommand EndTour { get; set; }
        public RelayCommand MoveKeyPoint { get; set; }
        #endregion

        private TourTime _tour;
        public TourTime Tour
        {
            get { return _tour; }
            set
            {
                _tour = value;
                OnPropertyChanged();
            }
        }

        public GuestTourAttendance SelectedGuest { get; set; }

        private TourService _tourService;
        private TourLifeCycleService _tourLifeCycleService;
        private GuestTourAttendanceService _guestTourAttendanceService;

        public TourProgressViewModel(TourTime tour)
        {
            Tour = tour;

            _tourLifeCycleService = new TourLifeCycleService();
            _guestTourAttendanceService = new GuestTourAttendanceService();
            _tourService = new TourService();

            InitCommands();
        }

        private void InitCommands()
        {
            MarkGuestPresent = new RelayCommand(ExecutedMarkGuestPresentCommand, CanExecuteCommand);
            EndTour = new RelayCommand(ExecutedEndTourCommand, CanExecuteCommand);
            MoveKeyPoint = new RelayCommand(ExecutedMoveKeyPointCommand, CanExecuteCommand);
        }

        private void ExecutedMarkGuestPresentCommand(object obj)
        {
            _guestTourAttendanceService.MarkGuestAsPresent(SelectedGuest);
            LoadTour();
        }

        private void ExecutedEndTourCommand(object obj)
        {
            _tourLifeCycleService.EndTour(Tour);
            LoadTour();
        }

        private void ExecutedMoveKeyPointCommand(object obj)
        {
            _tourLifeCycleService.MoveToNextKeyPoint(Tour);
            LoadTour();
        }

        private void LoadTour()
        {
            Tour = _tourService.GetTourInstance(Tour.Id);
        }

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }
    }
}
