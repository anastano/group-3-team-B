using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class AddAccommodationViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly AccommodationService _accommodationService;
        public Owner Owner { get; set; }
        public Accommodation Accommodation { get; set; }
        public Location Location { get; set; }
        public AddAccommodationView AddAccommodationView { get; set; }
        public AccommodationsViewModel AccommodationsVM { get; set; }

        #region OnPropertyChanged
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {

                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (value != _city)
                {

                    _city = value;
                    OnPropertyChanged(nameof(City));
                }
            }
        }

        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (value != _country)
                {

                    _country = value;
                    OnPropertyChanged(nameof(Country));
                }
            }
        }

        private int? _maxGuests;
        public int? MaxGuests
        {
            get => _maxGuests;
            set
            {
                if (value != _maxGuests)
                {

                    _maxGuests = value;
                    OnPropertyChanged(nameof(MaxGuests));
                }
            }
        }

        private int? _minDays;
        public int? MinDays
        {
            get => _minDays;
            set
            {
                if (value != _minDays)
                {

                    _minDays = value;
                    OnPropertyChanged(nameof(MinDays));
                }
            }
        }

        private int? _cancellationDays;
        public int? CancellationDays
        {
            get => _cancellationDays;
            set
            {
                if (value != _cancellationDays)
                {

                    _cancellationDays = value;
                    OnPropertyChanged(nameof(CancellationDays));
                }
            }
        }

        private string _imageURL;
        public string ImageURL
        {
            get => _imageURL;
            set
            {
                if (value != _imageURL)
                {

                    _imageURL = value;
                    OnPropertyChanged(nameof(ImageURL));
                }
            }
        }
        #endregion

        public ObservableCollection<string> Images { get; set; }

        public RelayCommand AddAccommodationImageCommand { get; set; }
        public RelayCommand RemoveAccommodationImageCommand { get; set; }
        public RelayCommand RegisterNewAccommodationCommand { get; set; }
        public RelayCommand CloseAddAccommodationViewCommand { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public AddAccommodationViewModel(AddAccommodationView addAccommodationView, AccommodationsViewModel accommodationsVM, AccommodationService accommodationService, Owner owner)
        {
            InitCommands();

            _accommodationService = accommodationService;
            AddAccommodationView = addAccommodationView;
            AccommodationsVM = accommodationsVM;

            Owner = owner;
            Accommodation = new Accommodation();
            Location = new Location();
            Images = new ObservableCollection<string>();
            MaxGuests = null;
            MinDays = null;
            CancellationDays = null;
            ImageURL = "";
        }

        #region Commands
        public void Executed_AddAccommodationImageCommand(object obj)
        {
            if (IsImageURLValid && !ImageURL.Equals(""))
            {
                Images.Add(ImageURL);
                ImageURL = "";
            }
        }

        public bool CanExecute_AddAccommodationImageCommand(object obj)
        {
            return true;
        }

        public void Executed_RemoveAccommodationImageCommand(object obj)
        {
            if (AddAccommodationView.lbImages.SelectedItem != null)
            {
                Images.RemoveAt(AddAccommodationView.lbImages.SelectedIndex);
            }
        }

        public bool CanExecute_RemoveAccommodationImageCommand(object obj)
        {
            return true;
        }

        private MessageBoxResult ConfirmRegisterNewAccommodation()
        {
            string sMessageBoxText = $"Are you sure you want to register this accommodation?";
            string sCaption = "Add Accommodation Confirmation";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        public void Executed_RegisterNewAccommodationCommand(object obj)
        {
            if (IsValid)
            {
                if (ConfirmRegisterNewAccommodation() == MessageBoxResult.Yes)
                {
                    Location.City = City;
                    Location.Country = Country;

                    Accommodation.OwnerId = Owner.Id;
                    Accommodation.Name = Name;
                    Accommodation.MaxGuests = MaxGuests ?? 0;
                    Accommodation.MinimumReservationDays = MinDays ?? 0;
                    Accommodation.CancellationDeadlineInDays = CancellationDays ?? 0;

                    Accommodation.Images = new List<string>(Images);

                    _accommodationService.Add(Accommodation, Location);

                    AddAccommodationView.Close();
                    AccommodationsVM.UpdateAccommodations();
                }
            }
            else
            {
                MessageBox.Show("Not all fields are filled in correctly");
            }
        }

        public bool CanExecute_RegisterNewAccommodationCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseAddAccommodationViewCommand(object obj)
        {
            AddAccommodationView.Close();
        }

        public bool CanExecute_CloseAddAccommodationViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            AddAccommodationImageCommand = new RelayCommand(Executed_AddAccommodationImageCommand, CanExecute_AddAccommodationImageCommand);
            RemoveAccommodationImageCommand = new RelayCommand(Executed_RemoveAccommodationImageCommand, CanExecute_RemoveAccommodationImageCommand);
            RegisterNewAccommodationCommand = new RelayCommand(Executed_RegisterNewAccommodationCommand, CanExecute_RegisterNewAccommodationCommand);
            CloseAddAccommodationViewCommand = new RelayCommand(Executed_CloseAddAccommodationViewCommand, CanExecute_CloseAddAccommodationViewCommand);
        }

        #region Validation

        private Regex urlRegex = new Regex("(http(s?)://.)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)|(^$)");

        public string Error => null;

        public string this[string columnName]
        {
            get
            {

                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                        return "Name is required";
                }
                else if (columnName == "Country")
                {
                    if (string.IsNullOrEmpty(Country))
                        return "Country is required";
                }
                else if (columnName == "City")
                {
                    if (string.IsNullOrEmpty(City))
                        return "City is required";
                }
                else if (columnName == "MaxGuests")
                {
                    if (MaxGuests == null)
                        return "Maximum guests is required";

                    if (MaxGuests <= 0)
                    {
                        return "Maximum guests must be number greater than zero";
                    }
                }
                else if (columnName == "MinDays")
                {
                    if (MinDays == null)
                        return "Minimum days is required";

                    if (MinDays <= 0)
                    {
                        return "Minimum days must be number greater than zero";
                    }
                }
                else if (columnName == "CancellationDays")
                {
                    if (CancellationDays == null)
                        return "Cancellation days is required";

                    if (CancellationDays <= 0)
                    {
                        return "Cancellation days must be number greater than zero";
                    }
                }
                else if (columnName == "ImageURL")
                {
                    Match match = urlRegex.Match(ImageURL);
                    if (!match.Success)
                    {
                        return "URL is not in valid format.";
                    }
                }


                return null;
            }
        }

        private readonly string[] _validatedProperties = { "Name", "Country", "City" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

        private readonly string[] _validatedImageProperty = { "ImageURL" };

        public bool IsImageURLValid
        {
            get
            {
                foreach (var property in _validatedImageProperty)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

        #endregion

    }
}
