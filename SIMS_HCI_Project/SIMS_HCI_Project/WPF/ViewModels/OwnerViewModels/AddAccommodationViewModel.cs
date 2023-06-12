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
        public string SelectedImage { get; set; }
        public Style NormalButtonStyle { get; set; }
        public Style SelectedButtonStyle { get; set; }
        public Style NormalCircleButtonStyle { get; set; }
        public Style SelectedCircleButtonStyle { get; set; }

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

        private ObservableCollection<string> _images;
        public ObservableCollection<string> Images
        {
            get => _images;
            set
            {
                if (value != _images)
                {
                    _images = value;
                    OnPropertyChanged(nameof(Images));
                }
            }
        }

        private Style _addImageButtonStyle;
        public Style AddImageButtonStyle
        {
            get => _addImageButtonStyle;
            set
            {
                if (value != _addImageButtonStyle)
                {
                    _addImageButtonStyle = value;
                    OnPropertyChanged(nameof(AddImageButtonStyle));
                }
            }
        }

        private Style _registerButtonStyle;
        public Style RegisterButtonStyle
        {
            get => _registerButtonStyle;
            set
            {
                if (value != _registerButtonStyle)
                {
                    _registerButtonStyle = value;
                    OnPropertyChanged(nameof(RegisterButtonStyle));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public RelayCommand AddImageCommand { get; set; }
        public RelayCommand RemoveImageCommand { get; set; }
        public RelayCommand RegisterAccommodationCommand { get; set; }
        public RelayCommand CloseViewCommand { get; set; }
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

            NormalButtonStyle = Application.Current.FindResource("OwnerButtonStyle") as Style;
            SelectedButtonStyle = Application.Current.FindResource("OwnerSelectedButtonStyle") as Style;
            NormalCircleButtonStyle = Application.Current.FindResource("OwnerCircleButtonStyle") as Style;
            SelectedCircleButtonStyle = Application.Current.FindResource("OwnerSelectedCircleButtonStyle") as Style;
            AddImageButtonStyle = NormalCircleButtonStyle;
            RegisterButtonStyle = NormalButtonStyle;

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
                AddImageButtonStyle = SelectedCircleButtonStyle;
                await Task.Delay(1000, CT);
                AddImageButtonStyle = NormalCircleButtonStyle;
                Images.Add(ValidatedImageURL.ImageURL);
                ValidatedImageURL.ImageURL = "";
                await Task.Delay(1000, CT);
                RegisterButtonStyle = SelectedButtonStyle;
                await Task.Delay(1000, CT);
                RegisterButtonStyle = NormalButtonStyle;
              
                await Task.Delay(1000, CT);
                AddAccommodationView.Close();
            }
        }
        #endregion

        #region Commands
        public void Executed_AddImageCommand(object obj)
        {
            ValidatedImageURL.Validate();
            if (ValidatedImageURL.IsValid)
            {
                Images.Add(ValidatedImageURL.ImageURL);
                ValidatedImageURL.ImageURL = "";
            }
        }

        public void Executed_RemoveImageCommand(object obj)
        {
            if (SelectedImage != null)
            {
                Images.Remove(SelectedImage);
            }
            else 
            {
                MessageBox.Show("No image has been selected");
            }
        }

        public void Executed_RegisterAccommodationCommand(object obj)
        {
            ValidatedAccommodation.Validate();
            if (ValidatedAccommodation.IsValid)
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
            else
            {
                MessageBox.Show("Not all fields are filled in correctly.");
            }
            
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

        public void Executed_CloseViewCommand(object obj)
        {
            AddAccommodationView.Close();
        }

        #endregion

        public void InitCommands()
        {
            AddImageCommand = new RelayCommand(Executed_AddImageCommand);
            RemoveImageCommand = new RelayCommand(Executed_RemoveImageCommand);
            RegisterAccommodationCommand = new RelayCommand(Executed_RegisterAccommodationCommand);
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
            StopDemoCommand = new RelayCommand(Executed_StopDemoCommand);
        }      

    }
}
