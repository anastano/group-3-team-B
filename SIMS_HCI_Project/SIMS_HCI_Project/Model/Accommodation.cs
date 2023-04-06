﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;
using SIMS_HCI_Project.Serializer;


namespace SIMS_HCI_Project.Model
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
        public List<string> Images { get; set; } //[Maybe] Change to List<URI>

        public List<AccommodationReservation> Reservations { get; set; }
        public Accommodation() 
        {
            MaxGuests = 1;
            MinimumReservationDays = 1;
            CancellationDeadlineInDays = 1;
            Images = new List<string>();
        }

 

        //ORDER IS IMPORTANT, optional parametar at the end
       public Accommodation(int id, int ownerId, string name, int locationId, Location location, AccomodationType type, int maxGuests, int minimumReservationDays,  int cancellationDeadlineInDays = 1)
        {
            Id = id;
            OwnerId = ownerId;
            Name = name;
            LocationId = locationId;
            Location = location; // check if this is needed
            Type = type;
            MaxGuests = maxGuests;
            MinimumReservationDays = minimumReservationDays; // default --> 1
            CancellationDeadlineInDays = cancellationDeadlineInDays;
            Images = new List<string>();  //change if needed
        }

        public Accommodation(Accommodation temp)
        {
            Id = temp.Id;
            OwnerId = temp.OwnerId;
            Name = temp.Name;
            LocationId = temp.LocationId;
            Location = temp.Location;
            Type = temp.Type;
            MaxGuests = temp.MaxGuests;
            MinimumReservationDays = temp.MinimumReservationDays;
            CancellationDeadlineInDays = temp.CancellationDeadlineInDays;
            Images = temp.Images;

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
                string.Join(",", Images) // --> [Maybe] separate CSV file

            };
            return csvValues;
        }
        
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            OwnerId= Convert.ToInt32(values[1]);
            Name = values[2];
            LocationId = int.Parse(values[3]);
            Enum.TryParse(values[4], out AccomodationType type);
            Type = type;
            MaxGuests = int.Parse(values[5]);
            MinimumReservationDays = int.Parse(values[6]);
            CancellationDeadlineInDays = int.Parse(values[7]);
            Images = new List<string>(values[8].Split(",")); // --> [Maybe] separate CSV file
        }


        public static string ConvertAccommodationTypeToString(AccomodationType type) 
        {
            if (type == AccomodationType.APARTMENT) {
                return "APARTMENT";
            }else if(type == AccomodationType.HOUSE) 
            {
                return "HOUSE";
            }
            else 
            {
                return "HUT";
            }
        }
    }
}
