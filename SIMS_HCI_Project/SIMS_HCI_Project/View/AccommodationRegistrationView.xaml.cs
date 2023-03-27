using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
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
    public partial class AccommodationRegistrationView : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        private int _ownerId;

        public Accommodation Accommodation { get; set; }
        public Location Location { get; set; }

        private AccommodationController _accommodationController;

        private string _imageURL;
        public string ImageURL
        {
            get { return _imageURL; }
            set
            {
                _imageURL = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Images { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public AccommodationRegistrationView(AccommodationController accommodationController, int ownerId)
        {
            InitializeComponent();
            DataContext = this;

            _ownerId = ownerId;
            
            Accommodation = new Accommodation();
            Location = new Location();
            Images = new ObservableCollection<string>();
            ImageURL = "";

            _accommodationController = accommodationController;

        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (Accommodation.IsValid && Location.IsValid) 
            {
                Accommodation.OwnerId = _ownerId;
                Accommodation.Images = new List<string>(Images);

                _accommodationController.Register(Accommodation, Location);

                Close();
            }
            else 
            {
                MessageBox.Show("Not all fields are filled in correctly!");
            }
                
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

            if (maxGuests > 1)
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

            if (minDays > 1)
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

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            if (IsImageURLValid)
            {
                Images.Add(ImageURL);
                ImageURL = "";
            }
        }

        private void btnRemoveImage_Click(object sender, RoutedEventArgs e)
        {
            if (lbImages.SelectedItem != null)
            {
                Images.RemoveAt(lbImages.SelectedIndex);
            }
        }

        //ImageURl validation
        private Regex urlRegex = new Regex("(http(s?)://.)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)|(^$)");

        public string Error => null;


        public string this[string propertyName]
        {
            get
            {

                if (propertyName == "ImageURL")
                {
                    Match match = urlRegex.Match(ImageURL);
                    if (!match.Success)
                    {
                        return "URL is not in valid format.";
                    }
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "ImageURL" };

        public bool IsImageURLValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
    }
}
