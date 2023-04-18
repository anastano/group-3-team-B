using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum TourStatus { NOT_STARTED, IN_PROGRESS, COMPLETED, CANCELED };

    public class TourTime : ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        public DateTime DepartureTime { get; set; }
        public TourStatus Status { get; set; }
        public TourKeyPoint CurrentKeyPoint { get; set; }
        public int CurrentKeyPointIndex { get; set; }
        public List<GuestTourAttendance> GuestAttendances { get; set; }
        public int Available { get; set; }

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
            string[] csvValues = { Id.ToString(), TourId.ToString(), DepartureTime.ToString("M/d/yyyy h:mm:ss tt"), Status.ToString(), CurrentKeyPointIndex.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId = Convert.ToInt32(values[1]);
            DepartureTime = DateTime.ParseExact(values[2], "M/d/yyyy h:mm:ss tt", null);
            Enum.TryParse(values[3], out TourStatus status);
            Status = status;
            CurrentKeyPointIndex = Convert.ToInt32(values[4]);
        }

        public bool IsAtLastKeyPoint()
        {
            return this.CurrentKeyPointIndex >= this.Tour.KeyPoints.Count - 1;
        }
    }
}
