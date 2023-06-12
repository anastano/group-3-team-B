using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Commands.Global;
using SIMS_HCI_Project.WPF.Views.GuideViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class AllToursViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Commands
        public RelayCommand FilterTours { get; set; }
        public RelayCommand ResetFilter { get; set; }
        public GuideNavigationCommands NavigationCommands { get; set; }
        #endregion

        private TourService _tourService;

        private ObservableCollection<Tour> _allTours;
        public ObservableCollection<Tour> AllTours
        {
            get { return _allTours; }
            set
            {
                _allTours = value;
                OnPropertyChanged();
            }
        }
        public Tour SelectedTour { get; set; }

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
        private Location _location;
        public Location Location
        {
            get { return _location; }
            set
            {
                _location = value;
                OnPropertyChanged();
            }
        }
        public string _language;
        public string Language
        {
            get { return _language; }
            set
            {
                _language = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Location> _availableLocations;
        public ObservableCollection<Location> AvailableLocations
        {
            get { return _availableLocations; }
            set
            {
                _availableLocations = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> _availableLanguages;
        public ObservableCollection<string> AvailableLanguages
        {
            get { return _availableLanguages; }
            set
            {
                _availableLanguages = value;
                OnPropertyChanged();
            }
        }

        public AllToursViewModel(TourService tourService)
        {
            _tourService = tourService;

            Location = new Location();
            Title = "";

            LoadAllTours();
            InitCommands();
            LoadPossibleFilters();
        }

        private void LoadAllTours()
        {
            AllTours = new ObservableCollection<Tour>(_tourService.GetAllTourInformationByGuide(((User)App.Current.Properties["CurrentUser"]).Id));
        }

        private void InitCommands()
        {
            FilterTours = new RelayCommand(ExecutedFilterToursCommand, CanExecuteCommand);
            ResetFilter = new RelayCommand(ExecutedResetFilterCommand, CanExecuteCommand);
            NavigationCommands = new GuideNavigationCommands();
        }

        private void LoadPossibleFilters()
        {
            if (AllTours != null)
            {
                AvailableLocations = new ObservableCollection<Location>(AllTours.Select(t => t.Location).Distinct());
                AvailableLanguages = new ObservableCollection<string>(AllTours.Select(t => t.Language).Distinct());
            }
        }

        private void ExecutedFilterToursCommand(object obj)
        {
            if (Location == null) Location = new Location(); // E: kada se ispravi search obrisati

            AllTours = new ObservableCollection<Tour>(_tourService.Search(guideId: ((User)App.Current.Properties["CurrentUser"]).Id, country: Location.Country, city: Location.City, language: Language, duration: 0, guestsNum: 0).Where(t => t.Title.ToLower().Contains(Title.ToLower()))); // E: temporary filtriranje i ovde zbog hci, srediti za sims
        }

        private void ExecutedResetFilterCommand(object obj)
        {
            Location = null;
            Language = null;
            Title = "";

            LoadAllTours();
        }

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }
    }
}
