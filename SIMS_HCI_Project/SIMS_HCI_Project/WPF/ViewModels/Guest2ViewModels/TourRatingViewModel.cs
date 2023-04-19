using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System.Windows;
using SIMS_HCI_Project.Applications.Services;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class TourRatingViewModel : INotifyPropertyChanged
    {
        private TourService _tourService;
        private TourReservationService _tourReservationService;
        private TourVoucherService _tourVoucherService;
        private LocationService _locationService;
        private GuestTourAttendanceService _guestTourAttendanceService;
        private TourRatingService _tourRatingService;
        public TourRatingView TourRatingView { get; set; }
        public Tour Tour { get; set; }
        public TourReservation TourReservation { get; set; }
        public TourReservation SelectedReservation { get; set; }
        public Guest2 Guest { get; set; }
        public ObservableCollection<TourReservation> UnratedReservations { get; set; }

        public RelayCommand Back { get; set; }
        public RelayCommand Rate { get; set; }

        public TourRatingViewModel(TourRatingView tourRatingView, Guest2 guest2)
        {
            TourRatingView = tourRatingView;
            Guest = guest2;

            LoadFromFiles();
            InitCommands();

            

            UnratedReservations = new ObservableCollection<TourReservation>(_tourReservationService.GetUnratedReservations(Guest.Id, _guestTourAttendanceService, _tourRatingService, _tourService));

        }

        public void LoadFromFiles()
        {
            _tourService = new TourService();
            _tourReservationService = new TourReservationService();
            _tourVoucherService = new TourVoucherService();
            _locationService = new LocationService();
            _guestTourAttendanceService = new GuestTourAttendanceService();
            _tourRatingService = new TourRatingService();
        }

        public void InitCommands()
        {
            Back = new RelayCommand(Executed_Back, CanExecute_Back);
            Rate = new RelayCommand(Executed_Rate, CanExecute_Rate);
        }

        private void Executed_Back(object sender)
        {
            Window window = new Guest2MainView(Guest);
            window.Show();
            TourRatingView.Close();
        }
        public bool CanExecute_Back(object sender)
        {
            return true;
        }
        private void Executed_Rate(object sender)
        {
            //pozovvi prozor za ocenjivanje
            Window window = new RateSelectedReservationView(Guest, SelectedReservation);
            window.Show();
            TourRatingView.Close();
        }
        public bool CanExecute_Rate(object sender)
        {
            return true;
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}
