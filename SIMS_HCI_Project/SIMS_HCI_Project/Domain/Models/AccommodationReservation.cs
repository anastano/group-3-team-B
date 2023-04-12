﻿using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum AccommodationReservationStatus { RESERVED, CANCELLED, RESCHEDULED, COMPLETED };

    public class AccommodationReservation : ISerializable
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

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                GuestId.ToString(),
                Start.ToString("MM/dd/yyyy"),
                End.ToString("MM/dd/yyyy"),
                GuestNumber.ToString(),
                Status.ToString()

            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationId = int.Parse(values[1]);
            GuestId = int.Parse(values[2]);
            Start = DateTime.ParseExact(values[3], "MM/dd/yyyy", null);
            End = DateTime.ParseExact(values[4], "MM/dd/yyyy", null);
            GuestNumber = int.Parse(values[5]);
            Enum.TryParse(values[6], out AccommodationReservationStatus status);
            Status = status;

        }
    }
}