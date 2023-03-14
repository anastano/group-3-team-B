using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class TourInputView : Window, INotifyPropertyChanged
    {
        private Guide _guide;

        public Tour Tour { get; set; }

        #region newDepartureDate
        private DateTime _departureDate;
        public DateTime DepartureDate
        {
            get { return _departureDate; }
            set
            {
                _departureDate = value;
                OnPropertyChanged();
            }
        }
        private TimeOnly _departureTime;
        public TimeOnly DepartureTime
        {
            get { return _departureTime; }
            set
            {
                _departureTime = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public ObservableCollection<DateTime> DepartureTimes { get; set; }

        #region newKeyPoint
        private string _keyPointTitle;
        public string KeyPointTitle
        {
            get { return _keyPointTitle; }
            set
            {
                _keyPointTitle = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public ObservableCollection<TourKeyPoint> KeyPoints { get; set; }

        #region newImage
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
        #endregion
        public ObservableCollection<string> Images { get; set; }

        private readonly TourController _tourController;

        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public TourInputView(TourController tourController, Guide guide)
        {
            InitializeComponent();

            _guide = guide;

            Tour = new Tour(guide);
            DepartureDate = new DateTime(2023, 1, 1);
            DepartureTime = new TimeOnly();

            Images = new ObservableCollection<string>();
            DepartureTimes = new ObservableCollection<DateTime>();
            KeyPoints = new ObservableCollection<TourKeyPoint>();

            _tourController = tourController;

            lblDepartureTimesErrorMessage.Content = Tour["DepartureTimes"];
            lblKeyPointsErrorMessage.Content = Tour["KeyPoints"];

            DataContext = this;
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            if (ImageURL != "")
            {
                Images.Add(ImageURL);
                ImageURL = "";
            }
            UpdateSubmitButtonStatus();
        }

        private void btnAddKeyPoint_Click(object sender, RoutedEventArgs e)
        {
            if(KeyPointTitle != "")
            {
                KeyPoints.Add(new TourKeyPoint(KeyPointTitle));
                Tour.KeyPoints.Add(new TourKeyPoint(KeyPointTitle));
                lblKeyPointsErrorMessage.Content = Tour["KeyPoints"]; // TODO: Try to do this with binding only
                KeyPointTitle = "";
            }
            UpdateSubmitButtonStatus();
        }

        private void btnAddDepartureTime_Click(object sender, RoutedEventArgs e)
        {
            DateTime newDepartureTime = DepartureDate;
            newDepartureTime += DepartureTime.ToTimeSpan();

            DepartureTimes.Add(newDepartureTime);
            Tour.DepartureTimes.Add(new TourTime(newDepartureTime));
            lblDepartureTimesErrorMessage.Content = Tour["DepartureTimes"]; // TODO: Try to do this with binding only

            DepartureDate = new DateTime(2023, 1, 1);
            DepartureTime = new TimeOnly(0, 0);

            UpdateSubmitButtonStatus();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Tour.Images.AddRange(Images);
            _tourController.Save(Tour);

            this.Close();
        }

        private void btnRemoveDepartureTime_Click(object sender, RoutedEventArgs e)
        {
            if (lbDepartureTimes.SelectedItem != null)
            {
                Tour.DepartureTimes.RemoveAt(lbDepartureTimes.SelectedIndex);
                DepartureTimes.RemoveAt(lbDepartureTimes.SelectedIndex);
                lblDepartureTimesErrorMessage.Content = Tour["DepartureTimes"];
            }
            UpdateSubmitButtonStatus();
        }

        private void btnRemoveKeyPoint_Click(object sender, RoutedEventArgs e)
        {
            if (lbKeyPoints.SelectedItem != null)
            {
                Tour.KeyPoints.RemoveAt(lbKeyPoints.SelectedIndex);
                KeyPoints.RemoveAt(lbKeyPoints.SelectedIndex);
                lblKeyPointsErrorMessage.Content = Tour["KeyPoints"];
            }
            UpdateSubmitButtonStatus();
        }

        private void btnRemoveImage_Click(object sender, RoutedEventArgs e)
        {
            if (lbImages.SelectedItem != null)
            {
                Images.RemoveAt(lbImages.SelectedIndex);
            }
            UpdateSubmitButtonStatus();
        }

        private void ButtonAvailableCheck(object sender, RoutedEventArgs e)
        {
            UpdateSubmitButtonStatus();
        }

        private void UpdateSubmitButtonStatus()
        {
            if (Tour.IsValid && Tour.Location.IsValid) btnSubmit.IsEnabled = true;
            else btnSubmit.IsEnabled = false;
        }
    }
}
