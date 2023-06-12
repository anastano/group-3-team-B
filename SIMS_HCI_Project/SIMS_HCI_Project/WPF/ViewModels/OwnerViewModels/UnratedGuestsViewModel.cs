using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;

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
    public class UnratedGuestsViewModel
    {
        private readonly AccommodationReservationService _reservationService;
        private readonly RatingGivenByOwnerService _ownerRatingService;
        public UnratedGuestsView UnratedGuestsView { get; set; }
        public OwnerMainViewModel OwnerMainVM { get; set; }
        public Owner Owner { get; set; }
        public ObservableCollection<AccommodationReservation> UnratedReservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }
        public RelayCommand ShowRatingViewCommand { get; set; }
        public RelayCommand CloseViewCommand { get; set; }

        public UnratedGuestsViewModel(UnratedGuestsView unratedGuestsView, OwnerMainViewModel ownerMainVM, Owner owner) 
        { 
            InitCommands();

            _reservationService = new AccommodationReservationService();
            _ownerRatingService = new RatingGivenByOwnerService();

            UnratedGuestsView = unratedGuestsView;
            OwnerMainVM = ownerMainVM;
            Owner = owner;
            UnratedReservations = new ObservableCollection<AccommodationReservation>(_ownerRatingService.GetUnratedReservations(Owner.Id, _reservationService));
        }

        #region Commands
        public void Executed_ShowRatingViewCommand(object obj)
        {     
            if (SelectedReservation != null)
            {
                Window rateSelectedGuestView = new RateGuestView(this, SelectedReservation);
                rateSelectedGuestView.Show();
            }
            else 
            {
                MessageBox.Show("No guest has been selected.");
            }
        }

        public void Executed_CloseViewCommand(object obj)
        {
            UnratedGuestsView.Close();
            OwnerMainVM.UpdateNotifications();
        }

        #endregion

        public void InitCommands()
        {
            ShowRatingViewCommand = new RelayCommand(Executed_ShowRatingViewCommand);
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
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
