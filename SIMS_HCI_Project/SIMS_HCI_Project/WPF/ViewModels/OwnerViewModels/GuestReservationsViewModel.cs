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
    public class GuestReservationsViewModel
    {
        private readonly AccommodationReservationService _reservationService;

        public GuestReservationsView GuestReservationsView { get; set; }
        public Owner Owner { get; set; }
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }
        public string AccommodationName { get; set; }
        public string GuestName { get; set; }
        public string GuestSurname { get; set; }
        public RelayCommand SearchReservationsCommand { get; set; }
        public RelayCommand CloseReservationsViewCommand { get; set; }

        public GuestReservationsViewModel(GuestReservationsView accommodationsView, AccommodationReservationService reservationService, Owner owner)
        {
            InitCommands();

            _reservationService = reservationService;

            GuestReservationsView = accommodationsView;
            Owner = owner;
            Reservations = new ObservableCollection<AccommodationReservation>(_reservationService.GetByOwnerId(Owner.Id));
        }

        #region Commands

        public void Executed_SearchReservationsCommand(object obj)
        {
            List<AccommodationReservation> searchResult = _reservationService.OwnerSearch(GuestReservationsView.txtAccommodationName.Text, GuestName, GuestSurname, Owner.Id);
            Reservations.Clear();
            foreach (AccommodationReservation reservation in searchResult)
            {
                Reservations.Add(reservation);
            }
        }

        public bool CanExecute_SearchReservationsCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseReservationsViewCommand(object obj)
        {
            GuestReservationsView.Close();
        }

        public bool CanExecute_CloseReservationsViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            SearchReservationsCommand = new RelayCommand(Executed_SearchReservationsCommand, CanExecute_SearchReservationsCommand);
            CloseReservationsViewCommand = new RelayCommand(Executed_CloseReservationsViewCommand, CanExecute_CloseReservationsViewCommand);
        }

    }
}
