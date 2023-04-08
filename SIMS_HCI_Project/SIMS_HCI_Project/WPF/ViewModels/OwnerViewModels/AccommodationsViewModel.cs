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
    public class AccommodationsViewModel: IObserver
    {

        private readonly AccommodationService _accommodationService;

        public Owner Owner { get; set; }
        public AccommodationsView AccommodationsView { get; set; }      
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        public RelayCommand DeleteAccommodationCommand { get; set; }

        public AccommodationsViewModel(AccommodationService accommodationService, Owner owner)
        {
            InitCommands();

            Owner = owner;
            _accommodationService = accommodationService;
            Accommodations = new ObservableCollection<Accommodation>(Owner.Accommodations);           

            _accommodationService.Subscribe(this);
        }

        #region Commands
        public void Executed_DeleteAccommodationCommand(object obj)
        {
            if (SelectedAccommodation != null)
            {
                _accommodationService.Delete(SelectedAccommodation, Owner);
            }
        }

        public bool CanExecute_DeleteAccommodationCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            DeleteAccommodationCommand = new RelayCommand(Executed_DeleteAccommodationCommand, CanExecute_DeleteAccommodationCommand);
        }

        public void Update()
        {
            UpdateAccommodations();
        }

        public void UpdateAccommodations()
        {
            Accommodations.Clear();
            foreach (Accommodation accommodation in Owner.Accommodations)
            {
                Accommodations.Add(accommodation);
            }
        }
    }
}
