using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum AccommodationReservationStatus { RESERVED, CANCELLED, RESCHEDULED, COMPLETED };

    public class AccommodationReservation
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; }
        public int GuestId { get; set; }
        public Guest1 Guest { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int GuestNumber { get; set; }
        public AccommodationReservationStatus Status { get; set; }
        public bool isRated { get; set; }

        public AccommodationReservation() { }

        public AccommodationReservation(int accommodationId, int guestId, DateTime start, DateTime end, int guestNumber)
        {
            Id = -1;
            AccommodationId = accommodationId;
            GuestId = guestId;
            Start = start;
            End = end;
            GuestNumber = guestNumber;
            Status = AccommodationReservationStatus.RESERVED;
        }

        public AccommodationReservation(AccommodationReservation reservation)
        {
            Id = reservation.Id;
            AccommodationId = reservation.AccommodationId;
            GuestId = reservation.GuestId;
            Start = reservation.Start;
            End = reservation.End;
            GuestNumber = reservation.GuestNumber;
            Status = reservation.Status;
        }
    }
}
