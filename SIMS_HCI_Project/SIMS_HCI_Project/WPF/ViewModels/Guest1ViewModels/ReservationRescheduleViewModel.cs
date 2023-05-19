using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Services;
using SIMS_HCI_Project.WPF.Views.Guest1Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class ReservationRescheduleViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private NavigationService _navigationService;
        private UserService _userService;
        private AccommodationReservationService _accommodationReservationService;
        private RescheduleRequestService _rescheduleRequestService;
        public AccommodationReservation Reservation { get; set; }
        public ObservableCollection<RescheduleRequest> RescheduleRequests { get; set; }
        public RelayCommand SendReservationRescheduleRequestCommand { get; set; }
        public string FullName { get; set; }

        private DateTime _wantedStart;
        public DateTime WantedStart
        {
            get => _wantedStart;
            set
            {
                if (value != _wantedStart)
                {
                    _wantedStart = value;
                    OnPropertyChanged(nameof(WantedStart));
                    Validate();
                }
            }
        }
        private DateTime _wantedEnd;
        public DateTime WantedEnd
        {
            get => _wantedEnd;
            set
            {
                if (value != _wantedEnd)
                {
                    _wantedEnd = value;
                    OnPropertyChanged(nameof(WantedEnd));
                    Validate();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ReservationRescheduleViewModel(NavigationService navigationService, AccommodationReservation reservation)
        {
            _navigationService = navigationService;
            _accommodationReservationService = new AccommodationReservationService();
            _rescheduleRequestService = new RescheduleRequestService(); 
            _userService = new UserService();
            Reservation = reservation;
            RescheduleRequests = new ObservableCollection<RescheduleRequest>(_rescheduleRequestService.GetAllByOwnerId(Reservation.Accommodation.OwnerId));
            FullName = Reservation.Guest.GetFullName();
            WantedStart = reservation.Start;
            WantedEnd = reservation.End;
            InitCommands();
        }
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(WantedStart))
                {
                    if (WantedStart <= DateTime.Today)
                    {
                        return "Date must be after today.";
                    }
                }
                else if (columnName == nameof(WantedEnd))
                {
                    if (WantedEnd <= DateTime.Today)
                    {
                        return "Date must be after today.";
                    }
                }

                if (columnName == nameof(WantedEnd) || columnName == nameof(WantedStart))
                {
                    if (WantedStart > WantedEnd)
                    {
                        return "Start date must be before the end date.";
                    }
                    else if ( ((WantedEnd - WantedStart).Days + 1 ) < Reservation.Accommodation.MinimumReservationDays)
                    {
                        return $"Minimum days for reservation is {Reservation.Accommodation.MinimumReservationDays}";
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
        }
        private readonly string[] _validatedProperties = { "WantedStart", "WantedEnd" };

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
        private void Validate()
        {
            OnPropertyChanged(nameof(WantedStart));
            OnPropertyChanged(nameof(WantedEnd));
        }
        public void ExecutedSendReservationRescheduleRequestCommand(object obj)
        {
            MessageBoxResult result = ConfirmRescheduleRequest();
            if (result == MessageBoxResult.Yes && IsValid)
            {
               _rescheduleRequestService.Add(new RescheduleRequest(Reservation, WantedStart, WantedEnd));
                _navigationService.NavigateBack();
            }
        }
        private MessageBoxResult ConfirmRescheduleRequest()
        {
            string sMessageBoxText = $"This reservation will be rescheduled, are you sure?";
            string sCaption = "Reschedule Confirm";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        public bool CanExecute(object obj)
        {
            return true;
        }
        public void InitCommands()
        {
            SendReservationRescheduleRequestCommand = new RelayCommand(ExecutedSendReservationRescheduleRequestCommand, CanExecute);
        }        
    }
}
