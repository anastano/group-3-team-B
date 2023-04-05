using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static System.Net.Mime.MediaTypeNames;

namespace SIMS_HCI_Project.View
{
    /// <summary>
    /// Interaction logic for GuestRatingView.xaml
    /// </summary>
    public partial class GuestRatingView : Window
    {
       
        public OwnerGuestRating Rating { get; set; }

        private OwnerGuestRatingController _ratingController;
        

        public GuestRatingView(OwnerGuestRatingController ratingController, AccommodationReservation reservation, int ownerId)
        {
            InitializeComponent();
            DataContext = this;

            Rating = new OwnerGuestRating();  
            Rating.OwnerId = ownerId;
            Rating.ReservationId = reservation.Id;
            Rating.GuestId = reservation.GuestId;
            Rating.AccommodationReservation = reservation;
            
            _ratingController = ratingController;
        }

        private void btnRate_Click(object sender, RoutedEventArgs e)
        {
            _ratingController.Add(Rating);
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.R))
                btnRate_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.Escape))
                btnCancel_Click(sender, e);
        }
    }
}
