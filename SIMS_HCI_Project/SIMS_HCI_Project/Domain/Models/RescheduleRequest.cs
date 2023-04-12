using System;
using System.Collections.Generic;
using System.Linq;
using SIMS_HCI_Project.Serializer;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum RescheduleRequestStatus { ACCEPTED, DENIED, PENDING };

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

        public RescheduleRequest(RescheduleRequest request)
        {
            Id = request.Id;
            AccommodationReservationId = request.AccommodationReservationId;
            WantedStart = request.WantedStart;
            WantedEnd = request.WantedEnd;
            Status = request.Status;
            OwnerComment = request.OwnerComment;
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
            Status = status;
            OwnerComment = values[5];
        }
    }
}
