using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class ForumCreateViewModel : INotifyPropertyChanged
    {
        private NavigationService _navigationService;
        private ForumService _forumService;
        private ForumCommentService _forumCommentService;
        private LocationService _locationService;
        private NotificationService _notificationService;
        public ObservableCollection<String> Countries { get; set; }
        //public ObservableCollection<String> Cities { get; set; }
        public RelayCommand CreateForumCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public Guest1 Guest { get; set; }
        private String _comment;
        public String Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }
        private String _errorMessage;
        public String ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (value != _errorMessage)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }
        private String _user;
        public String User
        {
            get => _user;
            set
            {
                if (value != _user)
                {
                    _user = value;
                    OnPropertyChanged();
                }
            }
        }
        private List<String> _cities;
        public List<String> Cities
        {
            get => _cities;
            set
            {
                if (value != _cities)
                {
                    _cities = value;
                    
                    OnPropertyChanged();
                }
            }
        }
        private bool _isErrorVisible;
        public bool IsErrorVisible
        {
            get => _isErrorVisible;
            set
            {
                if (value != _isErrorVisible)
                {
                    _isErrorVisible = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isCityEnabled;
        public bool IsCityEnabled
        {
            get => _isCityEnabled;
            set
            {
                if (value != _isCityEnabled)
                {
                    _isCityEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _selectedCountry;
        public string SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;
                    UpdateCities();
                    OnPropertyChanged();
                }
            }
        }
        private string _selectedCity;
        public string SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;
                    UpdateCities();
                    OnPropertyChanged();
                }
            }
        }
        private Regex urlRegex = new Regex("(http(s?)://.)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)|(^$)");

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ForumCreateViewModel(Guest1 guest, NavigationService navigationService)
        {
            _navigationService = navigationService;
            _notificationService = new NotificationService();
            _locationService = new LocationService();
            _forumService = new ForumService();
            _forumCommentService = new ForumCommentService();
            InitProperties(guest);
            InitCommands();
        }

        private void InitProperties(Guest1 guest)
        {
            Countries = new ObservableCollection<String>(_locationService.GetAllCountries());
            Cities = _locationService.GetAllCities();
            Guest = guest;
            User = guest.GetFullName();
            Comment = "";
            IsCityEnabled = true;
            IsErrorVisible = false;
        }

        public void UpdateCities()
        {
            if(SelectedCountry == null)
            {
                Cities = _locationService.GetAllCities();
            }
            else
            {
                Cities = _locationService.GetCitiesByCountry(SelectedCountry);
            }
        }
        public void ExecutedCreateForumCommand(object obj)
        {
            if(SelectedCountry == null || SelectedCity == null || Comment == "")
            {
                ErrorMessage = "All fields are required";
                IsErrorVisible = true;
            }
            else
            {
                //rijesiti kako za komentar
                _forumService.Add(new Forum(Guest, _locationService.GetLocation(SelectedCountry, SelectedCity)));
                _notificationService.MakeForumNotifications(_locationService.GetLocation(SelectedCountry, SelectedCity));
                _navigationService.Navigate(new ForumsViewModel(Guest, _navigationService, 1), "Forums");
            }
            
        }
        public void ExecutedCancelCommand(object obj)
        {
            _navigationService.Navigate(new ForumsViewModel(Guest, _navigationService, 1), "Forums");
        }
        public bool CanExecute(object obj)
        {
            return true;
        }
        public void InitCommands()
        {
            CreateForumCommand = new RelayCommand(ExecutedCreateForumCommand, CanExecute);
            CancelCommand = new RelayCommand(ExecutedCancelCommand, CanExecute);
        }
    }
}
