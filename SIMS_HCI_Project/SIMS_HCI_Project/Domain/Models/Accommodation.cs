using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum AccomodationType { APARTMENT, HOUSE, HUT };

    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public AccomodationType Type { get; set; }

        public int MaxGuests { get; set; }
        public int MinimumReservationDays { get; set; }
        public int CancellationDeadlineInDays { get; set; }
        public List<string> Images { get; set; }
        public string FirstImage { get; set; }

        public List<AccommodationReservation> Reservations { get; set; }

        public Accommodation()
        {
            MaxGuests = 1;
            MinimumReservationDays = 1;
            CancellationDeadlineInDays = 1;
            Images = new List<string>();
            Reservations = new List<AccommodationReservation>();
        }

        public Accommodation(Accommodation accommodation)
        {
            Id = accommodation.Id;
            OwnerId = accommodation.OwnerId;
            Name = accommodation.Name;
            LocationId = accommodation.LocationId;
            Location = accommodation.Location;
            Type = accommodation.Type;
            MaxGuests = accommodation.MaxGuests;
            MinimumReservationDays = accommodation.MinimumReservationDays;
            CancellationDeadlineInDays = accommodation.CancellationDeadlineInDays;
            Images = new List<string>();
            Reservations = new List<AccommodationReservation>();
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                OwnerId.ToString(),
                Name,
                LocationId.ToString(),
                Type.ToString(),
                MaxGuests.ToString(),
                MinimumReservationDays.ToString(),
                CancellationDeadlineInDays.ToString(),
                string.Join(",", Images)
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            OwnerId = Convert.ToInt32(values[1]);
            Name = values[2];
            LocationId = int.Parse(values[3]);
            Enum.TryParse(values[4], out AccomodationType type);
            Type = type;
            MaxGuests = int.Parse(values[5]);
            MinimumReservationDays = int.Parse(values[6]);
            CancellationDeadlineInDays = int.Parse(values[7]);
            Images = new List<string>(values[8].Split(","));
            FirstImage = Images.FirstOrDefault();
        }
    }
}
