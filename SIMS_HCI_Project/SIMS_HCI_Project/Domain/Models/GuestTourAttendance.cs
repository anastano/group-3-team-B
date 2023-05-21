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
        public int TourReservationId { get; set; }
        public TourReservation TourReservation { get; set; }
        public AttendanceStatus Status { get; set; }
        public int KeyPointJoinedId { get; set; }
        public TourKeyPoint KeyPointJoined { get; set; }

        public GuestTourAttendance() { }

        public GuestTourAttendance(int reservationId)
        {
            TourReservationId = reservationId;
            Status = AttendanceStatus.NOT_PRESENT;
        }

        public void RequestConfirmation()
        {
            this.Status = AttendanceStatus.CONFIRMATION_REQUESTED;
            this.KeyPointJoined = TourReservation.TourTime.CurrentKeyPoint;
            this.KeyPointJoinedId = this.KeyPointJoined.Id;
        }

        public void MarkPresence()
        {
            this.Status = AttendanceStatus.PRESENT;
        }

        public void MarkAbsence()
        {
            this.Status = AttendanceStatus.NEVER_SHOWED_UP;
        }
    }
}
