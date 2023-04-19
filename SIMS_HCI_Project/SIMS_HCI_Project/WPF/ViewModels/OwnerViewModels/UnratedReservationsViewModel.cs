using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class UnratedReservationsViewModel : IObserver
    {
        private readonly AccommodationReservationService _reservationService;
        private readonly RatingGivenByOwnerService _ownerRatingService;

        public UnratedReservationsView UnratedReservationsView { get; set; }
        public Owner Owner { get; set; }
        public ObservableCollection<AccommodationReservation> UnratedReservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }

        public RelayCommand ShowSelectedUnratedReservationCommand { get; set; }

        public UnratedReservationsViewModel(UnratedReservationsView unratedReservationsView, AccommodationReservationService reservationService, 
            RatingGivenByOwnerService ownerRatingService, Owner owner) 
        { 
            InitCommands();

            _reservationService = reservationService;
            _ownerRatingService = ownerRatingService;

            UnratedReservationsView = unratedReservationsView;
            Owner = owner;
            UnratedReservations = new ObservableCollection<AccommodationReservation>(_ownerRatingService.GetUnratedReservations(Owner.Id, _reservationService));

            _ownerRatingService.Subscribe(this);
        }

        #region Commands
        public void Executed_ShowSelectedUnratedReservationCommand(object obj)
        {
            
            if (SelectedReservation != null)
            {
                Window rateSelectedGuestView = new RateSelectedGuestView(_ownerRatingService, SelectedReservation);
                rateSelectedGuestView.Show();
            }
            
        }

        public bool CanExecute_ShowSelectedUnratedReservationCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            ShowSelectedUnratedReservationCommand = new RelayCommand(Executed_ShowSelectedUnratedReservationCommand, CanExecute_ShowSelectedUnratedReservationCommand);
        }

        public void Update()
        {
            UpdateUnratedReservations();
        }

        public void UpdateUnratedReservations()
        {
            UnratedReservations.Clear();
            foreach (AccommodationReservation reservation in _ownerRatingService.GetUnratedReservations(Owner.Id, _reservationService))
            {
                UnratedReservations.Add(reservation);
            }
        }

    }
}
