using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Xml.Linq;
using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class AccommodationSearchViewModel : INotifyPropertyChanged
    {
        private readonly AccommodationService _accommodationService;

        private readonly LocationController _locationController;

        private readonly AccommodationReservationService _accommodationReservationService;
        private AccommodationReservationViewModel _accommodationReservationViewModel;
        public Accommodation Accommodation { get; set; }
        public ObservableCollection<Accommodation> Accommodations { get; set; }

        public ObservableCollection<Location> Locations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public Guest1 Guest { get; set; }
        public RelayCommand PlusGuestNumberCommand { get; set; }
        public RelayCommand MinusGuestNumberCommand { get; set; }
        public RelayCommand PlusDaysNumberCommand { get; set; }
        public RelayCommand MinusDaysNumberCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand ReserveAccommodationCommand { get; set; }
        private int _guestsNumber;
        public int GuestsNumber
        {
            get => _guestsNumber;
            set
            {
                if (value != _guestsNumber)
                {
                    _guestsNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _daysNumber;
        public int DaysNumber
        {
            get => _daysNumber;
            set
            {
                if (value != _daysNumber)
                {
                    _daysNumber = value;
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public AccommodationSearchViewModel(Guest1 guest)
        {
            _accommodationService = new AccommodationService();
            _locationController = new LocationController();
            _accommodationReservationService = new AccommodationReservationService();
            Accommodation = new Accommodation();
            Guest = guest;
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetAll());
            InitCommands();

        }
        /*
        private void SearchAccommodation(object sender, EventArgs e)
        {
            List<Accommodation> searchResult = new List<Accommodation>();

            int maxGuests;
            bool isValidMaxGuests = int.TryParse(txtGuestNumber.Text, out maxGuests);
            int reservationDays;
            bool isValidReservationDays = int.TryParse(txtReservationDays.Text, out reservationDays);
            ComboBoxItem selectedItem = comboboxType.SelectedItem as ComboBoxItem;
            string selectedItemContent = null;

            if (selectedItem != null)
            {
                selectedItemContent = selectedItem.Content.ToString();
            }

            if (!isValidMaxGuests)
            {
                maxGuests = 0;
            }

            if (!isValidReservationDays)
            {
                reservationDays = 0;
            }

            searchResult = _accommodationController.Search(txtName.Text, txtCountry.Text, txtCity.Text, selectedItemContent, maxGuests, reservationDays);

            DataGridAccommodation.ItemsSource = searchResult;
        }
        */
        public void ExecutedReserveAccommodationCommand(object obj)
        {
            if (SelectedAccommodation != null)
            {
                _accommodationReservationViewModel = new AccommodationReservationViewModel(SelectedAccommodation, Guest);
                _accommodationReservationViewModel.Closed += UnloadUserControl;
                CurrentViewModel = _accommodationReservationViewModel;
                
            }
        }
        private void UnloadUserControl(object sender, EventArgs e)
        {
            CurrentViewModel = new AccommodationSearchViewModel(Guest);
        }
        public void ExecutedSearchCommand(object obj)
        {
            //kasnije
        }
        public void ExecutedMinusGuestNumberCommand(object obj)
        {
            if (GuestsNumber > 1)
            {
                GuestsNumber -= 1;
            }
        }
        public void ExecutedPlusGuestNumberCommand(object obj)
        {
            GuestsNumber += 1;
        }
        public void ExecutedMinusDaysNumberCommand(object obj)
        {
            if (DaysNumber > 1)
            {
                DaysNumber -= 1;
            }
        }
        public void ExecutedPlusDaysNumberCommand(object obj)
        {
            DaysNumber += 1;
        }

        public bool CanExecute(object obj)
        {
            return true;
        }
        public void InitCommands()
        {
            ReserveAccommodationCommand = new RelayCommand(ExecutedReserveAccommodationCommand, CanExecute);
            SearchCommand = new RelayCommand(ExecutedSearchCommand, CanExecute);
            MinusGuestNumberCommand = new RelayCommand(ExecutedMinusGuestNumberCommand, CanExecute);
            PlusGuestNumberCommand = new RelayCommand(ExecutedPlusGuestNumberCommand, CanExecute);
            MinusDaysNumberCommand = new RelayCommand(ExecutedMinusDaysNumberCommand, CanExecute);
            PlusDaysNumberCommand = new RelayCommand(ExecutedPlusDaysNumberCommand, CanExecute);
        }
    }
}
