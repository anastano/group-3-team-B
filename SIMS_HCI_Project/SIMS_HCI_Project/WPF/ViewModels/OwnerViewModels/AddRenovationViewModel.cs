using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Validations;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class AddRenovationViewModel : INotifyPropertyChanged
    {
        private readonly RenovationService _renovationService;
        private readonly AccommodationService _accommodationService;
        public Owner Owner { get; set; }
        public List<Accommodation> OwnerAccommodations { get; set; }
        public Renovation SelectedRenovation { get; set; }
        public AddRenovationView AddRenovationView { get; set; }
        public RenovationsViewModel RenovationsVM { get; set; } 

        #region OnPropertyChanged

        private List<Renovation> _availableRenovations;
        public List<Renovation> AvailableRenovations
        {
            get => _availableRenovations;
            set
            {
                if (value != _availableRenovations)
                {
                    _availableRenovations = value;
                    OnPropertyChanged(nameof(AvailableRenovations));
                }
            }
        }

        private RenvationValidation _validatedRenovation;
        public RenvationValidation ValidatedRenovation
        {
            get => _validatedRenovation;
            set
            {
                if (value != _validatedRenovation)
                {
                    _validatedRenovation = value;
                    OnPropertyChanged(nameof(ValidatedRenovation));
                }
            }
        }

        private string _availableDatesText;
        public string AvailableDatesText
        {
            get => _availableDatesText;
            set
            {
                if (value != _availableDatesText)
                {
                    _availableDatesText = value;
                    OnPropertyChanged(nameof(AvailableDatesText));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public RelayCommand AddRenovationCommand { get; set; }
        public RelayCommand FindDatesCommand { get; set; }
        public RelayCommand CloseViewCommand { get; set; }
    
        public AddRenovationViewModel(AddRenovationView addRenovationView, RenovationsViewModel renovationsVM, Owner owner)
        {
            InitCommands();

            _renovationService = new RenovationService();
            _accommodationService = new AccommodationService();

            Owner = owner;
            AddRenovationView = addRenovationView;
            RenovationsVM = renovationsVM;

            ValidatedRenovation = new RenvationValidation();
            OwnerAccommodations = _accommodationService.GetByOwnerId(Owner.Id);

            AvailableRenovations = new List<Renovation>();
            AvailableDatesText = "";
        }

        #region Commands

        public void Executed_FindDatesCommand(object obj)
        {
            ValidatedRenovation.Validate();
            if (ValidatedRenovation.IsValid)
            {
                AvailableRenovations = _renovationService.GetAvailableRenovations(ValidatedRenovation.SelectedAccommodation, ValidatedRenovation.EnteredStart, ValidatedRenovation.EnteredEnd, ValidatedRenovation.DaysNumber ?? 1);
                if (AvailableRenovations.Count() == 0)
                {
                    AvailableDatesText = "There are not any available dates on those days.";
                }
                else 
                {
                    AvailableDatesText = "There are available dates on those days.";
                }
            }
            else 
            {
                MessageBox.Show("Not all fields are field in correctly.");
            }
        }

        public void Executed_AddRenovationCommand(object obj)
        {
            if (SelectedRenovation != null)
            {           
                 SelectedRenovation.Description = ValidatedRenovation.Description;
                 SelectedRenovation.AccommodationId = ValidatedRenovation.SelectedAccommodation.Id;
                 SelectedRenovation.Accommodation = ValidatedRenovation.SelectedAccommodation;
                 _renovationService.Add(SelectedRenovation);
                 AddRenovationView.Close();
                 RenovationsVM.UpdateRenovations();            
            }
            else 
            {
                MessageBox.Show("No date range has been selected.");
            }
        }

        public void Executed_CloseViewCommand(object obj)
        {
            AddRenovationView.Close();
        }

        #endregion

        public void InitCommands()
        {
            AddRenovationCommand = new RelayCommand(Executed_AddRenovationCommand);
            FindDatesCommand = new RelayCommand(Executed_FindDatesCommand);
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);

        }

    }
}
