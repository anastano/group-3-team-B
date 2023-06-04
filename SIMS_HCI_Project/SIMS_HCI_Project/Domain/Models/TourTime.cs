﻿


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

    public class TourTime
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
            CurrentKeyPointIndex = 0;
        }

        public TourTime(int tourId, DateTime departureTime)
        {
            TourId = tourId;
            DepartureTime = departureTime;
            Status = TourStatus.NOT_STARTED;

            CurrentKeyPointIndex = 0;
            GuestAttendances = new List<GuestTourAttendance>();
        }

        public bool IsAtLastKeyPoint => this.CurrentKeyPointIndex >= this.Tour.KeyPoints.Count - 1;
        public bool IsCancellable => DateTime.Now.AddDays(2) < this.DepartureTime;
        public bool IsCompleted => this.Status == TourStatus.COMPLETED; 
        public bool IsStartable => this.Status == TourStatus.NOT_STARTED && this.DepartureTime.Date == DateTime.Today;
        public bool IsCanceled => this.Status == TourStatus.CANCELED;
        public bool IsInProgress => this.Status == TourStatus.IN_PROGRESS;

        public void Cancel()
        {
            this.Status = TourStatus.CANCELED;
        }

        public void Complete()
        {
            this.Status = TourStatus.COMPLETED;
        }

        public void Start()
        {
            this.Status = TourStatus.IN_PROGRESS;
        }

        public void Progress()
        {
            this.CurrentKeyPointIndex++;
        }
    }
}
