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
    public class OwnerMainViewModel: IObserver
    {
        #region Service Fields
        private OwnerService _ownerService;
        private Guest1Service _guest1Service;
        private LocationService _locationService;
        private AccommodationService _accommodationService;
        private AccommodationReservationService _accommodationReservationService;
        #endregion

        public Owner Owner { get; set; }
        public OwnerMainView OwnerMainView { get; set; }
        public ObservableCollection<AccommodationReservation> ReservationsInProgress { get; set; }

        public RelayCommand ShowAccommodationsCommand { get; set; }

        public OwnerMainViewModel(Owner owner, OwnerMainView ownerMainView) 
        {                           
            Owner = owner;
            OwnerMainView = ownerMainView;

            LoadFromFiles();
            InitCommands();

            ReservationsInProgress = new ObservableCollection<AccommodationReservation>(_accommodationReservationService.GetInProgressByOwnerId(Owner.Id));
        }

        public void LoadFromFiles()
        {
            _ownerService = new OwnerService();
            _guest1Service = new Guest1Service();
            _locationService = new LocationService();
            _accommodationService = new AccommodationService();
            _accommodationReservationService = new AccommodationReservationService();

            _accommodationService.ConnectAccommodationsWithLocations(_locationService);
            _accommodationReservationService.ConnectReservationsWithAccommodations(_accommodationService);

            _accommodationService.FillOwnerAccommodationList(Owner);
            _accommodationReservationService.FillOwnerReservationList(Owner);
        }

        #region Commands
        public void Executed_ShowAccommodationsCommand(object obj)
        {
            Window accommodationsView = new AccommodationsView(_accommodationService, Owner);
            accommodationsView.Show();
        }

        public bool CanExecute_ShowAccommodationsCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands() 
        {
            ShowAccommodationsCommand = new RelayCommand(Executed_ShowAccommodationsCommand,
                CanExecute_ShowAccommodationsCommand);
        }

        public void Update()
        {
            UpdateReservationsInProgress();
        }

        public void UpdateReservationsInProgress()
        {
            ReservationsInProgress.Clear();
            foreach (AccommodationReservation reservation in _accommodationReservationService.GetInProgressByOwnerId(Owner.Id))
            {
                ReservationsInProgress.Add(reservation);
            }
        }
    }
}
