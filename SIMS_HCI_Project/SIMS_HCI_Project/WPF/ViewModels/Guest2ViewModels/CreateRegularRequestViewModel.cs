using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using SIMS_HCI_Project.Applications.Services;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Text.RegularExpressions;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class CreateRegularRequestViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public NavigationService NavigationService { get; set; }
        private RegularTourRequestService _regularTourRequestService { get; set; }
        public Guest2 Guest { get; set; }
        public RegularTourRequest RegularTourRequest { get; set; }
        public RelayCommand CompleteRequest { get; set; }
        public RelayCommand QuitRequest { get; set; }
        public CreateRegularRequestView CreateRegularRequestView { get; set; }
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

        private DateTime _start;
        public DateTime Start
        {
            get => _start;
            set
            {
                if (value != _start)
                {
                    _start = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _end;
        public DateTime End
        {
            get => _end;
            set
            {
                if (value != _end)
                {
                    _end = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _language;
        public string Language
        {
            get => _language;
            set
            {
                if (value != _language)
                {

                    _language = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        private int? _guestNumber;
        public int? GuestNumber
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

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {

                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        
        #endregion

        public CreateRegularRequestViewModel(Guest2 guest, NavigationService navigationService, CreateRegularRequestView createRegularRequestView)
        {
            Guest = guest;
            NavigationService = navigationService;
            CreateRegularRequestView = createRegularRequestView;
            RegularTourRequest = new RegularTourRequest();
            RegularTourRequest.Location = new Location();

            Start = DateTime.Now.AddDays(3); //48h before requests turns invalid
            End = DateTime.Now.AddDays(4);

            InitCommands();
            LoadFromFiles();

        }

        private void LoadFromFiles()
        {
            _regularTourRequestService = new RegularTourRequestService();
        }

        private void InitCommands()
        {
            CompleteRequest = new RelayCommand(ExecuteCompleteRequest, CanExecute);
            QuitRequest = new RelayCommand(ExecuteQuitRequest, CanExecute);
        }

        private MessageBoxResult ConfirmRequestSubmission()
        {
            string sMessageBoxText = $"Are you sure you want to submit this request?";
            string sCaption = "Regular tour request submission";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        private MessageBoxResult ConfirmRequestQuit()
        {
            string sMessageBoxText = $"Are you sure you want to quit this request?";
            string sCaption = "Regular tour request submission";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        private void ExecuteQuitRequest(object obj)
        {
            if(ConfirmRequestQuit() == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new RequestsView(Guest, NavigationService));
            }
        }

        private void ExecuteCompleteRequest(object obj)
        {
            if (IsValid)
            {
                if (ConfirmRequestSubmission() == MessageBoxResult.Yes)
                {
                    RegularTourRequest.GuestId = Guest.Id;
                    RegularTourRequest.Guest = Guest;
                    RegularTourRequest.IsPartOfComplex = false;
                    RegularTourRequest.SubmittingDate = DateTime.Now.AddDays(0);
                    RegularTourRequest.Status = RegularRequestStatus.PENDING;
                    RegularTourRequest.DateRange.Start = Start;
                    RegularTourRequest.DateRange.End = End;

                    RegularTourRequest.Language = Language;
                    RegularTourRequest.Location.Country = Country;
                    RegularTourRequest.Location.City = City;
                    RegularTourRequest.Description = Description;
                    RegularTourRequest.GuestNumber = GuestNumber ?? 0;

                    _regularTourRequestService.Add(RegularTourRequest);
                    MessageBox.Show("Tour request is submited. You can see it in the list of your regular tour requests list.");
                    NavigationService.Navigate(new RequestsView(Guest, NavigationService));
                }
            }
            else
            {
                MessageBox.Show("Not all fields are filled in correctly");
            }
        }

        private bool CanExecute(object obj)
        {
            return true;
        }

        #region Validation

        public string Error => null;

        public string this[string columnName]
        {
            get
            {

                if (columnName == "Language")
                {
                    if (string.IsNullOrEmpty(Language))
                        return "Language is required";
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
                else if (columnName == "GuestNumber")
                {
                    if (GuestNumber == null)
                        return "Guest number is required";

                    if (GuestNumber <= 0)
                    {
                        return "Guest number must be number greater than zero";
                    }
                }
                else if (columnName == "Description")
                {
                    if (string.IsNullOrEmpty(Description))
                        return "Description is required";
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "Name", "Country", "City", "GuestNumber", "Description" };

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

        #endregion
    }
}
