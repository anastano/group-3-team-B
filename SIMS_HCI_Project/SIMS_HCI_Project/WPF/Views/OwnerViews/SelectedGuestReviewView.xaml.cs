using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SIMS_HCI_Project.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for SelectedGuestReviewView.xaml
    /// </summary>
    public partial class SelectedGuestReviewView : Window
    {
        public SelectedGuestReviewView(GuestReviewsView GuestReviewsView, RatingGivenByGuest selectedReview)
        {
            InitializeComponent();
            this.DataContext = new SelectedGuestReviewViewModel(this, GuestReviewsView, selectedReview);
        }
    }
}
