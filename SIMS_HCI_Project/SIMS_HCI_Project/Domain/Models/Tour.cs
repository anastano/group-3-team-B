using SIMS_HCI_Project.Model;
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
    public class Tour
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

        public void AssignToTourTimes(List<TourTime> tourTimes)
        {
            foreach (TourTime tourTime in tourTimes)
            {
                tourTime.TourId = this.Id;
                tourTime.Tour = this;
            }
        }
    }
}
