using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SIMS_HCI_Project.View
{
    /// <summary>
    /// Interaction logic for ReservationRescheduleView.xaml
    /// </summary>
    public partial class ReservationRescheduleView : Window, INotifyPropertyChanged
    {
        private AccommodationReservationController _accommodationReservationController;
        private RescheduleRequestController _rescheduleRequestController;
        public AccommodationReservation Reservation { get; set; }

        private DateTime _wantedStart;
        public DateTime WantedStart
        {
            get => _wantedStart;
            set
            {
                if (value != _wantedStart)
                {
                    _wantedStart = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _wantedEnd;
        public DateTime WantedEnd
        {
            get => _wantedEnd;
            set
            {
                if (value != _wantedEnd)
                {
                    _wantedEnd = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ReservationRescheduleView(AccommodationReservationController accommodationReservationController, AccommodationReservation reservation, Guest1 guest)
        {
            InitializeComponent();
            this.DataContext = this;
            _accommodationReservationController = accommodationReservationController;
            _rescheduleRequestController = new RescheduleRequestController();
            reservation.Guest = guest;
            Reservation = reservation;
        }

        private void btnSendRequest_Click(object sender, RoutedEventArgs e)
        {
            _rescheduleRequestController.Add(new RescheduleRequest(Reservation, WantedStart, WantedEnd));
        }
    }
}
