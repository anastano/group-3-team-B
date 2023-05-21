﻿using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public Location() { }

        public Location(string city, string country)
        {
            City = city;
            Country = country;
        }

        public override bool Equals(object? obj)
        {
            var location = obj as Location;

            if (location == null) return false;

            return this.City.Equals(location.City) && this.Country.Equals(location.Country);
        }
    }
}
