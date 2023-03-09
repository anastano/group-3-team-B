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
    /// Interaction logic for AccommodationRegistrationView.xaml
    /// </summary>
    public partial class AccommodationRegistrationView : Window, INotifyPropertyChanged
    {
        private string _ownerId;

        public Accommodation Accommodation { get; set; }
        public Location Location { get; set; }

        private AccommodationController _accommodationController;
        private LocationController _locationController = new LocationController(); //should this be passed as parameter???

        private string _pictureURL;
        public string PictureURL
        {
            get { return _pictureURL; }
            set
            {
                _pictureURL = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Pictures { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public AccommodationRegistrationView(AccommodationController accommodationController, string ownerId)
        {
            InitializeComponent();
            DataContext = this;

            _ownerId = ownerId;
            
            Accommodation = new Accommodation();   //check if it is needed to initialize default values
            Location = new Location();
            Pictures = new ObservableCollection<string>();

            _accommodationController = accommodationController;

        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
 
            _accommodationController.Register(Accommodation, _ownerId, Location, new List<string>(Pictures));

            Close();
        }

        private void btnPlusGuest_Click(object sender, RoutedEventArgs e)
        {
            int maxGuests = int.Parse(txtMaxGuestNumber.Text);
            maxGuests += 1;
            txtMaxGuestNumber.Text = maxGuests.ToString();
        }

        private void btnMinusGuest_Click(object sender, RoutedEventArgs e)
        {
            int maxGuests = int.Parse(txtMaxGuestNumber.Text);

            if (maxGuests > 0)
            {
                maxGuests -= 1;
            }
                txtMaxGuestNumber.Text = maxGuests.ToString();
        }

        private void btnPlusMinDays_Click(object sender, RoutedEventArgs e)
        {
            int minDays = int.Parse(txtMinDaysNumber.Text);
            minDays += 1;
            txtMinDaysNumber.Text = minDays.ToString();
        }

        private void btnMinusMinDays_Click(object sender, RoutedEventArgs e)
        {
            int minDays = int.Parse(txtMinDaysNumber.Text);

            if (minDays > 0)
            {
                minDays -= 1;
            }
            txtMinDaysNumber.Text = minDays.ToString();
        }

        private void btnPlusCancellationDays_Click(object sender, RoutedEventArgs e)
        {
            int cancellationDays = int.Parse(txtCancellationDeadLineInDays.Text);
            cancellationDays += 1;
            txtCancellationDeadLineInDays.Text = cancellationDays.ToString();

        }

        private void btnMinusCancellationDays_Click(object sender, RoutedEventArgs e)
        {
            int cancellationDays = int.Parse(txtCancellationDeadLineInDays.Text);

            if (cancellationDays > 0)
            {
                cancellationDays -= 1;
            }
            txtCancellationDeadLineInDays.Text = cancellationDays.ToString();
        }

        private void btnAddPicture_Click(object sender, RoutedEventArgs e)
        {
            if (PictureURL != "")
            {
                Pictures.Add(PictureURL);
                PictureURL = "";
            }
        }
    }
}
