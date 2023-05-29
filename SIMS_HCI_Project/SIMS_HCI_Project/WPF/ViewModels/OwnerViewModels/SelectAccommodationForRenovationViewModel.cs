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
    class SelectAccommodationForRenovationViewModel
    {
        private readonly AccommodationService _accommodationService;
        public SelectAccommodationForRenovationView SelectAccommodationForRenovationView { get; set; }
        public RenovationsViewModel RenovationsVM { get; set; }
        public Owner Owner { get; set; }
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        public RelayCommand SelectAccommodationForRenovationCommand { get; set; }
        public RelayCommand CloseSelectAccommodationForRenovationViewCommand { get; set; }

        public SelectAccommodationForRenovationViewModel(SelectAccommodationForRenovationView selectAccommodationForRenovationView, RenovationsViewModel renovationsVM, Owner owner)
        {
            InitCommands();

            _accommodationService = new AccommodationService();

            SelectAccommodationForRenovationView = selectAccommodationForRenovationView;
            RenovationsVM = renovationsVM;
            Owner = owner;
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetByOwnerId(Owner.Id));
        }

        #region Commands

        public void Executed_SelectAccommodationForRenovationCommand(object obj)
        {
            if (SelectedAccommodation != null)
            {
                Window addRenovationView = new AddRenovationView(SelectAccommodationForRenovationView, RenovationsVM, SelectedAccommodation);
                addRenovationView.ShowDialog();
            }
            else
            {
                MessageBox.Show("No accommodation has been selected");
            }
        }

        public bool CanExecute_SelectAccommodationForRenovationCommand(object obj)
        {
            return true;
        }


        public void Executed_CloseSelectAccommodationForRenovationViewCommand(object obj)
        {
            SelectAccommodationForRenovationView.Close();
        }

        public bool CanExecute_CloseSelectAccommodationForRenovationViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            SelectAccommodationForRenovationCommand = new RelayCommand(Executed_SelectAccommodationForRenovationCommand, CanExecute_SelectAccommodationForRenovationCommand);
            CloseSelectAccommodationForRenovationViewCommand = new RelayCommand(Executed_CloseSelectAccommodationForRenovationViewCommand, CanExecute_CloseSelectAccommodationForRenovationViewCommand);
        }

    }
}
