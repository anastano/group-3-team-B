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
    public class RenovationsViewModel
    {
        private readonly RenovationService _renovationService;
        private readonly AccommodationService _accommodationService;
        private readonly AccommodationReservationService _reservationService;
        public RenovationsView RenovationsView { get; set; }
        public Owner Owner { get; set; }
        public ObservableCollection<Renovation> Renovations { get; set; }
        public Renovation SelectedRenovation { get; set; }

        public RelayCommand CancelRenovationCommand { get; set; }
        public RelayCommand AddRenovationCommand { get; set; }
        public RelayCommand CreatePDFReportCommand { get; set; }
        public RelayCommand CloseRenovationsViewCommand { get; set; }

        public RenovationsViewModel(RenovationsView renovationsView, RenovationService renovationService, AccommodationReservationService reservationService,
            AccommodationService accommodationService, Owner owner)
        {
            InitCommands();

            _renovationService = renovationService;
            _accommodationService = accommodationService;
            _reservationService = reservationService;

            RenovationsView = renovationsView;
            Owner = owner;
            Renovations = new ObservableCollection<Renovation>(_renovationService.GetByOwnerId(Owner.Id));
        }

        #region Commands
        public void Executed_CancelRenovationCommand(object obj)
        {
            if (_renovationService.CancelRenovation(SelectedRenovation))
            {
                UpdateRenovations();
                MessageBox.Show("Renovation cancelled successfully");
            }
            else 
            {
                MessageBox.Show("Renovation can't be cancelled");
            }
        }

        public bool CanExecute_CancelRenovationCommand(object obj)
        {
            return true;
        }

        public void Executed_AddRenovationCommand(object obj)
        {
            Window selectAccommodationForRenovation = new SelectAccommodationForRenovationView(this, _accommodationService, _renovationService, _reservationService, Owner);
            selectAccommodationForRenovation.ShowDialog();
        }

        public bool CanExecute_AddRenovationCommand(object obj)
        {
            return true;
        }

        public void Executed_CreatePDFReportCommand(object obj)
        {
            Window createPDFView = new CreatePDFView(_renovationService, Owner);
            createPDFView.ShowDialog();
        }

        public bool CanExecute_CreatePDFReportCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseRenovationsViewCommand(object obj)
        {
            RenovationsView.Close();
        }

        public bool CanExecute_CloseRenovationsViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            CancelRenovationCommand = new RelayCommand(Executed_CancelRenovationCommand, CanExecute_CancelRenovationCommand);
            AddRenovationCommand = new RelayCommand(Executed_AddRenovationCommand, CanExecute_AddRenovationCommand);
            CreatePDFReportCommand = new RelayCommand(Executed_CreatePDFReportCommand, CanExecute_CreatePDFReportCommand);
            CloseRenovationsViewCommand = new RelayCommand(Executed_CloseRenovationsViewCommand, CanExecute_CloseRenovationsViewCommand);
        }

        public void UpdateRenovations()
        {
            Renovations.Clear();
            foreach (Renovation renovation in _renovationService.GetByOwnerId(Owner.Id))
            {
                Renovations.Add(renovation);
            }
        }
    }
}
