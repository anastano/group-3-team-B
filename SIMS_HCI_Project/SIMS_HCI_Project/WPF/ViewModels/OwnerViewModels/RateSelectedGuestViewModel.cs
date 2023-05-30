using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class RateSelectedGuestViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly RatingGivenByOwnerService _ownerRatingService;
        public RateSelectedGuestView RateSelectedGuestView { get; set; }
        public UnratedReservationsViewModel UnratedReservationsVM { get; set; }
        public RatingGivenByOwner Rating { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }

        #region OnPropertyChanged
        private int? _cleanliness;
        public int? Cleanliness
        {
            get => _cleanliness;
            set
            {
                if (value != _cleanliness)
                {

                    _cleanliness = value;
                    OnPropertyChanged(nameof(Cleanliness));
                }
            }
        }

        private int? _ruleCompliance;
        public int? RuleCompliance
        {
            get => _ruleCompliance;
            set
            {
                if (value != _ruleCompliance)
                {

                    _ruleCompliance = value;
                    OnPropertyChanged(nameof(RuleCompliance));
                }
            }
        }

        #endregion

        public RelayCommand RateGuestCommand { get; set; }
        public RelayCommand CloseRateSelectedGuestViewCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RateSelectedGuestViewModel(RateSelectedGuestView rateSelectedGuestView, UnratedReservationsViewModel unratedReservationsVM, AccommodationReservation selectedReservation) 
        {
            InitCommands();
            _ownerRatingService = new RatingGivenByOwnerService();
            RateSelectedGuestView = rateSelectedGuestView;
            UnratedReservationsVM = unratedReservationsVM;
            Rating = new RatingGivenByOwner();
            SelectedReservation = selectedReservation;
            Cleanliness = null;
            RuleCompliance = null;
        }

        #region Commands

        private MessageBoxResult ConfirmRateGuest()
        {
            string sMessageBoxText = $"Are you sure you want to rate this guest?";
            string sCaption = "Rate Guest Confirmation";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }
        public void Executed_RateGuestCommand(object obj)
        {
            if (IsValid)
            {
                if (ConfirmRateGuest() == MessageBoxResult.Yes)
                {
                    Rating.ReservationId = SelectedReservation.Id;
                    Rating.Cleanliness = Cleanliness ?? 0;
                    Rating.RuleCompliance = RuleCompliance ?? 0;
                    Rating.Reservation = SelectedReservation;
                    _ownerRatingService.Add(Rating);
                    RateSelectedGuestView.Close();
                    UnratedReservationsVM.UpdateUnratedReservations();
                }
            }
            else 
            {
                MessageBox.Show("Not all fields are filled in correctly.");
            }
        }

        public bool CanExecute_RateGuestCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseRateSelectedGuestViewCommand(object obj)
        {
            RateSelectedGuestView.Close();
        }

        public bool CanExecute_CloseRateSelectedGuestViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            RateGuestCommand = new RelayCommand(Executed_RateGuestCommand, CanExecute_RateGuestCommand);
            CloseRateSelectedGuestViewCommand = new RelayCommand(Executed_CloseRateSelectedGuestViewCommand, CanExecute_CloseRateSelectedGuestViewCommand);
        }

        #region Validation

        public string Error => null;

        public string this[string columnName]
        {
            get
            {

                if (columnName == "Cleanliness")
                {
                    if (Cleanliness == null)
                        return "Cleanliness is required";

                    if (Cleanliness < 1 || Cleanliness>5)
                    {
                        return "Rating can be between 1 and 5";
                    }
                }
                else if (columnName == "RuleCompliance")
                {
                    if (RuleCompliance == null)
                        return "Rule following is required";

                    if (RuleCompliance < 1 || RuleCompliance > 5)
                    {
                        return "Rating can be between 1 and 5";
                    }
                }


                return null;
            }
        }

        private readonly string[] _validatedProperties = { "Cleanliness", "RuleCompliance"};

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
