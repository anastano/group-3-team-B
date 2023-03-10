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
        public Location Location { get; set; }

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
        public ObservableCollection<DateTime> DepartureTimes { get; set; }

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
        public string StartKeyPointTitle { get; set; }
        public string EndKeyPointTitle { get; set; }
        public ObservableCollection<TourKeyPoint> KeyPoints { get; set; }

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

        private readonly TourController _tourController;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourInputView(TourController tourController, Guide guide)
        {
            InitializeComponent();

            _guide = guide;

            Tour = new Tour(guide);
            Location = new Location();
            DepartureDate = new DateTime(2023, 1, 1);
            DepartureTime = new TimeOnly();

            Images = new ObservableCollection<string>();
            DepartureTimes = new ObservableCollection<DateTime>();
            KeyPoints = new ObservableCollection<TourKeyPoint>();

            _tourController = tourController;

            DataContext = this;
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            if (ImageURL != "")
            {
                Images.Add(ImageURL);
                ImageURL = "";
            }
        }

        private void btnAddKeyPoint_Click(object sender, RoutedEventArgs e)
        {
            if(KeyPointTitle != "")
            {
                KeyPoints.Add(new TourKeyPoint(KeyPointTitle));
                KeyPointTitle = "";
            }
        }

        private void btnAddDepartureTime_Click(object sender, RoutedEventArgs e)
        {
            DateTime element = DepartureDate;
            element += DepartureTime.ToTimeSpan();
            DepartureTimes.Add(element);

            DepartureDate = new DateTime(2023, 1, 1);
            DepartureTime = new TimeOnly(0, 0);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            KeyPoints.Insert(0, new TourKeyPoint(StartKeyPointTitle));
            KeyPoints.Add(new TourKeyPoint(EndKeyPointTitle));

            _tourController.Save(Tour, Location, new List<TourKeyPoint>(KeyPoints), new List<DateTime>(DepartureTimes), new List<string>(Images));

            //this.Close();
        }
    }
}
