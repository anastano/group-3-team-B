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
        private TourReservationService _tourReservationService;
        private TourService _tourService;
        private TourTimeService _tourTimeService;
        private LocationService _locationService;
        private TourKeyPointService _tourKeyPointService;
        private TourVoucherService _tourVoucherService;
        public TourSearchView  TourSearchView { get; set; }
        public Guest2 Guest { get; set; }
        public Tour Tour { get; set; }
        public TourTime TourTime { get; set; }
        public Tour SelectedTour { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }


        public RelayCommand Search { get; set; }
        public RelayCommand ShowImages { get; set; }
        public RelayCommand Reserve { get; set; }

        public TourSearchViewModel(TourSearchView tourSearchView, Guest2 guest2)
        {
            TourSearchView = tourSearchView;
            Guest = guest2;
            InitCommands();
            LoadFromFiles();

            Tours = new ObservableCollection<Tour>(_tourService.GetAll());
           
            
        }
        public void LoadFromFiles()
        {
            _tourService = new TourService();
            _locationService = new LocationService();
            _tourKeyPointService = new TourKeyPointService();
            _tourTimeService = new TourTimeService();

            _tourService.ConnectLocations();
            _tourService.ConnectKeyPoints();
            _tourService.ConnectDepartureTimes();
        }
        public void InitCommands()
        {
            //Search = new RelayCommand(Executed_Search, CanExecute_Search);
            //ShowImages = new RelayCommand(Executed_ShowImages, CanExecute_ShowImages);
            Reserve = new RelayCommand(Executed_Reserve, CanExecute_Reserve);
        }
        public void Executed_Search(object obj) // must refactor to support command
        {
            /*
            List<Tour> result = new List<Tour>();

            int guestsNum;
            bool isValidGuestsNum = int.TryParse(txtGuestNumber.Text, out guestsNum);
            int duration; //Assuming that the user enters the maximum duration of the tour. Should discuss whether this is a good way, if so: the name should be changed and a field for the minimum duration added.
            bool isValidDuration = int.TryParse(txtDuration.Text, out duration);

            if (!isValidGuestsNum)
            {
                guestsNum = 0;
            }

            if (!isValidDuration)
            {
                duration = 0;
            }

            result = _tourController.Search(txtCountry.Text, txtCity.Text, duration, txtLanguage.Text, guestsNum);

            dgTours.ItemsSource = result;
            /////////////
            List<Tour> result = new List<Tour>();
            //todo: validacija da nije slovo
            result = _tourService.Search(Country, City, TourDuration, Language, GuestNumber);
            */
            
        }
        public bool CanExecute_Search(object obj)
        {
            return true;
        }
        public void Executed_ShowImages(object obj)
        {
           // Window window = new TourImagesView(_tourService, SelectedTour);
            //window.Show();
        }
        public bool CanExecute_ShowImages(object obj)
        {
            return true;
        }
        public void Executed_Reserve(object obj)
        {
            Window window = new TourReservationView(SelectedTour, Guest);
            window.Show();
            TourSearchView.Close();
        }
        public bool CanExecute_Reserve(object obj)
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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
