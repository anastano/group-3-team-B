using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System.Windows;
using SIMS_HCI_Project.Applications.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class TourSearchViewModel : INotifyPropertyChanged, IObserver
    {
        #region Services
        private TourReservationService _tourReservationService;
        private TourService _tourService;
        private LocationService _locationService;
        private TourVoucherService _tourVoucherService;
        #endregion
        #region Commands
        public RelayCommand Search { get; set; }
        public RelayCommand ShowImages { get; set; }
        public RelayCommand Reserve { get; set; }
        public RelayCommand Back { get; set; }
        #endregion
        public TourSearchView  TourSearchView { get; set; }
        public Guest2 Guest { get; set; }
        public Tour SelectedTour { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public TourSearchViewModel(TourSearchView tourSearchView, Guest2 guest2)
        {
            TourSearchView = tourSearchView;
            Guest = guest2;
            InitCommands();
            LoadFromFiles();

            Tours = new ObservableCollection<Tour>(_tourService.GetAllTourInformation());
        }
        public void LoadFromFiles()
        {
            _tourService = new TourService();
            _locationService = new LocationService();
        }
        public void InitCommands()
        {
            Reserve = new RelayCommand(ExecutedReserve, CanExecuteReserve);
            Back = new RelayCommand(ExecutedBack, CanExecuteBack);
        }
        #region Commands
        public void ExecutedReserve(object obj)
        {
            Window window = new TourReservationView(SelectedTour, Guest);
            window.Show();
            TourSearchView.Close();
        }
        public bool CanExecuteReserve(object obj)
        {
            return true;
        }
        private void ExecutedBack(object sender)
        {
            Window window = new Guest2MainView(Guest);
            window.Show();
            TourSearchView.Close();
        }
        public bool CanExecuteBack(object sender)
        {
            return true;
        }
        #endregion
        private int _guestNumber;
        public int GuestNumber
        {
            get => _guestNumber;
            set
            {
                if (value != _guestNumber)
                {
                    _guestNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
