using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class GuestReservationsViewModel : INotifyPropertyChanged
    {
        private readonly AccommodationReservationService _reservationService;

        public GuestReservationsView GuestReservationsView { get; set; }
        public Owner Owner { get; set; }
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }
        public Style NormalButtonStyle { get; set; }
        public Style SelectedButtonStyle { get; set; }          

        #region OnPropertyChanged
        private string _accommodationName; 
        public string AccommodationName
        {
            get => _accommodationName;
            set
            {
                if (value != _accommodationName)
                {

                    _accommodationName = value;
                    OnPropertyChanged(nameof(AccommodationName));
                }
            }
        }

        private string _guestName;
        public string GuestName 
        {
            get => _guestName;
            set
            {
                if (value != _guestName)
                {

                    _guestName = value;
                    OnPropertyChanged(nameof(GuestName));
                }
            }
        }

        private string _guestSurame;
        public string GuestSurname 
        {
            get => _guestSurame;
            set
            {
                if (value != _guestSurame)
                {

                    _guestSurame = value;
                    OnPropertyChanged(nameof(GuestSurname));
                }
            }
        }

        private Style _searchButtonStyle;
        public Style SearchButtonStyle
        {
            get => _searchButtonStyle;
            set
            {
                if (value != _searchButtonStyle)
                {

                    _searchButtonStyle = value;
                    OnPropertyChanged(nameof(SearchButtonStyle));
                }
            }
        }

        private Style _closeButtonStyle;

        public Style CloseButtonStyle
        {
            get => _closeButtonStyle;
            set
            {
                if (value != _closeButtonStyle)
                {

                    _closeButtonStyle = value;
                    OnPropertyChanged(nameof(CloseButtonStyle));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public RelayCommand SearchReservationsCommand { get; set; }
        public RelayCommand CloseReservationsViewCommand { get; set; }
        public RelayCommand StopDemoCommand { get; set; }



        public GuestReservationsViewModel(GuestReservationsView accommodationsView, Owner owner)
        {
            InitCommands();

            _reservationService = new AccommodationReservationService();

            GuestReservationsView = accommodationsView;
            Owner = owner;
            Reservations = new ObservableCollection<AccommodationReservation>(_reservationService.GetByOwnerId(Owner.Id));

            NormalButtonStyle = Application.Current.FindResource("OwnerButtonStyle") as Style;
            SelectedButtonStyle = Application.Current.FindResource("OwnerSelectedButtonStyle") as Style;
            SearchButtonStyle = NormalButtonStyle;
            CloseButtonStyle = NormalButtonStyle;

            DemoIsOn();
        }

        #region DemoIsOn
        private async Task DemoIsOn()
        {
            if (OwnerMainViewModel.Demo)
            {
                //demo message - search
                await Task.Delay(1000);
                Window messageView1 = new MessageView("The next feature is the reservation search.", "Stop Demo Mode(Ctrl+Q)");
                messageView1.Show();
                await Task.Delay(3500);
                messageView1.Close();
                await Task.Delay(1000);

                //search
                AccommodationName = "Royal";
                await Task.Delay(1500);
                GuestName = "Milos";
                await Task.Delay(1500);
                GuestSurname = "Milosev";
                await Task.Delay(1500);
                SearchButtonStyle = SelectedButtonStyle;
                await Task.Delay(1000);
                SearchButtonStyle = NormalButtonStyle;
                List<AccommodationReservation> searchResult = _reservationService.OwnerSearch(AccommodationName, GuestName, GuestSurname, Owner.Id);
                Reservations.Clear();
                foreach (AccommodationReservation reservation in searchResult)
                {
                    Reservations.Add(reservation);
                }
                await Task.Delay(1500);

                //close window
                CloseButtonStyle = SelectedButtonStyle;
                await Task.Delay(1500);
                CloseButtonStyle = NormalButtonStyle;
                GuestReservationsView.Close();
            }
        }

        #endregion

        #region Commands

        public void Executed_SearchReservationsCommand(object obj)
        {
            List<AccommodationReservation> searchResult = _reservationService.OwnerSearch(AccommodationName, GuestName, GuestSurname, Owner.Id);
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

        private async Task StopDemo()
        {
            if (OwnerMainViewModel.Demo)
            {
                OwnerMainViewModel.CTS.Cancel();
                OwnerMainViewModel.Demo = false;

                //demo message - end
                GuestReservationsView.Close();

                Window messageDemoOver = new MessageView("The demo mode is over.", "");
                messageDemoOver.Show();
                await Task.Delay(2500);
                messageDemoOver.Close();
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
        #endregion

        public void InitCommands()
        {
            SearchReservationsCommand = new RelayCommand(Executed_SearchReservationsCommand, CanExecute_SearchReservationsCommand);
            CloseReservationsViewCommand = new RelayCommand(Executed_CloseReservationsViewCommand, CanExecute_CloseReservationsViewCommand);
            StopDemoCommand = new RelayCommand(Executed_StopDemoCommand, CanExecute_StopDemoCommand);
        }

    }
}
