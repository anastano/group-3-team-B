using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Validations;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class CreateComplexRequestViewModel : INotifyPropertyChanged
    {
        public NavigationService NavigationService { get; set; }
        public Guest2 Guest { get; set; }
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

        private RegularTourRequest _regularRequestPart;
        public RegularTourRequest RegularRequestPart
        {
            get => _regularRequestPart;
            set
            {
                _regularRequestPart = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<RegularTourRequest> _userRequests;
        public ObservableCollection<RegularTourRequest> UserRequests
        {
            get => _userRequests;
            set
            {
                _userRequests = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public RelayCommand CompleteRequest { get; set; }
        public RelayCommand QuitRequest { get; set; }
        public RelayCommand AddRegularTourRequest { get; set; }
        public RegularTourRequest RegularTourRequest { get; set; }
        public ComplexTourRequest ComplexTourRequest { get; set; }
        public RegularTourRequest SelectedPart { get; set; }
        private RegularTourRequestService _regularTourRequestService { get; set; }
        private ComplexTourRequestsService _complexTourRequestsService { get; set; }

        public CreateComplexRequestViewModel(Guest2 guest, NavigationService navigationService)
        {
            Guest = guest;
            NavigationService = navigationService;
            RegularTourRequest = new RegularTourRequest();
            ComplexTourRequest = new ComplexTourRequest(); 
            RegularRequestPart = new RegularTourRequest(); 
            RegularTourRequest.Location = new Location();
            RegularTourRequestValidation = new RegularTourRequestValidation();
            UserRequests = new ObservableCollection<RegularTourRequest>(); 
            SelectedPart = new RegularTourRequest();


            Start = DateTime.Now.AddDays(3); //48h before requests turns invalid
            End = DateTime.Now.AddDays(4);
            ComplexTourRequest.GuestId = Guest.Id;
            ComplexTourRequest.Guest = Guest;

            InitCommands();
            LoadFromFiles();


        }

        private void LoadFromFiles()
        {
            _regularTourRequestService = new RegularTourRequestService();
            _complexTourRequestsService = new ComplexTourRequestsService();
        }

        private void InitCommands()
        {
            CompleteRequest = new RelayCommand(ExecuteCompleteRequest, CanExecute);
            QuitRequest = new RelayCommand(ExecuteQuitRequest, CanExecute);
            AddRegularTourRequest = new RelayCommand(ExecuteAddRegularTourRequest, CanExecute);
        }

        private void ExecuteAddRegularTourRequest(object obj)
        {
            RegularTourRequestValidation.Validate();
            if (RegularTourRequestValidation.IsValid)
            {
                if (ConfirmRequestSubmission() == MessageBoxResult.Yes)
                {
                    RegularTourRequest.GuestId = Guest.Id;
                    RegularTourRequest.Guest = Guest;
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

                    RegularTourRequest TEMP = new RegularTourRequest(RegularTourRequest);

                    ComplexTourRequest.TourRequests.Add(TEMP);


                    UserRequests.Add(TEMP);
                    _regularTourRequestService.Add(TEMP);
                    MessageBox.Show("Tour request is added to list. You can add more requests");

                    RegularTourRequestValidation.Language = string.Empty;
                    RegularTourRequestValidation.Country = string.Empty;
                    RegularTourRequestValidation.City = string.Empty;
                    RegularTourRequestValidation.Description = string.Empty;
                    RegularTourRequestValidation.GuestNumber = null;
                }
            }
            else
            {
                MessageBox.Show("Not all fields are filled in correctly");
            }
        }

        private MessageBoxResult ConfirmRequestSubmission()
        {
            string sMessageBoxText = $"Are you sure you want to submit this request?";
            string sCaption = "Complex tour request submission";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        private MessageBoxResult ConfirmRequestQuit()
        {
            string sMessageBoxText = $"Are you sure you want to quit this request?";
            string sCaption = "Complex tour request submission";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        private void ExecuteQuitRequest(object obj)
        {
            if (ConfirmRequestQuit() == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new RequestsView(Guest, NavigationService, 1));

            }
        }

        private void ExecuteCompleteRequest(object obj)
        {
            if (ConfirmRequestSubmission() == MessageBoxResult.Yes)
            {
                _complexTourRequestsService.Add(ComplexTourRequest);
                foreach(RegularTourRequest requestsList in UserRequests)
                {
                    requestsList.ComplexTourRequestId = ComplexTourRequest.Id;
                    requestsList.ComplexTourRequest = ComplexTourRequest;                
                }
                _complexTourRequestsService.Update(ComplexTourRequest);
                SetComplexTourIdForRegular();
                MessageBox.Show("Complex tour request is submited. You can see it in the list of your complex tour requests list.");
                NavigationService.Navigate(new RequestsView(Guest, NavigationService, 1)); //postavi da ide na odgovarajuci tab
            }
        }

        private void SetComplexTourIdForRegular() 
        {
            foreach (RegularTourRequest regularTourRequest in UserRequests)
            {
                RegularTourRequest RequestForUpdate = new RegularTourRequest();
                RequestForUpdate = _regularTourRequestService.GetById(regularTourRequest.Id);
                RequestForUpdate.ComplexTourRequestId = ComplexTourRequest.Id;
                RequestForUpdate.ComplexTourRequest = ComplexTourRequest;
                
                _regularTourRequestService.Update(RequestForUpdate);
            }
        }

        private bool CanExecute(object obj)
        {
            return true;
        }
    }
}
