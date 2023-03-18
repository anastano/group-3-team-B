using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    public enum TourStatus { NOT_STARTED, IN_PROGRESS, COMPLETED, CANCELED };

    public class TourTime : ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        public DateTime DepartureTime { get; set; }
        private TourStatus _status;
        public TourStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }
        private TourKeyPoint _currentKeyPoint;
        public TourKeyPoint CurrentKeyPoint
        {
            get { return _currentKeyPoint; }
            set
            {
                _currentKeyPoint = value;
                OnPropertyChanged();
            }
        }
        public int _currentKeyPointIndex;
        public int CurrentKeyPointIndex
        {
            get { return _currentKeyPointIndex; }
            set
            {
                _currentKeyPointIndex = value;
                OnPropertyChanged();
            }
        }
        public List<GuestTourAttendance> GuestAttendances { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourTime()
        {
            GuestAttendances = new List<GuestTourAttendance>();
        }

        public TourTime(DateTime departureTime)
        {
            DepartureTime = departureTime;
            Status = TourStatus.NOT_STARTED;

            GuestAttendances = new List<GuestTourAttendance>();
        }

        public TourTime(int tourId, DateTime departureTime)
        {
            TourId = tourId;
            DepartureTime = departureTime;
            Status = TourStatus.NOT_STARTED;

            GuestAttendances = new List<GuestTourAttendance>();
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourId.ToString(), DepartureTime.ToString(), Status.ToString(), CurrentKeyPointIndex.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId = Convert.ToInt32(values[1]);
            DepartureTime = Convert.ToDateTime(values[2]);
            Enum.TryParse(values[3], out TourStatus status);
            Status = status;
            CurrentKeyPointIndex = Convert.ToInt32(values[4]);
        }
    }
}
