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
using SIMS_HCI_Project.WPF.Validations;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class CreateRegularRequestViewModel : INotifyPropertyChanged
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

        private RegularTourRequestValidation _regularTourRequestValidation;
        public RegularTourRequestValidation RegularTourRequestValidation
        {
            get => _regularTourRequestValidation;
            set
            {
                if (value != _regularTourRequestValidation)
                {

                    _regularTourRequestValidation = value;
                    OnPropertyChanged(nameof(RegularTourRequestValidation));
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
            RegularTourRequestValidation = new RegularTourRequestValidation();

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
                NavigationService.Navigate(new RequestsView(Guest, NavigationService, 0));
            }
        }

        private void ExecuteCompleteRequest(object obj)
        {
            RegularTourRequestValidation.Validate();
            if (RegularTourRequestValidation.IsValid)
            {
                if (ConfirmRequestSubmission() == MessageBoxResult.Yes)
                {
                    RegularTourRequest.GuestId = Guest.Id;
                    RegularTourRequest.Guest = Guest;
                    RegularTourRequest.ComplexTourRequestId = -1;
                    RegularTourRequest.TourId = -1;
                    RegularTourRequest.SubmittingDate = DateTime.Now.AddDays(0);
                    RegularTourRequest.Status = TourRequestStatus.PENDING;
                    RegularTourRequest.DateRange.Start = RegularTourRequestValidation.EnteredStart;
                    RegularTourRequest.DateRange.End = RegularTourRequestValidation.EnteredEnd;

                    RegularTourRequest.Language = RegularTourRequestValidation.Language;
                    RegularTourRequest.Location.Country = RegularTourRequestValidation.Country;
                    RegularTourRequest.Location.City = RegularTourRequestValidation.City;
                    RegularTourRequest.Description = RegularTourRequestValidation.Description;
                    RegularTourRequest.GuestNumber = RegularTourRequestValidation.GuestNumber ?? 0;

                    _regularTourRequestService.Add(RegularTourRequest);
                    MessageBox.Show("Tour request is submited. You can see it in the list of your regular tour requests list.");
                    NavigationService.Navigate(new RequestsView(Guest, NavigationService, 1));
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
    }
}
