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
        

        public GuestRatingView(OwnerGuestRatingController ratingController, AccommodationReservation reservation, string ownerId)
        {
            InitializeComponent();
            DataContext = this;

            Rating = new OwnerGuestRating();  
            Rating.OwnerId = ownerId;
            Rating.ReservationId = reservation.Id;
            Rating.GuestId = reservation.GuestId;
            
            _ratingController = ratingController;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            _ratingController.Add(Rating);
            Close();
        }

        private void btnPlusCleanliness_Click(object sender, RoutedEventArgs e)
        {
            int cleanliness = int.Parse(txtCleanliness.Text);

            if (cleanliness < 5)
            {
                cleanliness += 1;
            }
            txtCleanliness.Text = cleanliness.ToString();
        }

        private void btnMinusCleanliness_Click(object sender, RoutedEventArgs e)
        {
            int cleanliness = int.Parse(txtCleanliness.Text);

            if (cleanliness > 1)
            {
                cleanliness -= 1;
            }
            txtCleanliness.Text = cleanliness.ToString();
        }

        private void btnPlusRuleCompliance_Click(object sender, RoutedEventArgs e)
        {
            int ruleCompliance = int.Parse(txtRuleCompliance.Text);

            if (ruleCompliance < 5)
            {
                ruleCompliance += 1;
            }
            txtRuleCompliance.Text = ruleCompliance.ToString();
        }

        private void btnMinusRuleCompliance_Click(object sender, RoutedEventArgs e)
        {
            int ruleCompliance = int.Parse(txtRuleCompliance.Text);

            if (ruleCompliance > 1)
            {
                ruleCompliance -= 1;
            }
            txtRuleCompliance.Text = ruleCompliance.ToString();
        }
    }
}
