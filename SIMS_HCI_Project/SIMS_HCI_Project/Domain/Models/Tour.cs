using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class Tour : IDataErrorInfo
    {
        public int Id { get; set; }
        public int GuideId { get; set; }
        public Guide Guide { get; set; }
        public string Title { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxGuests { get; set; }
        public List<int> KeyPointsIds { get; set; }
        public List<TourKeyPoint> KeyPoints { get; set; }
        public List<TourTime> DepartureTimes { get; set; }
        public int Duration { get; set; }
        public List<string> Images { get; set; }

        public Tour() {

            Location = new Location();
            Images = new List<string>();
            DepartureTimes = new List<TourTime>();
            KeyPoints = new List<TourKeyPoint>();
            KeyPointsIds = new List<int>();
        }

        public Tour(Guide guide)
        {
            Guide = guide;
            GuideId = guide.Id;


            Location = new Location();
            Images = new List<string>();
            DepartureTimes = new List<TourTime>();
            KeyPoints = new List<TourKeyPoint>();
            KeyPointsIds = new List<int>();
        }

        public Tour(string title, int locationId, string description, string language, int maxGuestNumber, int duration)
        {
            Title = title;
            LocationId = locationId;
            Description = description;
            Language = language;
            MaxGuests = maxGuestNumber;
            Duration = duration;

            Location = new Location();
            Images = new List<string>();
            DepartureTimes = new List<TourTime>();
            KeyPoints = new List<TourKeyPoint>();
            KeyPointsIds = new List<int>();
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch(columnName)
                {
                    case "Title":
                        if (string.IsNullOrEmpty(Title))
                            return "Title is required";
                        break;
                    case "City":
                        if (string.IsNullOrEmpty(Location.City))
                            return "City is required";
                        break;
                    case "Country":
                        if (string.IsNullOrEmpty(Location.Country))
                            return "Country is required";
                        break;
                    case "Description":
                        if (string.IsNullOrEmpty(Description))
                            return "Description is required";
                        break;
                    case "Language":
                        if (string.IsNullOrEmpty(Language))
                            return "Language is required";
                        break;
                    case "MaxGuests":
                        if (MaxGuests < 1)
                            return "MaxGuests is required and cannot be less than 1";
                        break;
                    case "Duration":
                        if (Duration < 1)
                            return "Duration is required";
                        break;
                    case "DepartureTimes":
                        if (DepartureTimes.Count < 1)
                            return "Tour must have at least one Departure Time";
                        break;
                    case "KeyPoints":
                        if (KeyPoints.Count < 2)
                            return "Tour must have at least two Key Points";
                        break;
                }
                return null;
            }

        }

        private readonly string[] _validatedProperties = { "Title", "Description", "Language", "MaxGuests", "Duration", "DepartureTimes", "KeyPoints" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

    }
}
