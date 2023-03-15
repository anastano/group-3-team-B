using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.Model
{
    public enum AttendanceStatus { NOT_PRESENT, CONFIRMATION_REQUESTED, PRESENT, NEVER_SHOWED_UP}

    public class GuestTourAttendance : ISerializable
    {
        public int Id { get; set; }
        public string GuestId { get; set; }
        //public Guest2 Guest { get; set; }
        public int TourTimeId { get; set; }
        public TourTime TourTime { get; set; }
        public AttendanceStatus Status { get; set; }
        public int KeyPointJoinedId { get; set; }
        public TourKeyPoint KeyPointJoined { get; set; }

        public GuestTourAttendance() { }

        public GuestTourAttendance(string guestId, int tourTimeId)
        {
            GuestId = guestId;
            TourTimeId = tourTimeId;
            Status = AttendanceStatus.NOT_PRESENT;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId, TourTimeId.ToString(), Status.ToString(), KeyPointJoinedId.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = values[1];
            TourTimeId = Convert.ToInt32(values[2]);
            Enum.TryParse(values[3], out AttendanceStatus status);
            Status = status;
            KeyPointJoinedId = Convert.ToInt32(values[4]);
        }
    }
}
