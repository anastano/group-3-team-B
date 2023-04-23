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

        public AccommodationsView AccommodationsView { get; set; }
        public Owner Owner { get; set; }        
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        public RelayCommand DeleteAccommodationCommand { get; set; }
        public RelayCommand AddAccommodationCommand { get; set; }
        public RelayCommand ShowAccommodationImagesCommand { get; set; }
        public RelayCommand CloseAccommodationsViewCommand { get; set; }

        public AccommodationsViewModel(AccommodationsView accommodationsView, AccommodationService accommodationService, Owner owner)
        {
            InitCommands();

            _accommodationService = accommodationService;

            AccommodationsView = accommodationsView;
            Owner = owner;            
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetByOwnerId(Owner.Id));

            _accommodationService.Subscribe(this);
        }

        #region Commands
        public void Executed_DeleteAccommodationCommand(object obj)
        {
            if (SelectedAccommodation != null)
            {
                _accommodationService.Delete(SelectedAccommodation);
            }
        }

        public bool CanExecute_DeleteAccommodationCommand(object obj)
        {
            return true;
        }

        public void Executed_AddAccommodationCommand(object obj)
        {
            Window addAccommodationView = new AddAccommodationView(_accommodationService, Owner);
            addAccommodationView.ShowDialog();
        }

        public bool CanExecute_AddAccommodationCommand(object obj)
        {
            return true;
        }

        public void Executed_ShowAccommodationImagesCommand(object obj)
        {
            if (SelectedAccommodation != null)
            {
                Window accommodationImagesView = new AccommodationImagesView(_accommodationService, SelectedAccommodation);
                accommodationImagesView.ShowDialog();
            }
        }

        public bool CanExecute_ShowAccommodationImagesCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseAccommodationsViewCommand(object obj)
        {
            AccommodationsView.Close();
        }

        public bool CanExecute_CloseAccommodationsViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            DeleteAccommodationCommand = new RelayCommand(Executed_DeleteAccommodationCommand, CanExecute_DeleteAccommodationCommand);
            AddAccommodationCommand = new RelayCommand(Executed_AddAccommodationCommand, CanExecute_AddAccommodationCommand);
            ShowAccommodationImagesCommand = new RelayCommand(Executed_ShowAccommodationImagesCommand, CanExecute_ShowAccommodationImagesCommand);
            CloseAccommodationsViewCommand = new RelayCommand(Executed_CloseAccommodationsViewCommand, CanExecute_CloseAccommodationsViewCommand);
        }

        public void Update()
        {
            UpdateAccommodations();
        }

        public void UpdateAccommodations()
        {
            Accommodations.Clear();
            foreach (Accommodation accommodation in _accommodationService.GetByOwnerId(Owner.Id))
            {
                Accommodations.Add(accommodation);
            }

            foreach (Accommodation accommodation in Accommodations)
            {
                accommodation.FirstImage = accommodation.Images.FirstOrDefault();
            }
        }
    }
}
