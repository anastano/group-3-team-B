using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum AttendanceStatus { NOT_PRESENT, CONFIRMATION_REQUESTED, PRESENT, NEVER_SHOWED_UP}

    public class GuestTourAttendance
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public Guest2 Guest { get; set; }
        public int TourTimeId { get; set; }
        public TourTime TourTime { get; set; }
        public TourReservation TourReservation { get; set; }
        public AttendanceStatus Status { get; set; }
        public int KeyPointJoinedId { get; set; }
        public TourKeyPoint KeyPointJoined { get; set; }

        public GuestTourAttendance() { }

        public GuestTourAttendance(int guestId, int tourTimeId)
        {
            GuestId = guestId;
            TourTimeId = tourTimeId;
            Status = AttendanceStatus.NOT_PRESENT;
        }
    }
}
