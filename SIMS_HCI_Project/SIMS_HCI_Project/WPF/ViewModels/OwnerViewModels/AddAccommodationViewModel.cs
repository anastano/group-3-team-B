using LiveCharts.Helpers;
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
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class AddAccommodationViewModel : INotifyPropertyChanged
    {
        private readonly AccommodationService _accommodationService;
        public Owner Owner { get; set; }
        public Accommodation Accommodation { get; set; }
        public Location Location { get; set; }
        public AddAccommodationView AddAccommodationView { get; set; }
        public AccommodationsViewModel AccommodationsVM { get; set; }

        #region OnPropertyChanged    

        private ImageURLValidation _validatedimageURL;
        public ImageURLValidation ValidatedImageURL
        {
            get => _validatedimageURL;
            set
            {
                if (value != _validatedimageURL)
                {

                    _validatedimageURL = value;
                    OnPropertyChanged(nameof(ValidatedImageURL));
                }
            }
        }

        private AccommodationValidation _validatedAccommodation;
        public AccommodationValidation ValidatedAccommodation
        {
            get => _validatedAccommodation;
            set
            {
                if (value != _validatedAccommodation)
                {

                    _validatedAccommodation = value;
                    OnPropertyChanged(nameof(ValidatedAccommodation));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ObservableCollection<string> Images { get; set; }

        public RelayCommand AddAccommodationImageCommand { get; set; }
        public RelayCommand RemoveAccommodationImageCommand { get; set; }
        public RelayCommand RegisterNewAccommodationCommand { get; set; }
        public RelayCommand CloseAddAccommodationViewCommand { get; set; }
        public RelayCommand StopDemoCommand { get; set; }

        public AddAccommodationViewModel(AddAccommodationView addAccommodationView, AccommodationsViewModel accommodationsVM, Owner owner)
        {
            InitCommands();

            _accommodationService = new AccommodationService();
            AddAccommodationView = addAccommodationView;
            AccommodationsVM = accommodationsVM;

            Owner = owner;
            ValidatedAccommodation = new AccommodationValidation();
            Accommodation = new Accommodation();
            ValidatedImageURL = new ImageURLValidation();
       
            Location = new Location();
            Images = new ObservableCollection<string>();

            CancellationToken CT = OwnerMainViewModel.CTS.Token;
            DemoIsOn(CT);
        }

        #region DemoIsOn
        private async Task DemoIsOn(CancellationToken CT)
        {
            if (OwnerMainViewModel.Demo)
            {
                await Task.Delay(500, CT);
                ValidatedAccommodation.Name = "Hotel 123";
                await Task.Delay(1000, CT);
                ValidatedAccommodation.Country = "Country 123";
                await Task.Delay(1000, CT);
                ValidatedAccommodation.City = "City 123";
                await Task.Delay(1000, CT);
                Accommodation.Type = AccommodationType.APARTMENT;
                await Task.Delay(1000, CT);
                ValidatedAccommodation.MaxGuests = 2;
                await Task.Delay(1000, CT);
                ValidatedAccommodation.MinDays = 2;
                await Task.Delay(1000, CT);
                ValidatedAccommodation.CancellationDays = 2;
                await Task.Delay(1000, CT);
                ValidatedImageURL.ImageURL = "https://s3.eu-central-1.amazonaws.com/apartmani-u-beogradu/uploads/apartmani/9073/sr/main/apartmani-beograd-centar-apartman-retro-cosy-apartment4.jpg";
                await Task.Delay(1000, CT);
                Style style = Application.Current.FindResource("OwnerSelectedCircleButtonStyle") as Style;
                AddAccommodationView.btnAddImage.Style = style;
                await Task.Delay(1000, CT);
                Style style2 = Application.Current.FindResource("OwnerCircleButtonStyle") as Style;
                AddAccommodationView.btnAddImage.Style = style2;
                Images.Add(ValidatedImageURL.ImageURL);
                ValidatedImageURL.ImageURL = "";
                await Task.Delay(1000, CT);
                Style style3 = Application.Current.FindResource("OwnerSelectedButtonStyle") as Style;
                AddAccommodationView.btnRegister.Style = style3;
                await Task.Delay(1000, CT);
                Style style4 = Application.Current.FindResource("OwnerButtonStyle") as Style;
                AddAccommodationView.btnRegister.Style = style4;
              
                await Task.Delay(1000, CT);
                AddAccommodationView.Close();
            }
        }
        #endregion

        #region Commands
        public void Executed_AddAccommodationImageCommand(object obj)
        {
            ValidatedImageURL.Validate();
            if (ValidatedImageURL.IsValid)
            {
                Images.Add(ValidatedImageURL.ImageURL);
                ValidatedImageURL.ImageURL = "";
            }
        }

        public bool CanExecute_AddAccommodationImageCommand(object obj)
        {
            return true;
        }

        public void Executed_RemoveAccommodationImageCommand(object obj)
        {
            if (AddAccommodationView.lbImages.SelectedItem != null)
            {
                Images.RemoveAt(AddAccommodationView.lbImages.SelectedIndex);
            }
        }

        public bool CanExecute_RemoveAccommodationImageCommand(object obj)
        {
            return true;
        }

        private MessageBoxResult ConfirmRegisterNewAccommodation()
        {
            string sMessageBoxText = $"Are you sure you want to register this accommodation?";
            string sCaption = "Add Accommodation Confirmation";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        public void Executed_RegisterNewAccommodationCommand(object obj)
        {
            ValidatedAccommodation.Validate();
            if (ValidatedAccommodation.IsValid)
            {
                if (ConfirmRegisterNewAccommodation() == MessageBoxResult.Yes)
                {
                    Location.City = ValidatedAccommodation.City;
                    Location.Country = ValidatedAccommodation.Country;

                    Accommodation.OwnerId = Owner.Id;
                    Accommodation.Name = ValidatedAccommodation.Name;
                    Accommodation.MaxGuests = ValidatedAccommodation.MaxGuests ?? 0;
                    Accommodation.MinimumReservationDays = ValidatedAccommodation.MinDays ?? 0;
                    Accommodation.CancellationDeadlineInDays = ValidatedAccommodation.CancellationDays ?? 0;

                    Accommodation.Images = new List<string>(Images);

                    _accommodationService.Add(Accommodation, Location);

                    AddAccommodationView.Close();
                    AccommodationsVM.UpdateAccommodations();
                }
            }
            else
            {
                MessageBox.Show("Not all fields are filled in correctly.");
            }
            
        }

        public bool CanExecute_RegisterNewAccommodationCommand(object obj)
        {
            return true;
        }

        private async Task StopDemo()
        {
            if (OwnerMainViewModel.Demo)
            {
                OwnerMainViewModel.CTS.Cancel();
                OwnerMainViewModel.Demo = false;

                //demo message - end
                Window messageDemoOver = new MessageView("The demo mode is over.", "");
                messageDemoOver.Show();
                await Task.Delay(2500);
                messageDemoOver.Close();

                AddAccommodationView.Close();
                AccommodationsVM.AccommodationsView.Close();
            }
        }

        public void Executed_StopDemoCommand(object obj)
        {
            try
            {
                StopDemo();
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Error!");
            }
        }

        public bool CanExecute_StopDemoCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseAddAccommodationViewCommand(object obj)
        {
            AddAccommodationView.Close();
        }

        public bool CanExecute_CloseAddAccommodationViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            AddAccommodationImageCommand = new RelayCommand(Executed_AddAccommodationImageCommand, CanExecute_AddAccommodationImageCommand);
            RemoveAccommodationImageCommand = new RelayCommand(Executed_RemoveAccommodationImageCommand, CanExecute_RemoveAccommodationImageCommand);
            RegisterNewAccommodationCommand = new RelayCommand(Executed_RegisterNewAccommodationCommand, CanExecute_RegisterNewAccommodationCommand);
            CloseAddAccommodationViewCommand = new RelayCommand(Executed_CloseAddAccommodationViewCommand, CanExecute_CloseAddAccommodationViewCommand);
            StopDemoCommand = new RelayCommand(Executed_StopDemoCommand, CanExecute_StopDemoCommand);
        }      

    }
}
