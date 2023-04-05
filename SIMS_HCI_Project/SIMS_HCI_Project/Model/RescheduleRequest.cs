﻿using SIMS_HCI_Project.Serializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    public enum RescheduleRequestStatus { ACCEPTED, REJECTED, PENDING};
    public class RescheduleRequest : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationReservationId { get; set; }
        public AccommodationReservation AccommodationReservation { get; set; }
        public DateTime WantedStart { get; set; }
        public DateTime WantedEnd { get; set; }
        public RescheduleRequestStatus Status { get; set; }
        public String OwnerComment { get; set; }

        public RescheduleRequest() { }
        public RescheduleRequest(AccommodationReservation reservation, DateTime wantedStart, DateTime wantedEnd)
        {
            AccommodationReservationId = reservation.Id;
            AccommodationReservation = reservation;
            WantedStart = wantedStart;
            WantedEnd = wantedEnd;
            Status = RescheduleRequestStatus.PENDING;
            OwnerComment = "";

        }
        public RescheduleRequest(RescheduleRequest temp)
        {
            Id = temp.Id;
            AccommodationReservationId = temp.AccommodationReservationId;
            WantedStart = temp.WantedStart;
            WantedEnd = temp.WantedEnd;
            Status = temp.Status;
            OwnerComment = temp.OwnerComment;
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationReservationId.ToString(),
                WantedStart.ToString("MM/dd/yyyy"),
                WantedEnd.ToString("MM/dd/yyyy"),
                Status.ToString(),
                OwnerComment
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationReservationId = int.Parse(values[1]);
            WantedStart = DateTime.ParseExact(values[2], "MM/dd/yyyy", null);
            WantedEnd = DateTime.ParseExact(values[3], "MM/dd/yyyy", null);
            Enum.TryParse(values[4], out RescheduleRequestStatus status);
            OwnerComment = values[5];
        }
    }
}